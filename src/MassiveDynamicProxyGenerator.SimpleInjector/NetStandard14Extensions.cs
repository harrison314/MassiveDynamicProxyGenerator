using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MassiveDynamicProxyGenerator.SimpleInjector
{
#if NETSTANDARD1_4
    internal static class NetStandard14Extensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type[] GetInterfaces(this TypeInfo typeInfo)
        {
            return typeInfo.AsType().GetInterfaces();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ConstructorInfo GetConstructor(this TypeInfo typeInfo, Type[] types)
        {
            return typeInfo.AsType().GetConstructor(types);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ConstructorInfo[] GetConstructors(this TypeInfo typeInfo)
        {
            return typeInfo.AsType().GetConstructors();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FieldInfo GetField(this TypeInfo typeInfo, string name, BindingFlags bindingAttr)
        {
            return typeInfo.AsType().GetField(name, bindingAttr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MethodInfo GetMethod(this TypeInfo typeInfo, string name, Type[] types)
        {
            return typeInfo.AsType().GetMethod(name, types);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MethodInfo GetMethod(this TypeInfo typeInfo, string name)
        {
            return typeInfo.AsType().GetMethod(name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MethodInfo GetMethod(this TypeInfo typeInfo, string name, BindingFlags bindingAttr)
        {
            return typeInfo.AsType().GetMethod(name, bindingAttr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MethodInfo[] GetMethods(this TypeInfo typeInfo, BindingFlags bindingAttr)
        {
            return typeInfo.AsType().GetMethods(bindingAttr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PropertyInfo GetProperty(this TypeInfo typeInfo, string name, BindingFlags bindngAttr)
        {
            return typeInfo.AsType().GetProperty(name, bindngAttr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PropertyInfo GetProperty(this TypeInfo typeInfo, string name)
        {
            return typeInfo.AsType().GetProperty(name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PropertyInfo[] GetProperties(this TypeInfo typeInfo, BindingFlags bindingAttr)
        {
            return typeInfo.AsType().GetProperties(bindingAttr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type[] GetGenericArguments(this TypeInfo info)
        {
            return info.AsType().GetGenericArguments();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MethodInfo GetSetMethod(this PropertyInfo property)
        {
            return property.SetMethod;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MethodInfo GetGetMethod(this PropertyInfo property)
        {
            return property.GetMethod;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAssignableFrom(this TypeInfo typeInfo, Type type)
        {
            return typeInfo.IsAssignableFrom(type.GetTypeInfo());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type CreateType(this System.Reflection.Emit.TypeBuilder typeBuilder)
        {
            return typeBuilder.CreateTypeInfo().AsType();
        }
    }
#endif
}
