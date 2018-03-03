using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MassiveDynamicProxyGenerator.Microsoft.DependencyInjection
{
    internal class TypeHelper
    {
        public static bool IsOpenGeneric(Type type)
        {
            return type.GetTypeInfo().IsGenericTypeDefinition;
        }

        public static bool IsGenericConstructedOf(Type genericDefinitionType, Type constructedType)
        {
            if (!genericDefinitionType.GetTypeInfo().IsGenericType || !constructedType.GetTypeInfo().IsGenericType)
            {
                return false;
            }

            return constructedType.GetGenericTypeDefinition() == genericDefinitionType;
        }

        public static bool IsPublicInterface(Type type)
        {
            return type.GetTypeInfo().IsPublic && type.GetTypeInfo().IsInterface;
        }

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
