using System;
using System.Collections.Generic;
using System.Text;

namespace MassiveDynamicProxyGenerator.Microsoft.DependencyInjection
{
    internal class FuncProxyGeneratorProvider : IProxyGeneratorProvider
    {
        private readonly Func<IServiceProvider, IProxygGenerator> provider;

        public FuncProxyGeneratorProvider(Func<IServiceProvider, IProxygGenerator> provider)
        {
            this.provider = provider;
        }

        public IProxygGenerator GetProxyGenerator(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            return this.provider.Invoke(serviceProvider);
        }
    }
}
