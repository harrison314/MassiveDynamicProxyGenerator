using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using SimpleInjector;

namespace MassiveDynamicProxyGenerator.SimpleInjector
{
    // TODO: lifestyles
    public static partial class ContainerExtensions
    {
        public static void RegisterInstanceProxy(this Container container, Type serviceType, Type instancePoxytype)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (instancePoxytype == null)
            {
                throw new ArgumentNullException(nameof(instancePoxytype));
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

            //container.Register(serviceType, ()=>)
        }

        public static void RegisterInstanceProxy<TService, TInstanceProvider>(this Container container)
            where TService : class
            where TInstanceProvider : IInstanceProvicer
        {

        }

        public static void RegisterInstanceProxy(this Container container, Type serviceType, IInstanceProvicer instanceProvider)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (instanceProvider == null)
            {
                throw new ArgumentNullException(nameof(instanceProvider));
            }

            if (!TypeHelper.IsPublicInterface(serviceType))
            {
                throw new ArgumentException($"Parameter {nameof(serviceType)} is not public interface.", nameof(serviceType));
            }

            ProxygGenerator generator = new ProxygGenerator();

            //container.Register(serviceType, ()=> generator.GenerateInstanceProxy())
        }

        public static void RegisterInstanceProxy<TService>(this Container container, IInstanceProvicer instanceProvider)
            where TService : class
        {
            if (instanceProvider == null)
            {
                throw new ArgumentNullException(nameof(instanceProvider));
            }

            if (!TypeHelper.IsPublicInterface(typeof(TService)))
            {
                throw new ArgumentException($"Generic parameter {nameof(TService)} is not public interface.", nameof(TService));
            }

            ProxygGenerator generator = new ProxygGenerator();
            container.Register(typeof(TService), () => generator.GenerateInstanceProxy<TService>(instanceProvider));
        }

        public static void RegisterInstanceProxy(this Container container, Type serviceType, Func<object> instanceProvider)
        {

        }

        public static void RegisterInstanceProxy<TService>(this Container container, Func<TService> instanceProvider)
            where TService : class
        {
            if (instanceProvider == null)
            {
                throw new ArgumentNullException(nameof(instanceProvider));
            }

            if (!TypeHelper.IsPublicInterface(typeof(TService)))
            {
                throw new ArgumentException($"Generic parameter {nameof(TService)} is not public interface.", nameof(TService));
            }

            ProxygGenerator generator = new ProxygGenerator();
            IInstanceProvicer provider = new FuncInstanceProvider(instanceProvider);
            container.Register(typeof(TService), () => generator.GenerateInstanceProxy<TService>(provider));
        }
    }
}
