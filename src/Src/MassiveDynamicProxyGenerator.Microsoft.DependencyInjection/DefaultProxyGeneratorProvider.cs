﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MassiveDynamicProxyGenerator.Microsoft.DependencyInjection
{
    internal class DefaultProxyGeneratorProvider : IProxyGeneratorProvider
    {
        private readonly IProxyGenerator proxygGenerator;

        public DefaultProxyGeneratorProvider()
        {
            this.proxygGenerator = new ProxyGenerator();
        }

        public IProxyGenerator GetProxyGenerator(IServiceProvider serviceProvider)
        {
            return this.proxygGenerator;
        }
    }
}
