using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using SimpleInjector;

namespace MassiveDynamicProxyGenerator.SimpleInjector
{
    public static partial class ContainerExtensions
    {
        /// <summary>
        /// Registers the instance proxy (using instance).
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="instanceProvider">The instance provider crates instance of service type.</param>
        /// <exception cref="System.ArgumentNullException">
        /// serviceType
        /// or
        /// instanceProvider
        /// </exception>
        /// <exception cref="System.ArgumentException">Parameter serviceType is not public interface.</exception>
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

            IProxygGenerator generator = ProxyGeneratorFactory.Factory.GetInstance();

            container.Register(serviceType, () => generator.GenerateInstanceProxy(serviceType, instanceProvider));
        }

        /// <summary>
        /// Registers the instance proxy (using isntance).
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="instanceProvider">The instance provider crates instance of service type.</param>
        /// <param name="lifeStyle">The service life style.</param>
        /// <exception cref="System.ArgumentNullException">
        /// serviceType
        /// or
        /// instanceProvider
        /// or
        /// lifeStyle
        /// </exception>
        /// <exception cref="System.ArgumentException">Parameter serviceType is not public interface.</exception>
        public static void RegisterInstanceProxy(this Container container, Type serviceType, IInstanceProvicer instanceProvider, Lifestyle lifeStyle)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (instanceProvider == null)
            {
                throw new ArgumentNullException(nameof(instanceProvider));
            }

            if (lifeStyle == null)
            {
                throw new ArgumentNullException(nameof(lifeStyle));
            }

            if (!TypeHelper.IsPublicInterface(serviceType))
            {
                throw new ArgumentException($"Parameter {nameof(serviceType)} is not public interface.", nameof(serviceType));
            }

            IProxygGenerator generator = ProxyGeneratorFactory.Factory.GetInstance();

