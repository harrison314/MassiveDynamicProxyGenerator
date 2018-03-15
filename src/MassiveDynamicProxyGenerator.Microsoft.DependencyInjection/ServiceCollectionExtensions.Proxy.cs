using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MassiveDynamicProxyGenerator.Microsoft.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add proxy of type <typeparamref name="TService"/> using <paramref name="interceptor"/>.
        /// </summary>
        /// <typeparam name="TService">The created service. Must by public interface.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="interceptor">The interceptor to create proxy.</param>
        /// <returns>The <see cref="IServiceCollection" /> to add the service to.</returns>
        /// <exception cref="ArgumentNullException">
        /// interceptor
        /// </exception>
        public static IServiceCollection AddProxy<TService>(this IServiceCollection services, IInterceptor interceptor)
           where TService : class
        {
            if (interceptor == null)
            {
                throw new ArgumentNullException(nameof(interceptor));
            }

            ProxygGenerator generator = new ProxygGenerator();

            services.AddTransient<TService>(t => generator.GenerateProxy<TService>(interceptor));

            return services;
        }

        /// <summary>
        /// Add proxy of type <paramref name="serviceType"/> using <paramref name="interceptor"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceType">The created service. Must by public interface.</param>
        /// <param name="interceptor">The interceptor to create proxy.</param>
        /// <returns>The <see cref="IServiceCollection" /> to add the service to.</returns>
        /// <exception cref="ArgumentNullException">
        /// serviceType
        /// or
        /// interceptor
        /// </exception>
        /// <exception cref="ArgumentException"></exception>
        public static IServiceCollection AddProxy(this IServiceCollection services, Type serviceType, IInterceptor interceptor)
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
                throw new ArgumentException($"Parameter {nameof(serviceType)} of type '{serviceType.AssemblyQualifiedName}' is not public interface.");
            }

            ProxygGenerator generator = new ProxygGenerator();

            services.AddTransient(serviceType, t => generator.GenerateProxy(serviceType, interceptor));

            return services;
        }

        /// <summary>
        /// Add proxy of type <typeparamref name="TService"/> using <paramref name="interceptor"/>.
        /// </summary>
        /// <typeparam name="TService">The created service. Must by public interface.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="interceptor">The interceptor to create proxy as <see cref="Action"/>.</param>
        /// <returns>The <see cref="IServiceCollection" /> to add the service to.</returns>
        /// <exception cref="ArgumentNullException">
        /// interceptor
        /// </exception>
        public static IServiceCollection AddProxy<TService>(this IServiceCollection services, Action<IInvocation> interceptor)
            where TService : class
        {
            if (interceptor == null)
            {
                throw new ArgumentNullException(nameof(interceptor));
            }

            ProxygGenerator generator = new ProxygGenerator();
            IInterceptor realInteceptor = new InterceptorAdapter(invocation => interceptor(invocation));
            services.AddTransient<TService>(t => generator.GenerateProxy<TService>(realInteceptor));

            return services;
        }

        /// <summary>
        /// Add proxy of type <paramref name="serviceType"/> using <paramref name="interceptor"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceType">The created service. Must by public interface.</param>
        /// <param name="interceptor">The interceptor to create proxy as <see cref="Action"/>.</param>
        /// <returns>The <see cref="IServiceCollection" /> to add the service to.</returns>
        /// <exception cref="ArgumentNullException">
        /// serviceType
        /// or
        /// interceptor
        /// </exception>
        /// <exception cref="ArgumentException"></exception>
        public static IServiceCollection AddProxy(this IServiceCollection services, Type serviceType, Action<IInvocation> interceptor)
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
                throw new ArgumentException($"Parameter {nameof(serviceType)} of type '{serviceType.AssemblyQualifiedName}' is not public interface.");
            }

            ProxygGenerator generator = new ProxygGenerator();
            IInterceptor realInteceptor = new InterceptorAdapter(invocation => interceptor(invocation));
            services.AddTransient(serviceType, t => generator.GenerateProxy(serviceType, realInteceptor));

            return services;
        }

        /// <summary>
        /// Add null pattern proxy to <typeparamref name="TService"/>.
        /// </summary>
        /// <typeparam name="TService">The created service. Must by public interface.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <returns>The <see cref="IServiceCollection" /> to add the service to.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static IServiceCollection AddProxy<TService>(this IServiceCollection services)
            where TService : class
        {
            ProxygGenerator generator = new ProxygGenerator();
            services.AddTransient<TService>(t => generator.GenerateProxy<TService>(NullInterceptor.Instance));

            return services;
        }

        /// <summary>
        /// Add null pattern proxy to <paramref name="serviceType"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceType">The created service. Must by public interface.</param>
        /// <returns>The <see cref="IServiceCollection" /> to add the service to.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static IServiceCollection AddProxy(this IServiceCollection services, Type serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (!TypeHelper.IsPublicInterface(serviceType))
            {
                throw new ArgumentException($"Parameter {nameof(serviceType)} of type '{serviceType.AssemblyQualifiedName}' is not public interface.");
            }
            ProxygGenerator generator = new ProxygGenerator();
            services.AddTransient(serviceType, t => generator.GenerateProxy(serviceType, NullInterceptor.Instance));

            return services;
        }

        /// <summary>
        /// Add proxy for type <paramref name="serviceType"/> using interceptor of <paramref name="interceptorType"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceType">The created service. Must by public interface.</param>
        /// <param name="interceptorType">The interceptor type.</param>
        /// <param name="interceptorParams">Additional interceptor parameters.</param>
        /// <returns>The <see cref="IServiceCollection" /> to add the service to.</returns>
        /// <exception cref="ArgumentNullException">
        /// serviceType
        /// or
        /// interceptorType
        /// </exception>
        /// <exception cref="ArgumentException"></exception>
        public static IServiceCollection AddProxy(this IServiceCollection services, Type serviceType, Type interceptorType, params object[] interceptorParams)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (interceptorType == null)
            {
                throw new ArgumentNullException(nameof(interceptorType));
            }

            if (!TypeHelper.IsPublicInterface(serviceType))
            {
                throw new ArgumentException($"Parameter {nameof(serviceType)} of type '{serviceType.AssemblyQualifiedName}' is not public interface.");
            }

            if (!typeof(IInterceptor).IsAssignableFrom(interceptorType))
            {
                throw new ArgumentException($"Parameter {nameof(interceptorType)} of type '{serviceType.AssemblyQualifiedName}' is IInterceptor.");
            }

            ProxygGenerator generator = new ProxygGenerator();
            services.AddTransient(serviceType, t => generator.GenerateProxy(serviceType, (IInterceptor)ActivatorUtilities.CreateInstance(t, interceptorType, interceptorParams)));

            return services;
        }

        /// <summary>
        /// Add proxy for type <typeparamref name="TService"/> using interceptor of type <typeparamref name="TInterceptor"/>.
        /// </summary>
        /// <typeparam name="TService">The created service. Must by public interface.</typeparam>
        /// <typeparam name="TInterceptor">The interceptor type.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="interceptorParams">Additional interceptor parameters.</param>
        /// <returns>The <see cref="IServiceCollection" /> to add the service to.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static IServiceCollection AddProxy<TService, TInterceptor>(this IServiceCollection services, params object[] interceptorParams)
            where TService : class
            where TInterceptor : IInterceptor
        {
            return services.AddProxy(typeof(TService), typeof(TInterceptor), interceptorParams: interceptorParams);
        }

        /// <summary>
        /// Add proxy for type <paramref name="serviceType"/> using interceptor created with <paramref name="interceptorFactory"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceType">The created service. Must by public interface.</param>
        /// <param name="interceptorFactory">The interceptor factory.</param>
        /// <returns>The <see cref="IServiceCollection" /> to add the service to.</returns>
        /// <exception cref="ArgumentNullException">
        /// serviceType
        /// or
        /// interceptorFactory
        /// </exception>
        /// <exception cref="ArgumentException"></exception>
        public static IServiceCollection AddProxy(this IServiceCollection services, Type serviceType, Func<IServiceProvider, IInterceptor> interceptorFactory)
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
                throw new ArgumentException($"Parameter {nameof(serviceType)} of type '{serviceType.AssemblyQualifiedName}' is not public interface.");
            }

            ProxygGenerator generator = new ProxygGenerator();
            services.AddTransient(serviceType, t => generator.GenerateProxy(serviceType, interceptorFactory.Invoke(t)));

            return services;
        }

        /// <summary>
        /// Add proxy for type <typeparamref name="TService"/> sing interceptor created with <paramref name="interceptorFactory"/>.
        /// </summary>
        /// <typeparam name="TService">The created service. Must by public interface.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="interceptorFactory">The interceptor factory.</param>
        /// <returns>The <see cref="IServiceCollection" /> to add the service to.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static IServiceCollection AddProxy<TService>(this IServiceCollection services, Func<IServiceProvider, IInterceptor> interceptorFactory)
            where TService : class
        {
            return services.AddProxy(typeof(TService), interceptorFactory);
        }
    }
}
