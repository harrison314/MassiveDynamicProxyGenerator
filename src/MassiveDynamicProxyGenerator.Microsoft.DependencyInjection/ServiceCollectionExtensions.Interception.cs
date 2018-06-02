using MassiveDynamicProxyGenerator.Microsoft.DependencyInjection.ServiceProvider;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MassiveDynamicProxyGenerator.Microsoft.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add intercepted decorator to type <typeparamref name="TService" />.
        /// </summary>
        /// <typeparam name="TService">Type of the decorated service. Must by public interface.</typeparam>
        /// <typeparam name="TInterceptor">Type of interceptor.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <returns>
        /// The <see cref="IServiceCollection" /> to add the service to.
        /// </returns>
        /// <exception cref="ArgumentException">TService</exception>
        public static IServiceCollection AddInterceptedDecorator<TService, TInterceptor>(this IServiceCollection services)
           where TService : class
           where TInterceptor : ICallableInterceptor
        {
            if (!TypeHelper.IsPublicInterface(typeof(TService)))
            {
                throw new ArgumentException($"Parameter {nameof(TService)} of type '{typeof(TService).AssemblyQualifiedName}' is not public interface.");
            }

            EnshureRegistration(services).Add(typeof(TService), typeof(TInterceptor));
            return services;
        }

        /// <summary>
        /// Add intercepted decorator to type <paramref name="serviceType"/> using interceptor type <paramref name="interceptorType"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceType">Type of the decorated service. Must by public interface.</param>
        /// <param name="interceptorType">Type of interceptor. Must implement <see cref="ICallableInterceptor"/>.</param>
        /// <returns>The <see cref="IServiceCollection" /> to add the service to.</returns>
        /// <exception cref="ArgumentNullException">
        /// serviceType
        /// or
        /// interceptorType
        /// </exception>
        /// <exception cref="ArgumentException"></exception>
        public static IServiceCollection AddInterceptedDecorator(this IServiceCollection services, Type serviceType, Type interceptorType)
        {
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (interceptorType == null) throw new ArgumentNullException(nameof(interceptorType));

            if (!TypeHelper.IsPublicInterface(serviceType))
            {
                throw new ArgumentException($"Parameter {nameof(serviceType)} of type '{serviceType.AssemblyQualifiedName}' is not public interface.");
            }

            if (!typeof(ICallableInterceptor).IsAssignableFrom(interceptorType))
            {
                throw new ArgumentException($"Parameter {nameof(interceptorType)} of type '{interceptorType.AssemblyQualifiedName}' must implement {typeof(ICallableInterceptor).AssemblyQualifiedName}.");
            }

            EnshureRegistration(services).Add(serviceType, interceptorType);

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
                throw new ArgumentException($"Parameter {nameof(serviceType)} of type '{serviceType.AssemblyQualifiedName}' is not public interface.");
            }

            EnshureRegistration(services).Add(t => t == serviceType, sp => interceptor); //TODO: open generic parameters
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
                throw new ArgumentException($"Parameter {nameof(serviceType)} of type '{serviceType.AssemblyQualifiedName}' is not public interface.");
            }

            EnshureRegistration(services).Add(t => t == serviceType, interceptorFactory); //TODO: open generic parameters

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

            EnshureRegistration(services).Add(predicate, _ => interceptor);

            return services;
        }

        /// <summary>
        /// Add intercepted decorator to multiple services identified <paramref name="predicate"/> and intercepted created using <paramref name="interceptorFactory"/>.
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

            EnshureRegistration(services).Add(predicate, interceptorFactory);

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
                throw new ArgumentException($"Parameter {nameof(interceptorType)} of type '{interceptorType.AssemblyQualifiedName}' must implement {typeof(ICallableInterceptor).AssemblyQualifiedName}.");
            }

            EnshureRegistration(services).Add(predicate, sp => (ICallableInterceptor)ActivatorUtilities.CreateInstance(sp, interceptorType, interceptorParams));

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

        private static Registrations EnshureRegistration(IServiceCollection services)
        {
            foreach (ServiceDescriptor register in services)
            {
                if (register.ImplementationType == typeof(Registrations))
                {
                    return (Registrations)register.ImplementationInstance;
                }
            }

            Registrations registration = new Registrations();
            services.Add(new ServiceDescriptor(typeof(Registrations), registration));
            services.TryAddTransient(typeof(IOriginalService<>), typeof(OriginalService<>));

            return registration;
        }
    }
}
