using System;
using System.Collections.Generic;
using System.Text;

namespace MassiveDynamicProxyGenerator.Microsoft.DependencyInjection
{
    /// <summary>
    /// Interface provide original non-wrapped or intercepted service.
    /// </summary>
    /// <typeparam name="T">Type of service.</typeparam>
    public interface IOriginalService<T> where T : class
    {
        /// <summary>
        /// Gets the service instance.
        /// </summary>
        /// <value>
        /// The service instance.
        /// </value>
        T ServiceInstance
        {
            get;
        }
    }
}
