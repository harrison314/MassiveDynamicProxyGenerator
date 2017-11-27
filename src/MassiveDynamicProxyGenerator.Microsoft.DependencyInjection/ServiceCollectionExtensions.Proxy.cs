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

        public static IServiceCollection AddProxy<TService>(this IServiceCollection services)
            where TService : class
        {
            ProxygGenerator generator = new ProxygGenerator();
            services.AddTransient<TService>(t => generator.GenerateProxy<TService>(NullInterceptor.Instance));

            return services;
        }
    }
}
