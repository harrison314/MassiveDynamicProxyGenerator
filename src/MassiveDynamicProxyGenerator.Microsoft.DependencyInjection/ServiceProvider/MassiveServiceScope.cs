using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MassiveDynamicProxyGenerator.Microsoft.DependencyInjection.ServiceProvider
{
    internal class MassiveServiceScope : IServiceScope
    {
        private readonly IServiceScope serviceScope;
        private readonly IProxyGenerator proxygGenerator;
        private readonly IServiceWrapperer serviceWraperer;

        public IServiceProvider ServiceProvider
        {
            get
            {
                IServiceProvider serviceProvider = this.serviceScope.ServiceProvider;
                if(serviceProvider is MassiveServiceProvider)
                {
                    return serviceProvider;
                }
                else
                {
                    return new MassiveServiceProvider(serviceProvider, this.proxygGenerator, this.serviceWraperer);
                }
            }
        }

        public MassiveServiceScope(IServiceScope serviceScope, IProxyGenerator proxygGenerator, IServiceWrapperer serviceWraperer)
        {
            this.serviceScope = serviceScope;
            this.proxygGenerator = proxygGenerator;
            this.serviceWraperer = serviceWraperer;
        }

        public void Dispose()
        {
            this.serviceScope.Dispose();
        }
    }
}
