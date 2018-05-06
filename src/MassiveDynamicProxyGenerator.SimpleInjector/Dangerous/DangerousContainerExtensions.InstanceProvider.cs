using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using SimpleInjector;
using MassiveDynamicProxyGenerator.SimpleInjector.InstanceProxy;

namespace MassiveDynamicProxyGenerator.SimpleInjector.Dangerous
{
    public static partial class DangerousContainerExtensions
    {
        /// <summary>
        /// Registers the dangerous instance proxy with non-transient lifestyle.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="serviceType">Type of the service, accept open generic.</param>
        /// <param name="instanceProviderType">Type of the instance provider, accept open generic. Must by assignable to <see cref="IInstanceProvicer"/>.</param>
        /// <param name="lifeStyle">The life style for registration.</param>
        /// <exception cref="System.ArgumentNullException">
        /// serviceType
        /// or
        /// instanceProviderType
        /// or
        /// lifeStyle
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// Parameter serviceType is not public interface.
        /// or
        /// Parameter instanceProviderType assignable to IInstanceProvicer.
        /// or
        /// ArgumentsserviceType and instanceProviderType must by both concerete types or open generic types.
        /// or
        /// Arguments serviceType and instanceProviderType must have the same count of generic arguments.
        /// </exception>
        public static void RegisterInstanceProxy(this Container container, Type serviceType, Type instanceProviderType, Lifestyle lifeStyle)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (instanceProviderType == null)
            {
                throw new ArgumentNullException(nameof(instanceProviderType));
            }

            if (lifeStyle == null)
            {
                throw new ArgumentNullException(nameof(lifeStyle));
            }

            if (!TypeHelper.IsPublicInterface(serviceType))
            {
                throw new ArgumentException($"Parameter {nameof(serviceType)} is not public interface.", nameof(serviceType));
            }

            if (!typeof(IInstanceProvicer).GetTypeInfo().IsAssignableFrom(instanceProviderType))
            {
                throw new ArgumentException($"Type '{instanceProviderType.AssemblyQualifiedName}' is not assignable to '{typeof(IInstanceProvicer).AssemblyQualifiedName}'.", nameof(instanceProviderType));
            }

            bool isServiceTypeOpenGeneric = TypeHelper.IsOpenGeneric(serviceType);
            bool isInstanceProviderOpenGeneric = TypeHelper.IsOpenGeneric(instanceProviderType);

            if (isServiceTypeOpenGeneric == true && isInstanceProviderOpenGeneric == false)
            {
                throw new ArgumentException($"Arguments {nameof(serviceType)} and {nameof(instanceProviderType)} must by both concerete types or open generic types. Argument {nameof(serviceType)} is open generic type and {nameof(instanceProviderType)} is not open generic type.");
            }

            if (isServiceTypeOpenGeneric == false && isInstanceProviderOpenGeneric == true)
            {
                throw new ArgumentException($"Arguments {nameof(serviceType)} and {nameof(instanceProviderType)} must by both concerete types or open generic types. Argument {nameof(instanceProviderType)} is open generic type and {nameof(serviceType)} is not open generic type.");
            }

            IProxyGenerator generator = ProxyGeneratorFactory.Factory.GetInstance();

            if (isServiceTypeOpenGeneric)
            {
                int serviceTypeOpenGenericParamCount = serviceType.GetTypeInfo().GetGenericArguments().Length;
                int instancePoxyaOpenGenericPramCount = instanceProviderType.GetTypeInfo().GetGenericArguments().Length;

                if (serviceTypeOpenGenericParamCount != instancePoxyaOpenGenericPramCount)
                {
                    throw new ArgumentException($"Arguments {nameof(serviceType)} and {nameof(instanceProviderType)} must have the same count of generic arguments. The {nameof(serviceType)} is '{serviceType.FullName}' and {nameof(instanceProviderType)} is '{instanceProviderType.FullName}'.");
                }

                OpenInstanceProxyBuildProxy builder = new OpenInstanceProxyBuildProxy(generator, serviceType, instanceProviderType);
                container.ResolveUnregisteredType += builder.ResolveUnregisteredType;
            }
            else
            {
                Registration registration = new Registrations.InstanceProxyWithTypeRegistration(container.Options.DefaultLifestyle,
                    container,
                    serviceType,
                    instanceProviderType,
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

            IProxyGenerator generator = ProxyGeneratorFactory.Factory.GetInstance();
            Registration registration = new Registrations.InstanceProxyWithTypeRegistration(container.Options.DefaultLifestyle,
                    container,
                    typeof(TService),
                    typeof(TInstanceProvider),
                    generator);

            container.AddRegistration(typeof(TService), registration);
        }
    }
}
