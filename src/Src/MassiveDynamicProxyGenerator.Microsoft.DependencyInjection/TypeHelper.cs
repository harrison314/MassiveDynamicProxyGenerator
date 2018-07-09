using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace MassiveDynamicProxyGenerator.Microsoft.DependencyInjection
{
    internal class TypeHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOpenGeneric(Type type)
        {
            return type.GetTypeInfo().IsGenericTypeDefinition;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsGenericConstructedOf(Type genericDefinitionType, Type constructedType)
        {
            if (!genericDefinitionType.GetTypeInfo().IsGenericType || !constructedType.GetTypeInfo().IsGenericType)
            {
                return false;
            }

            return constructedType.GetGenericTypeDefinition() == genericDefinitionType;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPublicInterface(Type type)
        {
            return type.GetTypeInfo().IsPublic && type.GetTypeInfo().IsInterface;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type[] GetConstructorRequiredTypes(Type type)
        {
            ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
            if (constructors.Length != 1)
            {
                throw new ArgumentException($"Type '{type.AssemblyQualifiedName}' must have only one public constructor.");
            }

            ParameterInfo[] parameters = constructors[0].GetParameters();
            Type[] requiredTypes = new Type[parameters.Length];
            for (int i = 0; i < requiredTypes.Length; i++)
            {
                requiredTypes[i] = parameters[i].ParameterType;
            }

            return requiredTypes;
        }
    }
}
