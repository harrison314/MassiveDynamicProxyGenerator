using MassiveDynamicProxyGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SampleWebApplication.Services.Interceptors
{
    public class ServiceProviderInterceptor : IInterceptor
    {
        private readonly IServiceProvider serviceProvider;

        public ServiceProviderInterceptor(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public void Intercept(IInvocation invocation)
        {
            TypeInfo info = invocation.ReturnType.GetTypeInfo();
            if (info.IsInterface || info.IsClass)
            {
                invocation.ReturnValue = this.serviceProvider.GetService(invocation.ReturnType);
            }
        }
    }
}
