using MassiveDynamicProxyGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

namespace SampleWebApplication.IocExtensions
{
    public class ServiceProviderInterceptor : IInterceptor
    {
        private readonly IServiceProvider serviceProvider;

        public ServiceProviderInterceptor(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            this.serviceProvider = serviceProvider;
        }

        public void Intercept(IInvocation invocation, bool isDynamicInterception)
        {
            TypeInfo info = invocation.ReturnType.GetTypeInfo();
            if (info.IsInterface || info.IsClass)
            {
                invocation.ReturnValue = this.serviceProvider.GetService(invocation.ReturnType);
            }
        }
    }
}
