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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type CreateType(this TypeBuilder typeBuilder)
        {
            TypeInfo typeInfo = typeBuilder.CreateTypeInfo();
            return typeBuilder.AsType();
        }
    }
}