            container.Register(serviceType, () => generator.GenerateInstanceProxy(serviceType, instanceProvider), lifeStyle);
        }

        /// <summary>
        /// Registers the instance proxy (using instance).
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="container">The container.</param>
        /// <param name="instanceProvider">The instance provider crates instance of service type.</param>
        /// <exception cref="System.ArgumentNullException">instanceProvider</exception>
        /// <exception cref="System.ArgumentException">Generic parameterTService is not public interface.</exception>
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

            IProxygGenerator generator = ProxyGeneratorFactory.Factory.GetInstance();
            container.Register(typeof(TService), () => generator.GenerateInstanceProxy<TService>(instanceProvider));
        }

        /// <summary>
        /// Registers the instance proxy (using instance).
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="container">The container.</param>
        /// <param name="instanceProvider">The instance provider crates instance of service type.</param>
        /// <param name="lifeStyle">The life style.</param>
        /// <exception cref="System.ArgumentNullException">
        /// instanceProvider
        /// or
        /// lifeStyle
        /// </exception>
        /// <exception cref="System.ArgumentException">Generic parameter TService is not public interface.</exception>
        public static void RegisterInstanceProxy<TService>(this Container container, IInstanceProvicer instanceProvider, Lifestyle lifeStyle)
           where TService : class
        {
            if (instanceProvider == null)
            {
                throw new ArgumentNullException(nameof(instanceProvider));
            }

            if (lifeStyle == null)
            {
                throw new ArgumentNullException(nameof(lifeStyle));
            }

            if (!TypeHelper.IsPublicInterface(typeof(TService)))
            {
                throw new ArgumentException($"Generic parameter {nameof(TService)} is not public interface.", nameof(TService));
            }

            IProxygGenerator generator = ProxyGeneratorFactory.Factory.GetInstance();
            container.Register(typeof(TService), () => generator.GenerateInstanceProxy<TService>(instanceProvider), lifeStyle);
        }

        /// <summary>
        /// Registers the instance proxy (using function).
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="instanceProvider">The instance provider genrates instance of service type.</param>
        /// <exception cref="System.ArgumentNullException">
        /// serviceType
        /// or
        /// instanceProvider
        /// </exception>
        /// <exception cref="System.ArgumentException">Parameter serviceType is not public interface.</exception>
        public static void RegisterInstanceProxy(this Container container, Type serviceType, Func<object> instanceProvider)
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

            IProxygGenerator generator = ProxyGeneratorFactory.Factory.GetInstance();

            container.Register(serviceType, () =>
            {
                IInstanceProvicer provider = new FuncInstanceProvider(instanceProvider);
                return generator.GenerateInstanceProxy(serviceType, provider);
            });
        }

        /// <summary>
        /// Registers the instance proxy (using function).
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="instanceProvider">The instance provider genrates instance of service type.</param>
        /// <param name="lifeStyle">The service life style.</param>
        /// <exception cref="System.ArgumentNullException">
        /// serviceType
        /// or
        /// instanceProvider
        /// or
        /// lifeStyle
        /// </exception>
        /// <exception cref="System.ArgumentException">Parameter serviceType is not public interface.</exception>
        public static void RegisterInstanceProxy(this Container container, Type serviceType, Func<object> instanceProvider, Lifestyle lifeStyle)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (instanceProvider == null)
            {
                throw new ArgumentNullException(nameof(instanceProvider));
            }

            if (lifeStyle == null)
            {
                throw new ArgumentNullException(nameof(lifeStyle));
            }

            if (!TypeHelper.IsPublicInterface(serviceType))
            {
                throw new ArgumentException($"Parameter {nameof(serviceType)} is not public interface.", nameof(serviceType));
            }

            IProxygGenerator generator = ProxyGeneratorFactory.Factory.GetInstance();

            container.Register(serviceType,
                () =>
                {
                    IInstanceProvicer provider = new FuncInstanceProvider(instanceProvider);
                    return generator.GenerateInstanceProxy(serviceType, provider);
                },
            lifeStyle);
        }

        /// <summary>
        /// Registers the instance proxy (using function).
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="container">The container.</param>
        /// <param name="instanceProvider">The instance provider genrates instance of service type.</param>
        /// <exception cref="System.ArgumentNullException">instanceProvider</exception>
        /// <exception cref="System.ArgumentException">Generic parameter TService is not public interface.</exception>
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

            IProxygGenerator generator = ProxyGeneratorFactory.Factory.GetInstance();
            IInstanceProvicer provider = new FuncInstanceProvider(instanceProvider);
            container.Register(typeof(TService), () => generator.GenerateInstanceProxy<TService>(provider));
        }

        /// <summary>
        /// Registers the instance proxy (using function).
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="container">The container.</param>
        /// <param name="instanceProvider">The instance provider genrates instance of service type.</param>
        /// <param name="lifeStyle">The service life style.</param>
        /// <exception cref="System.ArgumentNullException">
        /// instanceProvider
        /// or
        /// lifeStyle
        /// </exception>
        /// <exception cref="System.ArgumentException">Generic parameter TService is not public interface.</exception>
        public static void RegisterInstanceProxy<TService>(this Container container, Func<TService> instanceProvider, Lifestyle lifeStyle)
            where TService : class
        {
            if (instanceProvider == null)
            {
                throw new ArgumentNullException(nameof(instanceProvider));
            }

            if (lifeStyle == null)
            {
                throw new ArgumentNullException(nameof(lifeStyle));
            }

            if (!TypeHelper.IsPublicInterface(typeof(TService)))
            {
                throw new ArgumentException($"Generic parameter {nameof(TService)} is not public interface.", nameof(TService));
            }

            IProxygGenerator generator = ProxyGeneratorFactory.Factory.GetInstance();
            IInstanceProvicer provider = new FuncInstanceProvider(instanceProvider);
            container.Register(typeof(TService), () => generator.GenerateInstanceProxy<TService>(provider), lifeStyle);
        }
    }
}
