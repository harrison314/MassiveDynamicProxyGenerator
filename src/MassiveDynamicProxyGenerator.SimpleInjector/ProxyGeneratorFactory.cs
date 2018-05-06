using System;
using System.Collections.Generic;
using System.Text;

namespace MassiveDynamicProxyGenerator.SimpleInjector
{
    /// <summary>
    /// Class representes singlton for generate <see cref="IProxyGenerator"/>.
    /// </summary>
    public static class ProxyGeneratorFactory
    {
        private static IProxyGeneratorFactory factory;

        /// <summary>
        /// Gets the instance offactory.
        /// </summary>
        /// <value>
        /// The instance of factory.
        /// </value>
        public static IProxyGeneratorFactory Factory
        {
            get => factory;
        }

        static ProxyGeneratorFactory()
        {
            factory = new DefaultProxyGeneratorFactory();
        }

        /// <summary>
        /// Overrides the factory instance.
        /// </summary>
        /// <param name="newFactory">The new factory instance.</param>
        /// <exception cref="ArgumentNullException">newFactory</exception>
        public static void OverrideFactory(IProxyGeneratorFactory newFactory)
        {
            if (newFactory == null)
            {
                throw new ArgumentNullException(nameof(newFactory));
            }

            factory = newFactory;
        }
    }
}
