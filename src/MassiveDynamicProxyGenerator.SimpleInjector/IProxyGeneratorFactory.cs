using System;
using System.Collections.Generic;
using System.Text;

namespace MassiveDynamicProxyGenerator.SimpleInjector
{
    /// <summary>
    /// Interface representes factory for crate <see cref="IProxyGenerator"/>
    /// for Simple Injector extensions.
    /// </summary>
    /// <seealso cref="IProxyGenerator"/>
    /// <seealso cref="ProxyGenerator"/>
    /// <seealso cref="ContainerExtensions"/>
    /// <seealso cref="MassiveDynamicProxyGenerator.SimpleInjector.Dangerous.DangerousContainerExtensions"/>
    /// <seealso cref="ProxyGeneratorFactory"/>
    public interface IProxyGeneratorFactory
    {
        /// <summary>
        /// Gets the instance of <see cref="IProxyGenerator"/>.
        /// </summary>
        /// <returns>Instance <see cref="IProxyGenerator"/>.</returns>
        IProxyGenerator GetInstance();
    }
}
