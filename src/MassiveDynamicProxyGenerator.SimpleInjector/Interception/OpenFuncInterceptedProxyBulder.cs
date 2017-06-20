using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using SimpleInjector;

namespace MassiveDynamicProxyGenerator.SimpleInjector.Interception
{
    internal class OpenFuncInterceptedProxyBulder : InterceptedProxyBulder
    {
        private static readonly MethodInfo InvokeMethod = typeof(Func<IInterceptor>).GetTypeInfo().GetMethod(nameof(Func<IInterceptor>.Invoke));

        private readonly Type serviceType;
        private readonly Func<IInterceptor> interceptorFactory;

        public OpenFuncInterceptedProxyBulder(IProxygGenerator generator, Type serviceType, Func<IInterceptor> interceptorFactory)
            : base(generator)
        {
            this.serviceType = serviceType;
            this.interceptorFactory = interceptorFactory;
        }

        protected override Expression BuildInterceptionExpression(Container container, Type typeToIntercept)
        {
            return Expression.Call(Expression.Constant(this.interceptorFactory, typeof(Func<IInterceptor>)), InvokeMethod);
        }

        protected override bool CheckTypeToIntercept(Type interfaceType)
        {
            return TypeHelper.IsGenericConstructedOf(this.serviceType, interfaceType);
        }
    }
}
