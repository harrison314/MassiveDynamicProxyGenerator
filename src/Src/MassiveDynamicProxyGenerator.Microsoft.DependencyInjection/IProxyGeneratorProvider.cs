﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MassiveDynamicProxyGenerator.Microsoft.DependencyInjection
{
    /// <summary>
    /// Interface represents provider for <see cref="IProxyGenerator"/>.
    /// </summary>
    /// <seealso cref="IProxyGenerator"/>
    public interface IProxyGeneratorProvider
    {
        /// <summary>
        /// Get or create <see cref="IProxyGenerator"/> instance.
        /// </summary>
        /// <param name="serviceProvider">The service provider from IoC.</param>
        /// <returns>Instance of proxy generator.</returns>
        IProxyGenerator GetProxyGenerator(IServiceProvider serviceProvider);
    }
}
