using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.TypedInstanceProxy
{
    public class SimpleInstanceProvider<T> : IInstanceProvicer
        where T : class
    {
        private readonly T instance;

        public SimpleInstanceProvider(T instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            this.instance = instance;
        }

        public object GetInstance()
        {
            return this.instance;
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
