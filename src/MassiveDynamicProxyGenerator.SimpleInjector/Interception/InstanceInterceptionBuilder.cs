using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using SimpleInjector;

namespace MassiveDynamicProxyGenerator.SimpleInjector.Interception
{
    internal class InstanceInterceptionBuilder : InterceptionBuilder
    {
        private readonly Predicate<Type> predicate;
        private readonly ICallableInterceptor interceptor;

        public InstanceInterceptionBuilder(Predicate<Type> predicate, IProxyGenerator generator, ICallableInterceptor interceptor)
            : base(generator)
        {
            this.interceptor = interceptor;
            this.predicate = predicate;
        }

        protected override Expression BuildInterceptionExpression(Container container, Type typeToIntercept)
        {
            return Expression.Constant(this.interceptor, typeof(ICallableInterceptor));
        }

        protected override bool CheckTypeToIntercept(Type typeToIntercept)
        {
            return this.predicate(typeToIntercept);
        }
    }
}
