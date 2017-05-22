﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using SimpleInjector;

namespace MassiveDynamicProxyGenerator.SimpleInjector.Interception
{
    /// <summary>
    /// Bulder for prxy interception.
    /// </summary>
    internal abstract class InterceptedProxyBulder
    {
        protected static readonly MethodInfo GenerateProxyMethod = typeof(IProxygGenerator).GetTypeInfo()
                    .GetMethod(nameof(IProxygGenerator.GenerateProxy), new[] { typeof(Type), typeof(IInterceptor) });

        private readonly ProxygGenerator generator;

        public InterceptedProxyBulder(ProxygGenerator generator)
        {
            this.generator = generator;
        }

        public void ResolveUnregisteredType(object sender, UnregisteredTypeEventArgs unregistredTypeArgs)
        {
            if (unregistredTypeArgs.UnregisteredServiceType.GetTypeInfo().IsInterface && this.CheckTypeToIntercept(unregistredTypeArgs.UnregisteredServiceType))
            {
                Expression generator = Expression.Constant(this.generator, typeof(ProxygGenerator));
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