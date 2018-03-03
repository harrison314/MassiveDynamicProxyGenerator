using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MassiveDynamicProxyGenerator.Microsoft.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
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

        public static IServiceCollection AddProxy<TService>(this IServiceCollection services)
            where TService : class
        {
            ProxygGenerator generator = new ProxygGenerator();
            services.AddTransient<TService>(t => generator.GenerateProxy<TService>(NullInterceptor.Instance));

            return services;
        }

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
    }
}
