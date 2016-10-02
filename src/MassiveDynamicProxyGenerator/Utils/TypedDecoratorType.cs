using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.Utils
{
    internal enum TypedDecoratorType : int
    {
        TypedProxy = 1,
        TypedProxyWithParameters = 2,
        TypedInstancedProxy = 3,
        TypedDecorator = 4
    }
}
