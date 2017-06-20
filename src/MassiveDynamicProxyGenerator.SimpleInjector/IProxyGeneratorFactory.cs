using System;
using System.Collections.Generic;
using System.Text;

namespace MassiveDynamicProxyGenerator.SimpleInjector
{
    /// <summary>
    /// Interface representes factory for crate <see cref="IProxygGenerator"/>
    /// for Simple Injector extensions.
    /// </summary>
    /// <seealso cref="IProxygGenerator"/>
    /// <seealso cref="ProxygGenerator"/>
    /// <seealso cref="ContainerExtensions"/>
    /// <seealso cref="MassiveDynamicProxyGenerator.SimpleInjector.Dangerous.DangerousContainerExtensions"/>
    /// <seealso cref="ProxyGeneratorFactory"/>
    public interface IProxyGeneratorFactory
    {
        /// <summary>
        /// Gets the instance of <see cref="IProxygGenerator"/>.
        /// </summary>
        /// <returns>Instance <see cref="IProxygGenerator"/>.</returns>
        IProxygGenerator GetInstance();
    }
}
