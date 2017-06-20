using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using MassiveDynamicProxyGenerator;
using MassiveDynamicProxyGenerator.SimpleInjector;
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
        /// <exception cref="System.ArgumentNullException">
        /// interceptorType
        /// or
        /// predicate
        /// </exception>
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

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            IProxygGenerator generator = ProxyGeneratorFactory.Factory.GetInstance();
            InterceptionBuilder builder = new TypeInterceptionBuilder(predicate, generator, interceptorType);
            container.ExpressionBuilt += builder.ReguildExpresion;
        }

        /// <summary>
        /// Using interceptor to decorate all types that match the predicate.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="interceptor">The interceptor for decoration.</param>
        /// <param name="predicate">The predicate.</param>
        /// <exception cref="System.ArgumentNullException">
        /// interceptor
        /// or
        /// predicate
        /// </exception>
        public static void RegisterInterceptedDecorator(this Container container, ICallableInterceptor interceptor, Predicate<Type> predicate)
        {
            if (interceptor == null)
            {
                throw new ArgumentNullException(nameof(interceptor));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            IProxygGenerator generator = ProxyGeneratorFactory.Factory.GetInstance();
            InterceptionBuilder builder = new InstanceInterceptionBuilder(predicate, generator, interceptor);
            container.ExpressionBuilt += builder.ReguildExpresion;
        }

        /// <summary>
        /// Using interceptor to decorate all types that match the predicate.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="interceptorFactory">The interceptor factory.</param>
        /// <param name="predicate">The predicate.</param>
        /// <exception cref="System.ArgumentNullException">
        /// interceptorFactory
        /// or
        /// predicate
        /// </exception>
        public static void RegisterInterceptedDecorator(this Container container, Func<ICallableInterceptor> interceptorFactory, Predicate<Type> predicate)
        {
            if (interceptorFactory == null)
            {
                throw new ArgumentNullException(nameof(interceptorFactory));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            IProxygGenerator generator = ProxyGeneratorFactory.Factory.GetInstance();
            InterceptionBuilder builder = new FuncInterceptionBuilder(generator, predicate, interceptorFactory);
            container.ExpressionBuilt += builder.ReguildExpresion;
        }
    }
}
