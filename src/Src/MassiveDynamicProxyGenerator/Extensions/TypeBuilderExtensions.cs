using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator
{
    internal static class TypeBuilderExtensions
    {
#if NETSTANDARD1_6 || NETSTANDARD2_0
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type CreateType(this TypeBuilder typeBuilder)
        {
            TypeInfo typeInfo = typeBuilder.CreateTypeInfo();
            return typeBuilder.AsType();
        }
#endif
    }
}
