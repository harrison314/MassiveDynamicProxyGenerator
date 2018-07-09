using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator
{
    internal static class AssemblyBuilderExtensios
    {
#if NETSTANDARD1_6 || NETSTANDARD2_0
        public static void Save(this AssemblyBuilder typeBuilder, string fileName)
        {
            throw new NotImplementedException(".Net Standard does not support save assembly.");
        }
#endif
    }
}
