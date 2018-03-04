using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace MassiveDynamicProxyGenerator.Microsoft.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInstanceProxy(this IServiceCollection serviceCollection, Type serviceType, IInstanceProvicer instanceProvicer)
        {
            throw new NotImplementedException();
        }

        public static IServiceCollection AddInstanceProxy<TService>(this IServiceCollection serviceCollection, IInstanceProvicer instanceProvicer)
            where TService : class
        {
            return serviceCollection.AddInstanceProxy(typeof(TService), instanceProvicer);
        }

        public static IServiceCollection AddInstanceProxy(this IServiceCollection serviceCollection, Type serviceType, IInstanceProvicer instanceProvicer, ServiceLifetime proxyLifetime)
        {
            throw new NotImplementedException();
        }

        public static IServiceCollection AddInstanceProxy<TService>(this IServiceCollection serviceCollection, IInstanceProvicer instanceProvicer, ServiceLifetime proxyLifetime)
              where TService : class
        {
            return serviceCollection.AddInstanceProxy(typeof(TService), instanceProvicer, proxyLifetime);
        }

        public static IServiceCollection AddInstanceProxy(this IServiceCollection serviceCollection, Type serviceType, Func<object> instanceProvider)
        {
            throw new NotImplementedException();
        }

        public static IServiceCollection AddInstanceProxy<TService>(this IServiceCollection serviceCollection, Func<TService> instanceProvider)
             where TService : class
        {
            return serviceCollection.AddInstanceProxy(typeof(TService), instanceProvider);
        }

        public static IServiceCollection AddInstanceProxy(this IServiceCollection serviceCollection, Type serviceType, Func<object> instanceProvider, ServiceLifetime proxyLifetime)
        {
            throw new NotImplementedException();
        }

        public static IServiceCollection AddInstanceProxy<TService>(this IServiceCollection serviceCollection, Func<TService> instanceProvider, ServiceLifetime proxyLifetime)
            where TService : class
        {
            return serviceCollection.AddInstanceProxy(typeof(TService), instanceProvider, proxyLifetime);
        }
    }
}
