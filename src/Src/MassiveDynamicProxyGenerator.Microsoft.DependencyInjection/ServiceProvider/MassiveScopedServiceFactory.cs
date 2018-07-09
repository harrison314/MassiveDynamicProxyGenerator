using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MassiveDynamicProxyGenerator.Microsoft.DependencyInjection.ServiceProvider
{
    internal class MassiveScopedServiceFactory : IServiceScopeFactory
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly IProxyGenerator proxygGenerator;
        private readonly IServiceWrapperer serviceWraperer;

        public MassiveScopedServiceFactory(IServiceScopeFactory serviceScopeFactory, IProxyGenerator proxygGenerator, IServiceWrapperer serviceWraperer )
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.proxygGenerator = proxygGenerator;
            this.serviceWraperer = serviceWraperer;
        }

        public IServiceScope CreateScope()
        {
            IServiceScope serviceScope = this.serviceScopeFactory.CreateScope();
            if (serviceScope is MassiveServiceScope)
            {
                return serviceScope;
            }
            else
            {
                return new MassiveServiceScope(serviceScope, this.proxygGenerator, this.serviceWraperer);
            }
        }
    }
}
