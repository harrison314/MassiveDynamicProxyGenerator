using MassiveDynamicProxyGenerator;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApplication.IocExtensions
{
    public static class IocDecoratorExtensions
    {
        public static IServiceCollection Decorate(this IServiceCollection services, Type serviceType, Type decoratorType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (decoratorType == null)
            {
                throw new ArgumentNullException(nameof(decoratorType));
            }

            List<ServiceDescriptor> descriptors = GetDescriptors(services, serviceType);
            foreach (ServiceDescriptor descriptor in descriptors)
            {
                int index = services.IndexOf(descriptor);


                ServiceDescriptor decoratedDescriptor = ServiceDescriptor.Describe(descriptor.ServiceType,
                provider => ActivatorUtilities.CreateInstance(provider, decoratorType, GetInstanceFromDescriptor(provider, descriptor)),
                descriptor.Lifetime);

                services.Insert(index, decoratedDescriptor);

                services.Remove(descriptor);
            }

            return services;
        }

        public static IServiceCollection Decorate<TInterface, TServise>(this IServiceCollection services)
            where TInterface : class
            where TServise : TInterface

        {
            return Decorate(services, typeof(TInterface), typeof(TServise));
        }

        public static IServiceCollection Intercept<TServise, TInterceptor>(this IServiceCollection services, params object[] interceptorParams)
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

        private static List<ServiceDescriptor> GetDescriptors(this IServiceCollection services, Type serviceType)
        {
            List<ServiceDescriptor> descriptors = new List<ServiceDescriptor>();

            foreach (ServiceDescriptor service in services)
            {
                if (service.ServiceType == serviceType)
                {
                    descriptors.Add(service);
                }
            }

            if (descriptors.Count == 0)
            {
                throw new InvalidOperationException($"Could not find any registered services for type '{serviceType.FullName}'.");
            }

            return descriptors;
        }

        private static object GetInstanceFromDescriptor(IServiceProvider provider, ServiceDescriptor descriptor)
        {
            if (descriptor.ImplementationInstance != null)
            {
                return descriptor.ImplementationInstance;
            }

            if (descriptor.ImplementationType != null)
            {
                return ActivatorUtilities.GetServiceOrCreateInstance(provider, descriptor.ImplementationType);
            }

            return descriptor.ImplementationFactory(provider);
        }
    }
}
