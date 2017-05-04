using System;
using System.Collections.Generic;
using System.Text;
using MassiveDynamicProxyGenerator;
using MassiveDynamicProxyGenerator.SimpleInjector;
using System.Reflection;
using SimpleInjector;

namespace MassiveDynamicProxyGenerator.SimpleInjector
{
    public static partial class ContainerExtensions
    {
        /// <summary>
        /// Registers the mock for service <typeparamref name="TService"/> to container.
        /// (With defult lifestyle).
        /// </summary>
        /// <typeparam name="TService">The type of the service. Must by public interface.</typeparam>
        /// <param name="container">The container.</param>
        /// <exception cref="System.ArgumentException">Generic type <typeparamref name="TService"/> must by public interface.</exception>
        public static void RegisterMock<TService>(this Container container)
           where TService : class
        {
            if (!TypeHelper.IsPublicInterface(typeof(TService)))
            {
                throw new ArgumentException($"Generic type {typeof(TService).FullName} must by public interface.");
            }

            ProxygGenerator generator = new ProxygGenerator();

#if NET40
            container.Register(typeof(TService), () => generator.GenerateProxy<TService>(new NullInterceptor()));
#else
            container.Register(typeof(TService), () => generator.GenerateProxy<TService>(new NullAsyncInterceptor()));
#endif
        }


        /// <summary>
        /// Registers the mock for service <paramref name="mockType"/> (is posssible use open generic) to container.
        /// (With defult lifestyle).
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="mockType">Type of the mock. Must by public interface.</param>
        /// <exception cref="System.ArgumentNullException">mockType</exception>
        /// <exception cref="System.ArgumentException">The type parameter <paramref name="mockType"/> must by public interface.</exception>
        public static void RegisterMock(this Container container, Type mockType)
        {
            if (mockType == null)
            {
                throw new ArgumentNullException(nameof(mockType));
            }

            if (!TypeHelper.IsPublicInterface(mockType))
            {
                throw new ArgumentException($"The type parameter {nameof(mockType)} {mockType.FullName} must by public interface.");
            }

            ProxygGenerator generator = new ProxygGenerator();

            if (TypeHelper.IsOpenGeneric(mockType))
            {
                container.ResolveUnregisteredType += (sender, arguments) =>
                {
                    if (!arguments.Handled && TypeHelper.IsGenericConstructedOf(mockType, arguments.UnregisteredServiceType))
                    {
#if NET40
                        arguments.Register(() => generator.GenerateProxy(arguments.UnregisteredServiceType, new NullInterceptor()));
#else
                        arguments.Register(() => generator.GenerateProxy(arguments.UnregisteredServiceType, new NullAsyncInterceptor()));
#endif
                    }
                };
            }
            else
            {

#if NET40
                container.Register(mockType, () => generator.GenerateProxy(mockType, new NullInterceptor()));
#else
                container.Register(mockType, () => generator.GenerateProxy(mockType, new NullAsyncInterceptor()));
#endif
            }
        }
    }
}
