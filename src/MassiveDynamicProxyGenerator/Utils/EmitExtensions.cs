using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.Utils
{
    /// <summary>
    /// Emit extensions.
    /// </summary>
    internal static class EmitExtensions
    {
        /// <summary>
        /// Emits the cast to reference.
        /// </summary>
        /// <param name="il">The IL generator.</param>
        /// <param name="type">The type.</param>
        /// <exception cref="ArgumentNullException">
        /// il
        /// or
        /// type
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EmitCastToReference(this ILGenerator il, Type type)
        {
#if DEBUG
            if (il == null)
            {
                throw new ArgumentNullException(nameof(il));
            }

            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
#endif

            if (type.GetTypeInfo().IsValueType)
            {
                il.Emit(OpCodes.Unbox_Any, type);
            }
            else
            {
                il.Emit(OpCodes.Castclass, type);
            }
        }

        /// <summary>
        /// Emits the box if needed.
        /// </summary>
        /// <param name="il">The IL genarator.</param>
        /// <param name="type">The type.</param>
        /// <exception cref="ArgumentNullException">
        /// il
        /// or
        /// type
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EmitBoxIfNeeded(this ILGenerator il, Type type)
        {
#if DEBUG
            if (il == null)
            {
                throw new ArgumentNullException(nameof(il));
            }

            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
#endif

            if (type.GetTypeInfo().IsValueType)
            {
                il.Emit(OpCodes.Box, type);
            }
        }

        /// <summary>
        /// Emits the fast <see cref="int"/> value.
        /// </summary>
        /// <param name="il">The IL generator.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="ArgumentNullException">il</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EmitFastInt(this ILGenerator il, int value)
        {
#if DEBUG
            if (il == null)
            {
                throw new ArgumentNullException(nameof(il));
            }
#endif

            switch (value)
            {
                case -1:
                    il.Emit(OpCodes.Ldc_I4_M1);
                    return;
                case 0:
                    il.Emit(OpCodes.Ldc_I4_0);
                    return;
                case 1:
                    il.Emit(OpCodes.Ldc_I4_1);
                    return;
                case 2:
                    il.Emit(OpCodes.Ldc_I4_2);
                    return;
                case 3:
                    il.Emit(OpCodes.Ldc_I4_3);
                    return;
                case 4:
                    il.Emit(OpCodes.Ldc_I4_4);
                    return;
                case 5:
                    il.Emit(OpCodes.Ldc_I4_5);
                    return;
                case 6:
                    il.Emit(OpCodes.Ldc_I4_6);
                    return;
                case 7:
                    il.Emit(OpCodes.Ldc_I4_7);
                    return;
                case 8:
                    il.Emit(OpCodes.Ldc_I4_8);
                    return;
            }

            if (value > -129 && value < 128)
            {
                il.Emit(OpCodes.Ldc_I4_S, (sbyte)value);
            }
            else
            {
                il.Emit(OpCodes.Ldc_I4, value);
            }
        }
    }
}
