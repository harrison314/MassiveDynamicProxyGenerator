using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.Utils
{
    internal interface ITypeRquest
    {
        TypedDecoratorType DecoratorType
        {
            get;
        }

        Type[] InterfaceTypes
        {
            get;
        }
    }
}
