using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;
using SimpleInjector;

namespace MassiveDynamicProxyGenerator.SimpleInjector.Interception
{
    internal class FuncInterceptedProxyBuilder : InterceptedProxyBuilder
    {
        private static readonly MethodInfo InvokeMethod = typeof(Func<IInterceptor>).GetTypeInfo().GetMethod(nameof(Func<IInterceptor>.Invoke));

        private readonly Type serviceType;
        private readonly Func<IInterceptor> interceptorFactory;

        public FuncInterceptedProxyBuilder(IProxyGenerator generator, Type serviceType, Func<IInterceptor> interceptorFactory)
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
            return this.serviceType == interfaceType;
        }
    }
}
