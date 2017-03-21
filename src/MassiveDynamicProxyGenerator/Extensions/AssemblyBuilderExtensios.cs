using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator
{
    internal static class AssemblyBuilderExtensios
    {
#if NETSTANDARD1_6
        public static void Save(this AssemblyBuilder typeBuilder, string fileName)
        {
            throw new NotImplementedException(".Net Core does not support save assembly.");
        }
#endif
    }
}
