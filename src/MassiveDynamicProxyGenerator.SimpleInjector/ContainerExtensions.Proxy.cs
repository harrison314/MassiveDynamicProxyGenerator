﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using SimpleInjector;
using MassiveDynamicProxyGenerator.SimpleInjector.Interception;

namespace MassiveDynamicProxyGenerator.SimpleInjector
{
    public static partial class ContainerExtensions
    {
        /// <summary>
        /// Registers the proxy crated using inteceptor. Interceptor type must by registred in IoC container or must by default constructor.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="serviseType">Type of the servise. Must by interface.</param>
        /// <param name="interceptorType">Type of the interceptor. Must by <see cref="IInterceptor"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// serviseType
        /// or
        /// interceptorType
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// interceptorType - interceptorType
        /// or
        /// serviseType
        /// </exception>
        public static void RegisterProxy(this Container container, Type serviseType, Type interceptorType)
        {
            if (serviseType == null)
            {
                throw new ArgumentNullException(nameof(serviseType));
            }

            if (interceptorType == null)
            {
                throw new ArgumentNullException(nameof(interceptorType));
            }

            if (!typeof(IInterceptor).GetTypeInfo().IsAssignableFrom(interceptorType))
            {
                throw new ArgumentException($"Type parameter {nameof(interceptorType)} is not asssignable to {typeof(IInterceptor).FullName}.", nameof(interceptorType));
            }

            if (!TypeHelper.IsPublicInterface(serviseType))
            {
                throw new ArgumentException($"The type parameter {nameof(serviseType)} {serviseType.FullName} must by public interface.");
            }

            ProxygGenerator generator = new ProxygGenerator();

            if (TypeHelper.IsOpenGeneric(serviseType))
            {
                InterceptedProxyBulder builder = new OpenTypeInterceptedProxyBuilder(generator, serviseType, interceptorType);
                container.ResolveUnregisteredType += builder.ResolveUnregisteredType;
            }
            else
            {
                InterceptedProxyBulder builder = new TypeInterceptedProxyBuilder(generator, serviseType, interceptorType);
                container.ResolveUnregisteredType += builder.ResolveUnregisteredType;
            }
        }

        /// <summary>
        /// Registers the proxy crated using specific inteceptor.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="serviseType">Type of the servise. Must by interface.</param>
        /// <param name="interceptor">The interceptor instance.</param>
        /// <exception cref="System.ArgumentNullException">
        /// serviseType
        /// or
        /// interceptor
        /// </exception>
        /// <exception cref="System.ArgumentException">serviseType</exception>
        public static void RegisterProxy(this Container container, Type serviseType, IInterceptor interceptor)
        {
            if (serviseType == null)
            {
                throw new ArgumentNullException(nameof(serviseType));
            }

            if (interceptor == null)
            {
                throw new ArgumentNullException(nameof(interceptor));
            }

            if (!TypeHelper.IsPublicInterface(serviseType))
            {
                throw new ArgumentException($"The type parameter {nameof(serviseType)} {serviseType.FullName} must by public interface.");
            }

            ProxygGenerator generator = new ProxygGenerator();

            if (TypeHelper.IsOpenGeneric(serviseType))
            {
                InterceptedProxyBulder builder = new OpenInstanceInterceptedProxyBuilder(generator, serviseType, interceptor);
                container.ResolveUnregisteredType += builder.ResolveUnregisteredType;
            }
            else
            {
                InterceptedProxyBulder builder = new InstanceInterceptedProxyBuilder(generator, serviseType, interceptor);
                container.ResolveUnregisteredType += builder.ResolveUnregisteredType;
            }
        }
    }
}
