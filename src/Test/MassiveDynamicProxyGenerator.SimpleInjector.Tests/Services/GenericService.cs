using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.SimpleInjector.Tests.Services
{
    public class GenericService<T> : IGenericService<T>
    {
        private T last;

        public GenericService()
        {
            this.last = default(T);
        }

        public T GetLast()
        {
            return this.last;
        }

        public T Transform(T item)
        {
            this.last = item;
            return item;
        }
    }
}
