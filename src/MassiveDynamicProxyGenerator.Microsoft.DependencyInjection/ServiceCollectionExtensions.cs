using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MassiveDynamicProxyGenerator.Microsoft.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {


       

       

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
