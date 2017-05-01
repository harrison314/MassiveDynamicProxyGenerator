﻿using System;
using System.Reflection;

namespace MassiveDynamicProxyGenerator.SimpleInjector
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
            return type.GetTypeInfo().IsPublic || type.GetTypeInfo().IsInterface;
        }
    }
}