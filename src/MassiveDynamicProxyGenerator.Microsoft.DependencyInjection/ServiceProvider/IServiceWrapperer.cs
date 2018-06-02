using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MassiveDynamicProxyGenerator.Microsoft.DependencyInjection.ServiceProvider
{
    /// <summary>
    /// Microsoft.Extensions.DependencyInjection service wrapperer.
    /// </summary>
    public interface IServiceWrapperer
    {
        /// <summary>
        /// Provides the instance or wrap this.
        /// </summary>
        /// <param name="serviceType">Type of the requested service.</param>
        /// <param name="realInstance">The real instance or <c>null</c>.</param>
        /// <param name="aspServiceProvider">The ASP service provider.</param>
        /// <param name="proxyGenerator">The proxy generator.</param>
        /// <returns>Real service or wrapper.</returns>
        object ProvideInstance(Type serviceType, object realInstance, IServiceProvider aspServiceProvider, IProxyGenerator proxyGenerator);
    }
}