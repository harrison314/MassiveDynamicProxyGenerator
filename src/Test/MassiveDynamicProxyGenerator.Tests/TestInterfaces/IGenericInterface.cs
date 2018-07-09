using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.Tests.TestInterfaces
{
    public interface IGenericInterface<T>
    {
        T Value
        {
            get;
            set;
        }

        void PushNew(string name, T value);

        T Get(string name);

        T[] GetAll();
    }
}
