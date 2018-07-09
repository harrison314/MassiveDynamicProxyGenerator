using System;
using System.Collections.Generic;
using System.Text;

namespace MassiveDynamicProxyGenerator.Microsoft.DependencyInjection
{
    /// <summary>
    /// Original service generic implementation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="MassiveDynamicProxyGenerator.Microsoft.DependencyInjection.IOriginalService{T}" />
    public class OriginalService<T> : IOriginalService<T> where T : class
    {
        /// <summary>
        /// Gets the service instance.
        /// </summary>
        /// <value>
        /// The service instance.
        /// </value>
        public T ServiceInstance
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OriginalService{T}"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <exception cref="ArgumentNullException">serviceProvider</exception>
        public OriginalService(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

            this.ServiceInstance = serviceProvider.GetService(typeof(T)) as T;
        }
    }
}
