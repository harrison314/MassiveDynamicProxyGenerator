using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.TypedInstanceProxy
{
    public class LazyInstanceProvider<T> : IInstanceProvicer
        where T : class
    {
        private readonly object syncRoot;
        private readonly Func<T> factory;
        private T instance;
        private bool disposedValue = false;
        private bool enableDisposing;

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

        ~LazyInstanceProvider()
        {
            this.Dispose(false);
        }

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

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

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
