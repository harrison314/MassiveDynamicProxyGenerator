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

        public IProxyGenerator GetInstance()
        {
            return new ProxyGenerator();
        }
    }
}
