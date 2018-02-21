using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MassiveDynamicProxyGenerator.Microsoft.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
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

        public static void AddInterceptedDecorator(this IServiceCollection services, ICallableInterceptor interceptor, Predicate<Type> predicate)
        {

        }
    }
}
