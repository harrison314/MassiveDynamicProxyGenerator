using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.Tests.TestInterfaces
{
    public interface IInterfaceWithDefaultMethod
    {
        int GetAize();

#if NETSTANDARD2_1 || NETCOREAPP
        int GetSquare()
        {
            int size = this.GetSquare();
            return size * size;
        }
#else
        int GetSquare();
#endif
    }
}
