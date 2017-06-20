using System;
using System.Collections.Generic;
using System.Text;

namespace MassiveDynamicProxyGenerator.SimpleInjector
{
    internal class DefaultProxyGeneratorFactory : IProxyGeneratorFactory
    {
        public DefaultProxyGeneratorFactory()
        {
        }

        public IProxygGenerator GetInstance()
        {
            return new ProxygGenerator();
        }
    }
}
