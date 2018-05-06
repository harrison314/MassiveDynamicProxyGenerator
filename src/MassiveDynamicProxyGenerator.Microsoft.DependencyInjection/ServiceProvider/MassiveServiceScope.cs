using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MassiveDynamicProxyGenerator.Microsoft.DependencyInjection.ServiceProvider
{
    internal class MassiveServiceScope : IServiceScope
    {
        private readonly IServiceScope serviceScope;
        private readonly IProxygGenerator proxygGenerator;

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
                    return new MassiveServiceProvider(serviceProvider, this.proxygGenerator);
                }
            }
        }

        public MassiveServiceScope(IServiceScope serviceScope, IProxygGenerator proxygGenerator)
        {
            this.serviceScope = serviceScope;
            this.proxygGenerator = proxygGenerator;
        }

        public void Dispose()
        {
            this.serviceScope.Dispose();
        }
    }
}
