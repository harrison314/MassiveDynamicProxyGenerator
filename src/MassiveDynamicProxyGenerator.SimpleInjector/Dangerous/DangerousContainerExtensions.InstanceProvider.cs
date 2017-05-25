using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using SimpleInjector;

namespace MassiveDynamicProxyGenerator.SimpleInjector.Dangerous
{
    public static partial class DangerousContainerExtensions
    {
        public static void RegisterInstanceProxy(this Container container, Type serviceType, Type instancePoxyType, Lifestyle lifeStyle)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (instancePoxyType == null)
            {
                throw new ArgumentNullException(nameof(instancePoxyType));
            }

            if (lifeStyle == null)
            {
                throw new ArgumentNullException(nameof(lifeStyle));
            }

            if (!TypeHelper.IsPublicInterface(serviceType))
            {
                throw new ArgumentException($"Parameter {nameof(serviceType)} is not public interface.", nameof(serviceType));
            }

            if (!typeof(IInstanceProvicer).GetTypeInfo().IsAssignableFrom(instancePoxyType))
            {
                throw new ArgumentException($"Type '{instancePoxyType.AssemblyQualifiedName}' is not assignable to '{typeof(IInstanceProvicer).AssemblyQualifiedName}'.", nameof(instancePoxyType));
            }

            bool isServiceTypeOpenGeneric = TypeHelper.IsOpenGeneric(serviceType);
            bool isInstanceProviderOpenGeneric = TypeHelper.IsOpenGeneric(instancePoxyType);

            if (isServiceTypeOpenGeneric == true && isInstanceProviderOpenGeneric == false)
            {
                throw new ArgumentException($"Arguments {nameof(serviceType)} and {nameof(instancePoxyType)} must by both concerete types or open generic types. Argument {nameof(serviceType)} is open generic type and {nameof(instancePoxyType)} is not open generic type.");
            }

            if (isServiceTypeOpenGeneric == false && isInstanceProviderOpenGeneric == true)
            {
                throw new ArgumentException($"Arguments {nameof(serviceType)} and {nameof(instancePoxyType)} must by both concerete types or open generic types. Argument {nameof(instancePoxyType)} is open generic type and {nameof(serviceType)} is not open generic type.");
            }

            ProxygGenerator generator = new ProxygGenerator();

            // TODO: open generic types

            if (isServiceTypeOpenGeneric)
            {
                throw new NotImplementedException();
            }
            else
            {
                Registration registration = new Registrations.InstanceProxyWithTypeRegistration(container.Options.DefaultLifestyle,
                    container,
                    serviceType,
                    instancePoxyType,
                    generator);

                container.AddRegistration(serviceType, registration);
            }
        }

        /// <summary>
        /// Registers the dangerous instance proxy with non-transient lifestyle.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TInstanceProvider">The type of the instance provider.</typeparam>
        /// <param name="container">The container.</param>
        /// <param name="lifeStyle">The service life style - must by scoped or singlton.</param>
        /// <exception cref="System.ArgumentNullException">lifeStyle</exception>
        /// <exception cref="System.ArgumentException">Generic parameter TService is not public interface.</exception>
        public static void RegisterInstanceProxy<TService, TInstanceProvider>(this Container container, Lifestyle lifeStyle)
            where TService : class
            where TInstanceProvider : IInstanceProvicer
        {

            if (lifeStyle == null)
            {
                throw new ArgumentNullException(nameof(lifeStyle));
            }

            if (!TypeHelper.IsPublicInterface(typeof(TService)))
            {
                throw new ArgumentException($"Generic parameter {nameof(TService)} is not public interface.", nameof(TService));
            }

            ProxygGenerator generator = new ProxygGenerator();
            Registration registration = new Registrations.InstanceProxyWithTypeRegistration(container.Options.DefaultLifestyle,
                    container,
                    typeof(TService),
                    typeof(TInstanceProvider),
                    generator);

            container.AddRegistration(typeof(TService), registration);
        }
    }
}
