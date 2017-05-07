using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using SimpleInjector;

namespace MassiveDynamicProxyGenerator.SimpleInjector.Interception
{
    internal class OpenInstanceInterceptedProxyBuilder : InterceptedProxyBulder
    {
        private readonly Type serviseType;
        private readonly IInterceptor interceptor;

        public OpenInstanceInterceptedProxyBuilder(ProxygGenerator generator, Type serviseType, IInterceptor interceptor)
            : base(generator)
        {
            this.serviseType = serviseType;
            this.interceptor = interceptor;
        }

        protected override Expression BuildInterceptionExpression(Container container, Type typeToIntercept)
        {
            return Expression.Constant(this.interceptor, typeof(IInterceptor));
        }

        protected override bool CheckTypeToIntercept(Type interfaceType)
        {
            return TypeHelper.IsGenericConstructedOf(this.serviseType, interfaceType);
        }
    }
}
