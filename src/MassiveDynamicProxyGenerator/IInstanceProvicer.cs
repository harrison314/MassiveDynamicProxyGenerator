using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator
{
    /// <summary>
    /// Interface representes instance provider.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IInstanceProvicer : IDisposable
    {
        /// <summary>
        /// Gets the instance of real class implementation.
        /// </summary>
        /// <returns>Instance of real class implementation.</returns>
        object GetInstance();
    }
}
