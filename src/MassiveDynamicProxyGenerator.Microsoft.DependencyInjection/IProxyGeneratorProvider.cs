using System;
using System.Collections.Generic;
using System.Text;

namespace MassiveDynamicProxyGenerator.Microsoft.DependencyInjection
{
    /// <summary>
    /// Interface reprsentes provider for <see cref="IProxygGenerator"/>.
    /// </summary>
    /// <seealso cref="IProxygGenerator"/>
    public interface IProxyGeneratorProvider
    {
        /// <summary>
        /// Get or create <see cref="IProxygGenerator"/> instance.
        /// </summary>
        /// <param name="serviceProvider">The service provider from IoC.</param>
        /// <returns>Instance of prxy generator.</returns>
        IProxygGenerator GetProxyGenerator(IServiceProvider serviceProvider);
    }
}
