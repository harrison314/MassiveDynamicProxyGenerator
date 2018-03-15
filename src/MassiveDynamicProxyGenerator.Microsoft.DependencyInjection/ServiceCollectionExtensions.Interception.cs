using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MassiveDynamicProxyGenerator.Microsoft.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add intercepted decorator to type <typeparamref name="TServise"/>.
        /// </summary>
        /// <typeparam name="TServise">Type of the decorated service. Must by public interface.</typeparam>
        /// <typeparam name="TInterceptor">Type of interceptor.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="interceptorParams">Additional interceptor parameters.</param>
        /// <returns>The <see cref="IServiceCollection" /> to add the service to.</returns>
        public static IServiceCollection AddInterceptedDecorator<TServise, TInterceptor>(this IServiceCollection services, params object[] interceptorParams)
           where TServise : class
           where TInterceptor : ICallableInterceptor
        {
            ProxygGenerator generator = new ProxygGenerator();
            List<ServiceDescriptor> descriptors = GetDescriptors(services, typeof(TServise));
            foreach (ServiceDescriptor descriptor in descriptors)
            {
                int index = services.IndexOf(descriptor);

                ServiceDescriptor decoratedDescriptor = ServiceDescriptor.Describe(descriptor.ServiceType,
                provider =>
                {
                    TServise instance = (TServise)GetInstanceFromDescriptor(provider, descriptor);
                    ICallableInterceptor interceptor = (ICallableInterceptor)ActivatorUtilities.CreateInstance(provider, typeof(TInterceptor), interceptorParams);
                    return generator.GenerateDecorator<TServise>(interceptor, instance);
                },
                descriptor.Lifetime);

                services.Insert(index, decoratedDescriptor);

                services.Remove(descriptor);
            }

            return services;
        }

        /// <summary>
        /// Add intercepted decorator to type <paramref name="serviceType"/> using interceptor type <paramref name="interceptorType"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceType">Type of the decorated service. Must by public interface.</param>
        /// <param name="interceptorType">Type of interceptor. Must implement <see cref="ICallableInterceptor"/>.</param>
        /// <param name="interceptorParams">Additional interceptor params.</param>
        /// <returns>The <see cref="IServiceCollection" /> to add the service to.</returns>
        /// <exception cref="ArgumentNullException">
        /// serviceType
        /// or
        /// interceptorType
        /// </exception>
        /// <exception cref="ArgumentException"></exception>
        public static IServiceCollection AddInterceptedDecorator(this IServiceCollection services, Type serviceType, Type interceptorType, params object[] interceptorParams)
        {
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (interceptorType == null) throw new ArgumentNullException(nameof(interceptorType));

            if (!TypeHelper.IsPublicInterface(serviceType))
            {
                throw new ArgumentException($"Patameter {nameof(serviceType)} of type '{serviceType.AssemblyQualifiedName}' is not public interface.");
            }

            if (!typeof(ICallableInterceptor).IsAssignableFrom(interceptorType))
            {
                throw new ArgumentException($"Patameter {nameof(interceptorType)} of type '{interceptorType.AssemblyQualifiedName}' must implement {typeof(ICallableInterceptor).AssemblyQualifiedName}.");
            }

            ProxygGenerator generator = new ProxygGenerator();
            List<ServiceDescriptor> descriptors = GetDescriptors(services, serviceType);
            foreach (ServiceDescriptor descriptor in descriptors)
            {
                int index = services.IndexOf(descriptor);

                ServiceDescriptor decoratedDescriptor = ServiceDescriptor.Describe(descriptor.ServiceType,
                provider =>
                {
                    object instance = GetInstanceFromDescriptor(provider, descriptor);
                    ICallableInterceptor interceptor = (ICallableInterceptor)ActivatorUtilities.CreateInstance(provider, interceptorType, interceptorParams);
                    return generator.GenerateDecorator(serviceType, interceptor, instance);
                },
                descriptor.Lifetime);

                services.Insert(index, decoratedDescriptor);

                services.Remove(descriptor);
            }

            return services;
        }

        /// <summary>
        /// Add intercepted decorator to type <paramref name="serviceType"/> using <paramref name="interceptor"/> instance.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceType">Type of the decorated service. Must by public interface.</param>
        /// <param name="interceptor">The interceptor instance.</param>
        /// <returns>The <see cref="IServiceCollection" /> to add the service to.</returns>
        /// <exception cref="ArgumentNullException">
        /// serviceType
        /// or
        /// interceptor
        /// </exception>
        /// <exception cref="ArgumentException"></exception>
        public static IServiceCollection AddInterceptedDecorator(this IServiceCollection services, Type serviceType, ICallableInterceptor interceptor)
        {
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (interceptor == null) throw new ArgumentNullException(nameof(interceptor));

            if (!TypeHelper.IsPublicInterface(serviceType))
            {
                throw new ArgumentException($"Patameter {nameof(serviceType)} of type '{serviceType.AssemblyQualifiedName}' is not public interface.");
            }

            ProxygGenerator generator = new ProxygGenerator();
            List<ServiceDescriptor> descriptors = GetDescriptors(services, serviceType);
            foreach (ServiceDescriptor descriptor in descriptors)
            {
                int index = services.IndexOf(descriptor);

                ServiceDescriptor decoratedDescriptor = ServiceDescriptor.Describe(descriptor.ServiceType,
                provider =>
                {
                    object instance = GetInstanceFromDescriptor(provider, descriptor);
                    return generator.GenerateDecorator(serviceType, interceptor, instance);
                },
                descriptor.Lifetime);

                services.Insert(index, decoratedDescriptor);

                services.Remove(descriptor);
            }

            return services;
        }

        /// <summary>
        /// Add intercepted decorator to type <typeparamref name="TSetvice"/> using <paramref name="interceptor"/> instance.
        /// </summary>
        /// <typeparam name="TSetvice">Type of the decorated service. Must by public interface.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="interceptor">The interceptor instance.</param>
        /// <returns>The <see cref="IServiceCollection" /> to add the service to.</returns>
        /// <exception cref="ArgumentNullException">interceptor</exception>
        public static IServiceCollection AddInterceptedDecorator<TSetvice>(this IServiceCollection services, ICallableInterceptor interceptor)
            where TSetvice : class
        {
            return services.AddInterceptedDecorator(typeof(TSetvice), interceptor);
        }

        /// <summary>
        /// Add intercepted decorator to type <paramref name="serviceType"/> using factory for interceptor <paramref name="interceptorFactory"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceType">Type of the decorated service. Must by public interface.</param>
        /// <param name="interceptorFactory">The interceptor factory.</param>
        /// <returns>The <see cref="IServiceCollection" /> to add the service to.</returns>
        /// <exception cref="ArgumentNullException">
        /// serviceType
        /// or
        /// interceptorFactory
        /// </exception>
        /// <exception cref="ArgumentException"></exception>
        public static IServiceCollection AddInterceptedDecorator(this IServiceCollection services, Type serviceType, Func<IServiceProvider, ICallableInterceptor> interceptorFactory)
        {
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (interceptorFactory == null) throw new ArgumentNullException(nameof(interceptorFactory));

            if (!TypeHelper.IsPublicInterface(serviceType))
            {
                throw new ArgumentException($"Patameter {nameof(serviceType)} of type '{serviceType.AssemblyQualifiedName}' is not public interface.");
            }

            ProxygGenerator generator = new ProxygGenerator();
            List<ServiceDescriptor> descriptors = GetDescriptors(services, serviceType);
            foreach (ServiceDescriptor descriptor in descriptors)
            {
                int index = services.IndexOf(descriptor);

                ServiceDescriptor decoratedDescriptor = ServiceDescriptor.Describe(descriptor.ServiceType,
                provider =>
                {
                    ICallableInterceptor interceptor = interceptorFactory.Invoke(provider);
                    object instance = GetInstanceFromDescriptor(provider, descriptor);
                    return generator.GenerateDecorator(serviceType, interceptor, instance);
                },
                descriptor.Lifetime);

                services.Insert(index, decoratedDescriptor);

                services.Remove(descriptor);
            }

            return services;
        }

        /// <summary>
        /// Add intercepted decorator to type <typeparamref name="TService"/> using factory for interceptor <paramref name="interceptorFactory"/>.
        /// </summary>
        /// <typeparam name="TService">Type of the decorated service. Must by public interface</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="interceptorFactory">The interceptor factory.</param>
        /// <returns>The <see cref="IServiceCollection" /> to add the service to.</returns>
        public static IServiceCollection AddInterceptedDecorator<TService>(this IServiceCollection services, Func<IServiceProvider, ICallableInterceptor> interceptorFactory)
        {
            return services.AddInterceptedDecorator(typeof(TService), interceptorFactory);
        }

        /// <summary>
        /// Add intercepted decorator to multiple services identified <paramref name="predicate"/> and intercepted <paramref name="interceptor"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="predicate">The predicated identified intercepted services.</param>
        /// <param name="interceptor">The interceptor instance.</param>
        /// <returns>The <see cref="IServiceCollection" /> to add the service to.</returns>
        /// <exception cref="ArgumentNullException">
        /// predicate
        /// or
        /// interceptor
        /// </exception>
        public static IServiceCollection AddInterceptedDecorator(this IServiceCollection services, Predicate<Type> predicate, ICallableInterceptor interceptor)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (interceptor == null) throw new ArgumentNullException(nameof(interceptor));

            ProxygGenerator generator = new ProxygGenerator();
            List<ServiceDescriptor> descriptors = GetDescriptors(services, predicate);
            foreach (ServiceDescriptor descriptor in descriptors)
            {
                if (descriptor.ServiceType.IsInterface)
                {
                    int index = services.IndexOf(descriptor);

                    ServiceDescriptor decoratedDescriptor = ServiceDescriptor.Describe(descriptor.ServiceType,
                    provider =>
                    {
                        object instance = GetInstanceFromDescriptor(provider, descriptor);
                        return generator.GenerateDecorator(descriptor.ServiceType, interceptor, instance);
                    },
                    descriptor.Lifetime);

                    services.Insert(index, decoratedDescriptor);

                    services.Remove(descriptor);
                }
            }

            return services;
        }

        /// <summary>
        /// Add intercepted decorator to multiple services identified <paramref name="predicate"/> and intercepted certed using <paramref name="interceptorFactory"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="predicate">The predicated identified intercepted services.</param>
        /// <param name="interceptorFactory">The interceptor factory.</param>
        /// <returns>The <see cref="IServiceCollection" /> to add the service to.</returns>
        /// <exception cref="ArgumentNullException">
        /// predicate
        /// or
        /// interceptorFactory
        /// </exception>
        public static IServiceCollection AddInterceptedDecorator(this IServiceCollection services, Predicate<Type> predicate, Func<IServiceProvider, ICallableInterceptor> interceptorFactory)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (interceptorFactory == null) throw new ArgumentNullException(nameof(interceptorFactory));

            ProxygGenerator generator = new ProxygGenerator();
            List<ServiceDescriptor> descriptors = GetDescriptors(services, predicate);
            foreach (ServiceDescriptor descriptor in descriptors)
            {
                if (descriptor.ServiceType.IsInterface)
                {
                    int index = services.IndexOf(descriptor);

                    ServiceDescriptor decoratedDescriptor = ServiceDescriptor.Describe(descriptor.ServiceType,
                    provider =>
                    {
                        object instance = GetInstanceFromDescriptor(provider, descriptor);
                        ICallableInterceptor interceptor = interceptorFactory.Invoke(provider);
                        return generator.GenerateDecorator(descriptor.ServiceType, interceptor, instance);
                    },
                    descriptor.Lifetime);

                    services.Insert(index, decoratedDescriptor);

                    services.Remove(descriptor);
                }
            }

            return services;
        }

        /// <summary>
        /// Add intercepted decorator to multiple services identified <paramref name="predicate"/> and intercepted using <paramref name="interceptorType"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="predicate">The predicated identified intercepted services.</param>
        /// <param name="interceptorType">Type of interceptor. Must by <see cref="ICallableInterceptor"/>.</param>
        /// <param name="interceptorParams">Additional interceptor parameters.</param>
        /// <returns>The <see cref="IServiceCollection" /> to add the service to.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException">
        /// predicate
        /// or
        /// interceptorType
        /// </exception>
        public static IServiceCollection AddInterceptedDecorator(this IServiceCollection services, Predicate<Type> predicate, Type interceptorType, params object[] interceptorParams)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (interceptorType == null) throw new ArgumentNullException(nameof(interceptorType));

            if (!typeof(ICallableInterceptor).IsAssignableFrom(interceptorType))
            {
                throw new ArgumentException($"Patameter {nameof(interceptorType)} of type '{interceptorType.AssemblyQualifiedName}' must implement {typeof(ICallableInterceptor).AssemblyQualifiedName}.");
            }

            ProxygGenerator generator = new ProxygGenerator();
            List<ServiceDescriptor> descriptors = GetDescriptors(services, predicate);
            CheckInterceptorConstructor(interceptorType, descriptors);

            foreach (ServiceDescriptor descriptor in descriptors)
            {
                if (descriptor.ServiceType.IsInterface)
                {
                    int index = services.IndexOf(descriptor);

                    ServiceDescriptor decoratedDescriptor = ServiceDescriptor.Describe(descriptor.ServiceType,
                    provider =>
                    {
                        object instance = GetInstanceFromDescriptor(provider, descriptor);
                        ICallableInterceptor interceptor = (ICallableInterceptor)ActivatorUtilities.CreateInstance(provider, interceptorType, interceptorParams);
                        return generator.GenerateDecorator(descriptor.ServiceType, interceptor, instance);
                    },
                    descriptor.Lifetime);

                    services.Insert(index, decoratedDescriptor);

                    services.Remove(descriptor);
                }
            }

            return services;
        }

        /// <summary>
        /// Add intercepted decorator to multiple services identified <paramref name="predicate"/> and intercepted using <typeparamref name="TInterceptor"/>.
        /// </summary>
        /// <typeparam name="TInterceptor">Type of interceptor. Must by <see cref="ICallableInterceptor"/>.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="predicate">Additional interceptor parameters.</param>
        /// <returns>The <see cref="IServiceCollection" /> to add the service to.</returns>
        /// <exception cref="ArgumentNullException">
        /// predicate
        /// </exception>
        public static IServiceCollection AddInterceptedDecorator<TInterceptor>(this IServiceCollection services, Predicate<Type> predicate)
            where TInterceptor : ICallableInterceptor
        {
            return services.AddInterceptedDecorator(predicate, typeof(TInterceptor));
        }

        private static void CheckInterceptorConstructor(Type interceptorType, List<ServiceDescriptor> descriptors)
        {
            Type[] requiredTypes = TypeHelper.GetConstructorRequiredTypes(interceptorType);
            List<Type> conflictTypes = new List<Type>();
            for (int i = 0; i < requiredTypes.Length; i++)
            {
                foreach (ServiceDescriptor descriptor in descriptors)
                {
                    if (requiredTypes[i] == descriptor.ServiceType)
                    {
                        conflictTypes.Add(requiredTypes[i]);
                    }
                }
            }

            if (conflictTypes.Count > 0)
            {
                string message = $"Interceptor of type '{interceptorType.FullName}' required in constructor types, which is intercepted. Conflict types: {string.Join(Environment.NewLine, conflictTypes)}";
                throw new ArgumentException(message, nameof(interceptorType));
            }
        }
    }
}
