using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.SimpleInjector.Tests.Services
{
    public class GenericInstanceProducer<T> : IInstanceProvicer
    {
        public GenericInstanceProducer()
        {

        }

        public void Dispose()
        {
        }

        public object GetInstance()
        {
            return new GenericService<T>();
        }
    }
}
