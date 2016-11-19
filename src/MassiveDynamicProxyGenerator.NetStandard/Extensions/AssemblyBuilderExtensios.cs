using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator
{
    internal static class AssemblyBuilderExtensios
    {
        public static void Save(this AssemblyBuilder typeBuilder, string fileName)
        {
            throw new NotImplementedException("Net core dosnt support save assembly.");
        }
    }
}
