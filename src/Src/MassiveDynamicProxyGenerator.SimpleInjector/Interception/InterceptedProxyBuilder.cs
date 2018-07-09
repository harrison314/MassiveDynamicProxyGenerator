using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using SimpleInjector;

namespace MassiveDynamicProxyGenerator.SimpleInjector.Interception
{
    /// <summary>
    /// Builder for proxy interception.
    /// </summary>
    internal abstract class InterceptedProxyBuilder
    {
        protected static readonly MethodInfo GenerateProxyMethod = typeof(IProxyGenerator).GetTypeInfo()
                    .GetMethod(nameof(IProxyGenerator.GenerateProxy), new[] { typeof(Type), typeof(IInterceptor) });

        private readonly IProxyGenerator generator;

        public InterceptedProxyBuilder(IProxyGenerator generator)
        {
            this.generator = generator;
        }

        public void ResolveUnregisteredType(object sender, UnregisteredTypeEventArgs unregistredTypeArgs)
        {
            if (unregistredTypeArgs.UnregisteredServiceType.GetTypeInfo().IsInterface && this.CheckTypeToIntercept(unregistredTypeArgs.UnregisteredServiceType))
            {
                Expression generator = Expression.Constant(this.generator, typeof(IProxyGenerator));
                Expression interceptor = this.BuildInterceptionExpression((Container)sender, unregistredTypeArgs.UnregisteredServiceType);
                Expression typeOfInstance = Expression.Constant(unregistredTypeArgs.UnregisteredServiceType, typeof(Type));
                Expression crateInstance = Expression.Call(generator, GenerateProxyMethod, typeOfInstance, interceptor);

                unregistredTypeArgs.Register(Expression.Convert(crateInstance, unregistredTypeArgs.UnregisteredServiceType));
            }
        }

        /// <summary>
        /// Builds the expressin for create interceptor instance.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="typeToIntercept">The type to intercept.</param>
        /// <returns>Expressin for create interceptor instance. Must by type of <see cref="IInterceptor"/>.</returns>
        protected abstract Expression BuildInterceptionExpression(Container container, Type typeToIntercept);

        /// <summary>
        /// Checks if specified type is suitable to generate proxy.
        /// </summary>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <returns><c>true</c> if specified type is suitable to generate proxy; otherwise <c>false</c>.</returns>
        protected abstract bool CheckTypeToIntercept(Type interfaceType);
    }
}
