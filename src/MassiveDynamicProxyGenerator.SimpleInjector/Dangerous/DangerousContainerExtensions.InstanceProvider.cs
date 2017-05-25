using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using SimpleInjector;

namespace MassiveDynamicProxyGenerator.SimpleInjector.Dangerous
{
    public static partial class DangerousContainerExtensions
    {
        public static void RegisterInstanceProxy(this Container container, Type serviceType, Type instancePoxytype, Lifestyle lifeStyle)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (instancePoxytype == null)
            {
                throw new ArgumentNullException(nameof(instancePoxytype));
            }

            if (lifeStyle == null)
            {
                throw new ArgumentNullException(nameof(lifeStyle));
            }

            if (!TypeHelper.IsPublicInterface(serviceType))
            {
                throw new ArgumentException($"Parameter {nameof(serviceType)} is not public interface.", nameof(serviceType));
            }

            if (!typeof(IInstanceProvicer).GetTypeInfo().IsAssignableFrom(instancePoxytype))
            {
                throw new ArgumentException($"Type '{instancePoxytype.AssemblyQualifiedName}' is not assignable to '{typeof(IInstanceProvicer).AssemblyQualifiedName}'.", nameof(instancePoxytype));
            }

            ProxygGenerator generator = new ProxygGenerator();

            // TODO: open generic types
            //container.Register(serviceType, ()=>)

            throw new NotImplementedException();
        }

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
