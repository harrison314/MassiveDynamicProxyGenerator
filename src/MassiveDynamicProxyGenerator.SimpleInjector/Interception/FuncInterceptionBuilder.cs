using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;
using SimpleInjector;

namespace MassiveDynamicProxyGenerator.SimpleInjector.Interception
{
    internal class FuncInterceptionBuilder : InterceptionBuilder
    {
        private static readonly MethodInfo InvokeMethod = typeof(Func<ICallableInterceptor>).GetTypeInfo().GetMethod(nameof(Func<ICallableInterceptor>.Invoke));

        private readonly Predicate<Type> predicate;
        private readonly Func<ICallableInterceptor> interceptorFactory;

        public FuncInterceptionBuilder(IProxygGenerator generator, Predicate<Type> predicate, Func<ICallableInterceptor> interceptorFactory)
            : base(generator)
        {
            this.predicate = predicate;
            this.interceptorFactory = interceptorFactory;
        }

        protected override Expression BuildInterceptionExpression(Container container, Type typeToIntercept)
        {
            return Expression.Call(Expression.Constant(this.interceptorFactory, typeof(Func<ICallableInterceptor>)), InvokeMethod);
        }

        protected override bool CheckTypeToIntercept(Type typeToIntercept)
        {
            return this.predicate(typeToIntercept);
        }
    }
}
