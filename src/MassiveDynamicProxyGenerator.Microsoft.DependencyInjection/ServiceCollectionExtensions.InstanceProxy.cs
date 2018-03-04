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
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (instanceProvicer == null)
            {
                throw new ArgumentNullException(nameof(instanceProvicer));
            }

            if (TypeHelper.IsOpenGeneric(serviceType))
            {
                throw new ArgumentException($"Service type {serviceType} can not open generic type.");
            }

            if (!TypeHelper.IsPublicInterface(serviceType))
            {
                throw new ArgumentException($"Parameter {nameof(serviceType)} of type '{serviceType.AssemblyQualifiedName}' is not public interface.");
            }

            ProxygGenerator proxygGenerator = new ProxygGenerator();
            return serviceCollection.AddSingleton(serviceType, sp => proxygGenerator.GenerateInstanceProxy(serviceType, instanceProvicer));
        }

        public static IServiceCollection AddInstanceProxy<TService>(this IServiceCollection serviceCollection, IInstanceProvicer instanceProvicer)
            where TService : class
        {
            return serviceCollection.AddInstanceProxy(typeof(TService), instanceProvicer);
        }

        public static IServiceCollection AddInstanceProxy(this IServiceCollection serviceCollection, Type serviceType, IInstanceProvicer instanceProvicer, ServiceLifetime proxyLifetime)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (instanceProvicer == null)
            {
                throw new ArgumentNullException(nameof(instanceProvicer));
            }

            if (TypeHelper.IsOpenGeneric(serviceType))
            {
                throw new ArgumentException($"Service type {serviceType} can not open generic type.");
            }

            if (!TypeHelper.IsPublicInterface(serviceType))
            {
                throw new ArgumentException($"Parameter {nameof(serviceType)} of type '{serviceType.AssemblyQualifiedName}' is not public interface.");
            }

            ProxygGenerator proxygGenerator = new ProxygGenerator();

            ServiceDescriptor descriptor = new ServiceDescriptor(serviceType,
                sp => proxygGenerator.GenerateInstanceProxy(serviceType, instanceProvicer),
                proxyLifetime);

            serviceCollection.Add(descriptor);
            return serviceCollection;
        }

        public static IServiceCollection AddInstanceProxy<TService>(this IServiceCollection serviceCollection, IInstanceProvicer instanceProvicer, ServiceLifetime proxyLifetime)
              where TService : class
        {
            return serviceCollection.AddInstanceProxy(typeof(TService), instanceProvicer, proxyLifetime);
        }

        public static IServiceCollection AddInstanceProxy(this IServiceCollection serviceCollection, Type serviceType, Func<object> instnaceFactory)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (instnaceFactory == null)
            {
                throw new ArgumentNullException(nameof(instnaceFactory));
            }

            if (TypeHelper.IsOpenGeneric(serviceType))
            {
                throw new ArgumentException($"Service type {serviceType} can not open generic type.");
            }

            if (!TypeHelper.IsPublicInterface(serviceType))
            {
                throw new ArgumentException($"Parameter {nameof(serviceType)} of type '{serviceType.AssemblyQualifiedName}' is not public interface.");
            }

            ProxygGenerator proxygGenerator = new ProxygGenerator();
            return serviceCollection.AddSingleton(serviceType, sp => proxygGenerator.GenerateInstanceProxy(serviceType, new FuncInstanceProvider(instnaceFactory)));
        }

        public static IServiceCollection AddInstanceProxy<TService>(this IServiceCollection serviceCollection, Func<TService> instanceProvider)
             where TService : class
        {
            return serviceCollection.AddInstanceProxy(typeof(TService), instanceProvider);
        }

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

            ProxygGenerator proxygGenerator = new ProxygGenerator();

            ServiceDescriptor descriptor = new ServiceDescriptor(serviceType,
                sp => proxygGenerator.GenerateInstanceProxy(serviceType, new FuncInstanceProvider(instanceFactory)),
                proxyLifetime);

            serviceCollection.Add(descriptor);
            return serviceCollection;
        }

        public static IServiceCollection AddInstanceProxy<TService>(this IServiceCollection serviceCollection, Func<TService> instanceProvider, ServiceLifetime proxyLifetime)
            where TService : class
        {
            return serviceCollection.AddInstanceProxy(typeof(TService), instanceProvider, proxyLifetime);
        }
    }
}
