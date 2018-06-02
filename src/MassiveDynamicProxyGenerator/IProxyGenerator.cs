using System;

namespace MassiveDynamicProxyGenerator
{
    /// <summary>
    /// Dynamic proxy generator.
    /// </summary>
    public interface IProxyGenerator
    {
        /// <summary>
        /// Generates the decorator.
        /// </summary>
        /// <typeparam name="T">Type of decorator.</typeparam>
        /// <param name="interceptor">The interceptor.</param>
        /// <param name="parent">The parent.</param>
        /// <returns>Instance of decorator.</returns>
        T GenerateDecorator<T>(ICallableInterceptor interceptor, T parent)
            where T : class;

        /// <summary>
        /// Generates the decorator.
        /// </summary>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="interceptor">The interceptor.</param>
        /// <param name="parent">The parent.</param>
        /// <returns>Instance of decorator.</returns>
        object GenerateDecorator(Type interfaceType, ICallableInterceptor interceptor, object parent);

        /// <summary>
        /// Generates the instance proxy.
        /// </summary>
        /// <typeparam name="T">Type of proxy.</typeparam>
        /// <param name="instanceProvider">The instance provider.</param>
        /// <returns>Instance of proxy class with instance provider.</returns>
        T GenerateInstanceProxy<T>(IInstanceProvicer instanceProvider)
            where T : class;

        /// <summary>
        /// Generates the instance proxy.
        /// </summary>
        /// <param name="proxyType">Type of proxy.</param>
        /// <param name="instanceProvider">The instance provider.</param>
        /// <returns>Instance of proxy class with instance provider.</returns>
        object GenerateInstanceProxy(Type proxyType, IInstanceProvicer instanceProvider);

        /// <summary>
        /// Generates the proxy instance with interceptor.
        /// </summary>
        /// <param name="interfaceType">Type of the interface for implementation proxy.</param>
        /// <param name="interceptor">The interceptor.</param>
        /// <returns>Instance of implementation of type <paramref name="interfaceType"/>.</returns>
        object GenerateProxy(Type interfaceType, IInterceptor interceptor);

        /// <summary>
        /// Generates the proxy with multiple interfaces implementation.
        /// </summary>
        /// <param name="interceptor">The interceptor.</param>
        /// <param name="additionalTypes">The additional interface types.</param>
        /// <returns>
        /// Instance of proxy class generated with multiple interfaces.
        /// </returns>
        object GenerateProxy(IInterceptor interceptor, params Type[] additionalTypes);

        /// <summary>
        /// Generates the proxy with multiple interfaces implementation.
        /// </summary>
        /// <param name="interfaceType">Type of the interface for decorator.</param>
        /// <param name="interceptor">The interceptor.</param>
        /// <param name="additionalTypes">The additional interface types.</param>
        /// <returns>
        /// Instance of proxy class generated with multiple interfaces.
        /// </returns>
        object GenerateProxy(Type interfaceType, IInterceptor interceptor, params Type[] additionalTypes);

        /// <summary>
        /// Generates the proxy instance with interceptor.
        /// </summary>
        /// <param name="interfaceType">Type of the interface for implementation proxy.</param>
        /// <param name="interceptor">The interceptor.</param>
        /// <param name="containsProperties">if set to <c>true</c> contains properties to interception.</param>
        /// <returns>Instance of implementation of type <paramref name="interfaceType"/>.</returns>
        object GenerateProxy(Type interfaceType, IInterceptor interceptor, bool containsProperties);

        /// <summary>
        /// Generates the proxy instance with interceptor.
        /// </summary>
        /// <typeparam name="T">Type of interface for generate proxy.</typeparam>
        /// <param name="interceptor">The interceptor.</param>
        /// <returns>Instance of <typeparamref name="T"/> implementation as proxy generator.</returns>
        T GenerateProxy<T>(IInterceptor interceptor)
            where T : class;

        /// <summary>
        /// Generates the proxy with multiple interfaces implementation.
        /// </summary>
        /// <typeparam name="T">Type of base interface.</typeparam>
        /// <param name="interceptor">The interceptor.</param>
        /// <param name="additionalTypes">The additional interface types.</param>
        /// <returns>Instance of proxy class generated with multiple interfaces.</returns>
        T GenerateProxy<T>(IInterceptor interceptor, params Type[] additionalTypes)
            where T : class;

        /// <summary>
        /// Generates the proxy instance with interceptor.
        /// </summary>
        /// <typeparam name="T">Type of interface for generate proxy.</typeparam>
        /// <param name="interceptor">The interceptor.</param>
        /// <param name="containsProperties">if set to <c>true</c> contains properties to interception.</param>
        /// <returns>Instance of <typeparamref name="T"/> implement as proxy generator.</returns>
        T GenerateProxy<T>(IInterceptor interceptor, bool containsProperties)
            where T : class;
    }
}