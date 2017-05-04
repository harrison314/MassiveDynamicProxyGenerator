using System;
using System.Collections.Generic;
using System.Text;
using MassiveDynamicProxyGenerator;
using MassiveDynamicProxyGenerator.SimpleInjector;
using System.Linq.Expressions;
using System.Reflection;
using SimpleInjector;
using MassiveDynamicProxyGenerator.SimpleInjector.Interception;

namespace MassiveDynamicProxyGenerator.SimpleInjector
{
    public static partial class ContainerExtensions
    {

        /// <summary>
        /// Using interceptor to decorate all types that match the predicate.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="interceptorType">Type of the interceptor. Registred in container or with default constructor.</param>
        /// <param name="predicate">The predicate.</param>
        /// <exception cref="System.ArgumentNullException">interceptorType</exception>
        /// <exception cref="System.ArgumentException">interceptorType - interceptorType</exception>
        public static void RegisterInterceptedDecorator(this Container container, Type interceptorType, Predicate<Type> predicate)
        {
            if (interceptorType == null)
            {
                throw new ArgumentNullException(nameof(interceptorType));
            }

            if (!typeof(ICallableInterceptor).GetTypeInfo().IsAssignableFrom(interceptorType))
            {
                throw new ArgumentException($"Type parameter {nameof(interceptorType)} is not asssignable to {typeof(ICallableInterceptor).FullName}.", nameof(interceptorType));
            }

            ProxygGenerator generator = new ProxygGenerator();
            InterceptionBuilder builder = new InterceptionBuilder(predicate, generator, interceptorType);
            container.ExpressionBuilt += builder.ReguildExpresion;
        }
    }
}
