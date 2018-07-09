using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using SimpleInjector;

namespace MassiveDynamicProxyGenerator.SimpleInjector.Interception
{
    internal class InstanceInterceptedProxyBuilder : InterceptedProxyBuilder
    {
        private readonly Type serviceType;
        private readonly IInterceptor interceptor;

        public InstanceInterceptedProxyBuilder(IProxyGenerator generator, Type serviceType, IInterceptor interceptor)
            : base(generator)
        {
            this.serviceType = serviceType;
            this.interceptor = interceptor;
        }

        protected override Expression BuildInterceptionExpression(Container container, Type typeToIntercept)
        {
            return Expression.Constant(this.interceptor, typeof(IInterceptor));
        }

        protected override bool CheckTypeToIntercept(Type interfaceType)
        {
            return this.serviceType == interfaceType;
        }
    }
}
