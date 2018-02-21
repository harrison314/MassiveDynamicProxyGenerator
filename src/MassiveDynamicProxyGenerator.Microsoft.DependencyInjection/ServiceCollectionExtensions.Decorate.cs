using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MassiveDynamicProxyGenerator.Microsoft.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDecorator(this IServiceCollection services, Type serviceType, Type decoratorType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (decoratorType == null)
            {
                throw new ArgumentNullException(nameof(decoratorType));
            }

            if (TypeHelper.IsOpenGeneric(serviceType))
            {
                throw new ArgumentException($"Service type {serviceType} can not open generic type.");
            }

            if (TypeHelper.IsOpenGeneric(decoratorType))
            {
                throw new ArgumentException($"Decorator type {decoratorType} can not open generic type.");
            }

            List<ServiceDescriptor> descriptors = GetDescriptors(services, serviceType);
            foreach (ServiceDescriptor descriptor in descriptors)
            {
                int index = services.IndexOf(descriptor);

                //ak ide o otvoreny gynericky typ pouzit IConvertible
                // a vo vnutri volat ActivatorUtilities a dorobit testy
                ServiceDescriptor decoratedDescriptor = ServiceDescriptor.Describe(descriptor.ServiceType,
                   provider => ActivatorUtilities.CreateInstance(provider, decoratorType, GetInstanceFromDescriptor(provider, descriptor)),
                   descriptor.Lifetime);

                services.Insert(index, decoratedDescriptor);

                services.Remove(descriptor);
            }

            return services;
        }

        public static IServiceCollection AddDecorator<TInterface, TServise>(this IServiceCollection services)
            where TInterface : class
            where TServise : TInterface

        {
            return AddDecorator(services, typeof(TInterface), typeof(TServise));
        }
    }
}
