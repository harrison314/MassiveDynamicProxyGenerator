using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MassiveDynamicProxyGenerator.Microsoft.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
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
    }
}
