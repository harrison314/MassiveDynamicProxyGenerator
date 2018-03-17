using System;
using System.Collections.Generic;
using System.Text;

namespace MassiveDynamicProxyGenerator.Microsoft.DependencyInjection
{
    /// <summary>
    /// Settings for MassiveDynamicProxyGenerator with Microsoft.Extensions.DependencyInjection.
    /// </summary>
    public static class MassiveDynamicProxyGeneratorDiSettings
    {
        /// <summary>
        /// Gets Proxy generator provider.
        /// </summary>
        /// <value>
        /// The proxy generator provider.
        /// </value>
        internal static IProxyGeneratorProvider ProxyGeneratorProvider
        {
            get;
            private set;
        }

        static MassiveDynamicProxyGeneratorDiSettings()
        {
            ProxyGeneratorProvider = new DefaultProxyGeneratorProvider();
        }

        /// <summary>
        /// Set default proxy generator provider.
        /// </summary>
        /// <param name="provider">The proxy generator provider.</param>
        /// <exception cref="ArgumentNullException">provider</exception>
        /// <seealso cref="IProxyGeneratorProvider"/>
        public static void SetProxyGeneratorProvider(IProxyGeneratorProvider provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            ProxyGeneratorProvider = provider;
        }

        /// <summary>
        /// Set default proxy generator provider.
        /// </summary>
        /// <param name="provider">The proxy generator provider as function.</param>
        /// <exception cref="ArgumentNullException">provider</exception>
        /// <seealso cref="IProxyGeneratorProvider"/>
        public static void SetProxyGeneratorProvider(Func<IServiceProvider, IProxygGenerator> provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            ProxyGeneratorProvider = new FuncProxyGeneratorProvider(provider);
        }
    }
}
