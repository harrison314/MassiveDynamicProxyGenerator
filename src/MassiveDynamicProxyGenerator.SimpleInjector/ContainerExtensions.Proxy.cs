using System;
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
        /// Registers the proxy crated using interceptor. Interceptor type must by registered in IoC container or must by default constructor.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="serviceType">Type of the service. Must by interface.</param>
        /// <param name="interceptorType">Type of the interceptor. Must by <see cref="IInterceptor"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// serviceType
        /// or
        /// interceptorType
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// interceptorType - interceptorType
        /// or
        /// serviceType
        /// </exception>
        public static void RegisterProxy(this Container container, Type serviceType, Type interceptorType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (interceptorType == null)
            {
                throw new ArgumentNullException(nameof(interceptorType));
            }

            if (!typeof(IInterceptor).GetTypeInfo().IsAssignableFrom(interceptorType))
            {
                throw new ArgumentException($"Type parameter {nameof(interceptorType)} is not assignable to {typeof(IInterceptor).FullName}.", nameof(interceptorType));
            }

            if (!TypeHelper.IsPublicInterface(serviceType))
            {
                throw new ArgumentException($"The type parameter {nameof(serviceType)} {serviceType.FullName} must by public interface.");
            }

            IProxyGenerator generator = ProxyGeneratorFactory.Factory.GetInstance();

            if (TypeHelper.IsOpenGeneric(serviceType))
            {
                InterceptedProxyBuilder builder = new OpenTypeInterceptedProxyBuilder(generator, serviceType, interceptorType);
                container.ResolveUnregisteredType += builder.ResolveUnregisteredType;
            }
            else
            {
                Registration registration = new Registrations.ProxyWithTypeInterceptorRegistration(container.Options.DefaultLifestyle,
                    container,
                    serviceType,
                    interceptorType,
                    generator);

                container.AddRegistration(serviceType, registration);
            }
        }

        /// <summary>
        /// Registers the proxy crated using specific interceptor.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="serviceType">Type of the service. Must by interface.</param>
        /// <param name="interceptor">The interceptor instance.</param>
        /// <exception cref="System.ArgumentNullException">
        /// serviceType
        /// or
        /// interceptor
        /// </exception>
        /// <exception cref="System.ArgumentException">serviceType</exception>
        public static void RegisterProxy(this Container container, Type serviceType, IInterceptor interceptor)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (interceptor == null)
            {
                throw new ArgumentNullException(nameof(interceptor));
            }

            if (!TypeHelper.IsPublicInterface(serviceType))
            {
                throw new ArgumentException($"The type parameter {nameof(serviceType)} {serviceType.FullName} must by public interface.");
            }

            IProxyGenerator generator = ProxyGeneratorFactory.Factory.GetInstance();

            if (TypeHelper.IsOpenGeneric(serviceType))
            {
                InterceptedProxyBuilder builder = new OpenInstanceInterceptedProxyBuilder(generator, serviceType, interceptor);
                container.ResolveUnregisteredType += builder.ResolveUnregisteredType;
            }
            else
            {
                Registration registration = new Registrations.ProxyWithInstanceInterceptorRegistration(container.Options.DefaultLifestyle,
                    container,
                    serviceType,
                    interceptor,
                    generator);

                container.AddRegistration(serviceType, registration);
            }
        }

        /// <summary>
        /// Registers the proxy crated using specific created by <paramref name="interceptorFactory"/>.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="serviceType">Type of the service. Must by interface.</param>
        /// <param name="interceptorFactory">The interceptor factory.</param>
        /// <exception cref="System.ArgumentNullException">
        /// serviceType
        /// or
        /// interceptorFactory
        /// </exception>
        /// <exception cref="System.ArgumentException">serviceType</exception>
        public static void RegisterProxy(this Container container, Type serviceType, Func<IInterceptor> interceptorFactory)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (interceptorFactory == null)
            {
                throw new ArgumentNullException(nameof(interceptorFactory));
            }

            if (!TypeHelper.IsPublicInterface(serviceType))
            {
                throw new ArgumentException($"The type parameter {nameof(serviceType)} {serviceType.FullName} must by public interface.");
            }

            IProxyGenerator generator = ProxyGeneratorFactory.Factory.GetInstance();

            if (TypeHelper.IsOpenGeneric(serviceType))
            {
                InterceptedProxyBuilder builder = new OpenFuncInterceptedProxyBuilder(generator, serviceType, interceptorFactory);
                container.ResolveUnregisteredType += builder.ResolveUnregisteredType;
            }
            else
            {
                Registration registration = new Registrations.ProxyWithFactoryInterceptorRegistration(container.Options.DefaultLifestyle,
                    container,
                    serviceType,
                    interceptorFactory,
                    generator);

                container.AddRegistration(serviceType, registration);
            }
        }
    }
}
