using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator
{
#if !(NETSTANDARD1_6 || NETSTANDARD1_4)

    /// <summary>
    /// Extensions for adapt .Net Core API to full framework.
    /// </summary>
    internal static class FullFrameworkExtensions
    {
        /// <summary>
        /// Gets same type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The same type.</returns>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Type GetTypeInfo(this Type type)
        {
            return type;
        }
    }

#endif
}
