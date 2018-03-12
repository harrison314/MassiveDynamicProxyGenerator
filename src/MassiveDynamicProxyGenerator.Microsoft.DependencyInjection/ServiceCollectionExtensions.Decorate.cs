using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MassiveDynamicProxyGenerator.Microsoft.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the decorator for service of <paramref name="serviceType"/> of type <paramref name="decoratorType"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
        /// <param name="serviceType">Type of the decorated service. Must by public interface.</param>
        /// <param name="decoratorType">Type of the decorator. Must by nonabstract class.</param>
        /// <returns>The <see cref="IServiceCollection"/> to add the service to.</returns>
        /// <exception cref="ArgumentNullException">
        /// serviceType
        /// or
        /// decoratorType
        /// </exception>
        /// <exception cref="ArgumentException">
        /// </exception>
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

                //TODO: ak ide o otvoreny gynericky typ pouzit IConvertible
                // a vo vnutri volat ActivatorUtilities a dorobit testy
                ServiceDescriptor decoratedDescriptor = ServiceDescriptor.Describe(descriptor.ServiceType,
                   provider => ActivatorUtilities.CreateInstance(provider, decoratorType, GetInstanceFromDescriptor(provider, descriptor)),
                   descriptor.Lifetime);

                services.Insert(index, decoratedDescriptor);

                services.Remove(descriptor);
            }

            return services;
        }

        /// <summary>
        /// Adds the decorator for service of <typeparamref name="TService"/> of type <typeparamref name="TDecorator"/>.
        /// </summary>
        /// <typeparam name="TService">The type of the intercepted service. Must by public interface.</typeparam>
        /// <typeparam name="TDecorator">Type of the decorator. Must by nonabstract class.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
        /// <returns>The <see cref="IServiceCollection"/> to add the service to.</returns>
        public static IServiceCollection AddDecorator<TService, TDecorator>(this IServiceCollection services)
            where TService : class
            where TDecorator : TService

        {
            return AddDecorator(services, typeof(TService), typeof(TDecorator));
        }
    }
}
