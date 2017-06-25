using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MassiveDynamicProxyGenerator.SimpleInjector
{
#if !(NETSTANDARD1_6 || NETSTANDARD1_4)

    /// <summary>
    /// Extensions for adapt .Net Core API to full framework.
    /// </summary>
#if !NET40 && !NET45
    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    internal static class FullFrameworkExtensions
    {
        /// <summary>
        /// Gets same type for NET Standard compatibility.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The same type.</returns>
        public static Type GetTypeInfo(this Type type)
        {
            return type;
        }
    }

#endif
}