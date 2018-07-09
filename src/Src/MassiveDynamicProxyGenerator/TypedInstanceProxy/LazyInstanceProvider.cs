using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.TypedInstanceProxy
{
    /// <summary>
    /// Lazy instance provider.
    /// </summary>
    /// <typeparam name="T">Type of instance.</typeparam>
    /// <seealso cref="MassiveDynamicProxyGenerator.IInstanceProvicer" />
    public class LazyInstanceProvider<T> : IInstanceProvicer
        where T : class
    {
        private readonly object syncRoot;
        private readonly Func<T> factory;
        private T instance;
        private bool disposedValue = false;
        private bool enableDisposing;

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyInstanceProvider{T}"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="enableDisposing">if set to <c>true</c> [enable disposing].</param>
        /// <exception cref="ArgumentNullException">factory</exception>
        public LazyInstanceProvider(Func<T> factory, bool enableDisposing)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            this.factory = factory;
            this.instance = null;
            this.syncRoot = new object();
            this.enableDisposing = enableDisposing;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="LazyInstanceProvider{T}"/> class.
        /// </summary>
        ~LazyInstanceProvider()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Gets the instance lazy of real class implementation.
        /// </summary>
        /// <returns>
        /// Instance of real class implementation.
        /// </returns>
        /// <exception cref="ObjectDisposedException">Throw if is instance is disposed.</exception>
        public object GetInstance()
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(typeof(T).Name);
            }

            if (object.ReferenceEquals(this.instance, null))
            {
                lock (this.syncRoot)
                {
                    if (object.ReferenceEquals(this.instance, null))
                    {
                        this.instance = this.factory.Invoke();
                    }
                }
            }

            return this.instance;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    if (this.enableDisposing)
                    {
                        IDisposable disposable = this.instance as IDisposable;
                        if (disposable != null)
                        {
                            disposable.Dispose();
                        }
                    }
                }

                this.disposedValue = true;
            }
        }
    }
}
