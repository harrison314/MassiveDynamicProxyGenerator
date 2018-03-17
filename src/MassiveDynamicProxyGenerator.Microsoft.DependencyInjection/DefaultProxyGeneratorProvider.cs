using System;
using System.Collections.Generic;
using System.Text;

namespace MassiveDynamicProxyGenerator.Microsoft.DependencyInjection
{
    internal class DefaultProxyGeneratorProvider : IProxyGeneratorProvider
    {
        private readonly IProxygGenerator proxygGenerator;

        public DefaultProxyGeneratorProvider()
        {
            this.proxygGenerator = new ProxygGenerator();
        }

        public IProxygGenerator GetProxyGenerator(IServiceProvider serviceProvider)
        {
            return this.proxygGenerator;
        }
    }
}
