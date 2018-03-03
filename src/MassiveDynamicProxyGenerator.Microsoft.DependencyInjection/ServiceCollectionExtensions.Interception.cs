using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MassiveDynamicProxyGenerator.Microsoft.DependencyInjection
{
    //TODO: implement for non open-generic types!!!!
    public static partial class ServiceCollectionExtensions
    {
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
