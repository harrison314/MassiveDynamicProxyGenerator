using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace MassiveDynamicProxyGenerator.Microsoft.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the instance proxy for service <paramref name="serviceType"/> using <paramref name="instanceProvider"/>.
        /// </summary>
        /// <param name="serviceCollection">The <see cref="IServiceCollection"/> to add the service to.</param>
        /// <param name="serviceType">Type of the service to create instance provider. Must by public interface.</param>
        /// <param name="instanceProvider">The instance provider instance.</param>
        /// <returns>The <see cref="IServiceCollection"/> to add the service to.</returns>
        /// <exception cref="ArgumentNullException">
        /// serviceType
        /// or
        /// instanceProvicer
        /// </exception>
        /// <exception cref="ArgumentException">
        /// serviceType
        /// </exception>
        /// <seealso cref="IInstanceProvicer"/>
        public static IServiceCollection AddInstanceProxy(this IServiceCollection serviceCollection, Type serviceType, IInstanceProvicer instanceProvider)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (instanceProvider == null)
            {
                throw new ArgumentNullException(nameof(instanceProvider));
            }

            if (TypeHelper.IsOpenGeneric(serviceType))
            {
                throw new ArgumentException($"Service type {serviceType} can not open generic type.");
            }

            if (!TypeHelper.IsPublicInterface(serviceType))
            {
                throw new ArgumentException($"Parameter {nameof(serviceType)} of type '{serviceType.AssemblyQualifiedName}' is not public interface.");
            }

            return serviceCollection.AddSingleton(serviceType, sp => MassiveDynamicProxyGeneratorDiSettings.ProxyGeneratorProvider.GetProxyGenerator(sp).GenerateInstanceProxy(serviceType, instanceProvider));
        }

        /// <summary>
        /// Adds the instance proxy for service <typeparamref name="TService"/> using <paramref name="instanceProvider"/>.
        /// </summary>
        /// <typeparam name="TService">Type of the service to create instance provider. Must by public interface.</typeparam>
        /// <param name="serviceCollection">The <see cref="IServiceCollection"/> to add the service to.</param>
        /// <param name="instanceProvider">The instance provider instance.</param>
        /// <returns>The <see cref="IServiceCollection"/> to add the service to.</returns>
        /// <exception cref="ArgumentNullException">instanceProvicer</exception>
        /// <exception cref="ArgumentException"></exception>
        /// <seealso cref="IInstanceProvicer"/>
        public static IServiceCollection AddInstanceProxy<TService>(this IServiceCollection serviceCollection, IInstanceProvicer instanceProvider)
            where TService : class
        {
            return serviceCollection.AddInstanceProxy(typeof(TService), instanceProvider);
        }

        /// <summary>
        /// Adds the instance proxy for service <paramref name="serviceType" /> using <paramref name="instanceProvider" />.
        /// </summary>
        /// <param name="serviceCollection">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceType">Type of the service to create instance provider. Must by public interface.</param>
        /// <param name="instanceProvider">The instance provider instance.</param>
        /// <param name="proxyLifetime">The proxy lifetime.</param>
        /// <returns>
        /// The <see cref="IServiceCollection" /> to add the service to.
        /// </returns>
        /// <exception cref="ArgumentNullException">serviceType
        /// or
        /// instanceProvicer</exception>
        /// <exception cref="ArgumentException">serviceType</exception>
        /// <seealso cref="IInstanceProvicer" />
        public static IServiceCollection AddInstanceProxy(this IServiceCollection serviceCollection, Type serviceType, IInstanceProvicer instanceProvider, ServiceLifetime proxyLifetime)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (instanceProvider == null)
            {
                throw new ArgumentNullException(nameof(instanceProvider));
            }

            if (TypeHelper.IsOpenGeneric(serviceType))
            {
                throw new ArgumentException($"Service type {serviceType} can not open generic type.");
            }

            if (!TypeHelper.IsPublicInterface(serviceType))
            {
                throw new ArgumentException($"Parameter {nameof(serviceType)} of type '{serviceType.AssemblyQualifiedName}' is not public interface.");
            }

            ServiceDescriptor descriptor = new ServiceDescriptor(serviceType,
                sp => MassiveDynamicProxyGeneratorDiSettings.ProxyGeneratorProvider.GetProxyGenerator(sp).GenerateInstanceProxy(serviceType, instanceProvider),
                proxyLifetime);

            serviceCollection.Add(descriptor);
            return serviceCollection;
        }

        /// <summary>
        /// Adds the instance proxy for service <typeparamref name="TService" /> using <paramref name="instanceProvider" />.
        /// </summary>
        /// <typeparam name="TService">Type of the service to create instance provider. Must by public interface.</typeparam>
        /// <param name="serviceCollection">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="instanceProvider">The instance provider instance.</param>
        /// <param name="proxyLifetime">The proxy lifetime.</param>
        /// <returns>
        /// The <see cref="IServiceCollection" /> to add the service to.
        /// </returns>
        /// <exception cref="ArgumentException">serviceType</exception>
        /// <exception cref="ArgumentNullException">instanceProvicer</exception>
        /// <seealso cref="IInstanceProvicer" />
        public static IServiceCollection AddInstanceProxy<TService>(this IServiceCollection serviceCollection, IInstanceProvicer instanceProvider, ServiceLifetime proxyLifetime)
              where TService : class
        {
            return serviceCollection.AddInstanceProxy(typeof(TService), instanceProvider, proxyLifetime);
        }

        /// <summary>
        /// Adds the instance proxy for service <paramref name="serviceType" /> using <paramref name="instanceFactory"/> for create instances of type <paramref name="serviceType" />.
        /// </summary>
        /// <param name="serviceCollection">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceType">Type of the service to create instance provider. Must by public interface..</param>
        /// <param name="instanceFactory">The instance factory crates instances of <paramref name="serviceType" />.</param>
        /// <returns>The <see cref="IServiceCollection" /> to add the service to.</returns>
        /// <exception cref="ArgumentNullException">
        /// serviceType
        /// or
        /// instnaceFactory
        /// </exception>
        /// <exception cref="ArgumentException">
        /// serviceType
        /// </exception>
        /// <seealso cref="IInstanceProvicer" />
        public static IServiceCollection AddInstanceProxy(this IServiceCollection serviceCollection, Type serviceType, Func<object> instanceFactory)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (instanceFactory == null)
            {
                throw new ArgumentNullException(nameof(instanceFactory));
            }

            if (TypeHelper.IsOpenGeneric(serviceType))
            {
                throw new ArgumentException($"Service type {serviceType} can not open generic type.");
            }

            if (!TypeHelper.IsPublicInterface(serviceType))
            {
                throw new ArgumentException($"Parameter {nameof(serviceType)} of type '{serviceType.AssemblyQualifiedName}' is not public interface.");
            }

            return serviceCollection.AddSingleton(serviceType, sp => MassiveDynamicProxyGeneratorDiSettings.ProxyGeneratorProvider.GetProxyGenerator(sp).GenerateInstanceProxy(serviceType, new FuncInstanceProvider(instanceFactory)));
        }

        /// <summary>
        /// Adds the instance proxy for service <typeparamref name="TService"/> using <paramref name="instnaceFactory"/> for create instances of type <paramref name="serviceType" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="serviceCollection">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="instanceFactory">The instance factory crates instances of <typeparamref name="TService"/>.</param>
        /// <returns>The <see cref="IServiceCollection" /> to add the service to.</returns>
        /// <exception cref="AggregateException"></exception>
        /// <exception cref="ArgumentNullException">instaceFactory</exception>
        public static IServiceCollection AddInstanceProxy<TService>(this IServiceCollection serviceCollection, Func<TService> instanceFactory)
             where TService : class
        {
            return serviceCollection.AddInstanceProxy(typeof(TService), instanceFactory);
        }

        /// <summary>
        /// Adds the instance proxy for service <paramref name="serviceType" /> using <paramref name="instnaceFactory" /> for create instances of type <paramref name="serviceType" />.
        /// </summary>
        /// <param name="serviceCollection">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceType">Type of the service to create instance provider. Must by public interface..</param>
        /// <param name="instanceFactory">The instance factory.</param>
        /// <param name="proxyLifetime">The proxy lifetime.</param>
        /// <returns>
        /// The <see cref="IServiceCollection" /> to add the service to.
        /// </returns>
        /// <exception cref="ArgumentNullException">serviceType
        /// or
        /// instnaceFactory</exception>
        /// <exception cref="ArgumentException">serviceType</exception>
        /// <seealso cref="IInstanceProvicer" />
        public static IServiceCollection AddInstanceProxy(this IServiceCollection serviceCollection, Type serviceType, Func<object> instanceFactory, ServiceLifetime proxyLifetime)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (instanceFactory == null)
            {
                throw new ArgumentNullException(nameof(instanceFactory));
            }

            if (TypeHelper.IsOpenGeneric(serviceType))
            {
                throw new ArgumentException($"Service type {serviceType} can not open generic type.");
            }

            if (!TypeHelper.IsPublicInterface(serviceType))
            {
                throw new ArgumentException($"Parameter {nameof(serviceType)} of type '{serviceType.AssemblyQualifiedName}' is not public interface.");
            }


            ServiceDescriptor descriptor = new ServiceDescriptor(serviceType,
                sp => MassiveDynamicProxyGeneratorDiSettings.ProxyGeneratorProvider.GetProxyGenerator(sp).GenerateInstanceProxy(serviceType, new FuncInstanceProvider(instanceFactory)),
                proxyLifetime);

            serviceCollection.Add(descriptor);
            return serviceCollection;
        }

        /// <summary>
        /// Adds the instance proxy for service <typeparamref name="TService" /> using <paramref name="instnaceFactory" /> for create instances of type <paramref name="serviceType" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="serviceCollection">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="instanceProvider">The instance provider.</param>
        /// <param name="proxyLifetime">The proxy lifetime.</param>
        /// <returns>
        /// The <see cref="IServiceCollection" /> to add the service to.
        /// </returns>
        /// <exception cref="AggregateException"></exception>
        /// <exception cref="ArgumentNullException">instaceFactory</exception>
        public static IServiceCollection AddInstanceProxy<TService>(this IServiceCollection serviceCollection, Func<TService> instanceProvider, ServiceLifetime proxyLifetime)
            where TService : class
        {
            return serviceCollection.AddInstanceProxy(typeof(TService), instanceProvider, proxyLifetime);
        }
    }
}
