using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.TypedInstanceProxy
{
    /// <summary>
    /// Class representes simple instance povider, allwais returns one instance.
    /// </summary>
    /// <typeparam name="T">Type of instance.</typeparam>
    /// <seealso cref="MassiveDynamicProxyGenerator.IInstanceProvicer" />
    public class SimpleInstanceProvider<T> : IInstanceProvicer
        where T : class
    {
        private readonly T instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleInstanceProvider{T}"/> class.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <exception cref="ArgumentNullException">instance</exception>
        public SimpleInstanceProvider(T instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            this.instance = instance;
        }

        /// <summary>
        /// Gets the instance of real class implementation.
        /// </summary>
        /// <returns>
        /// Instance of real class implementation.
        /// </returns>
        public object GetInstance()
        {
            return this.instance;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
