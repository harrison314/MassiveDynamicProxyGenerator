using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using SimpleInjector;

namespace MassiveDynamicProxyGenerator.SimpleInjector.Interception
{
    internal class TypeInterceptedProxyBuilder : InterceptedProxyBuilder
    {
        private readonly Type serviceType;
        private readonly Type interceptorType;

        public TypeInterceptedProxyBuilder(IProxyGenerator generator, Type serviceType, Type interceptorType)
            : base(generator)
        {
            this.serviceType = serviceType;
            this.interceptorType = interceptorType;
        }

        protected override Expression BuildInterceptionExpression(Container container, Type typeToIntercept)
        {
            InstanceProducer producer = container.GetRegistration(this.interceptorType, false);
            Expression interceptorSourse = (producer != null) ? producer.BuildExpression() : Expression.New(this.interceptorType);

            return interceptorSourse;
        }

        protected override bool CheckTypeToIntercept(Type interfaceType)
        {
            return this.serviceType == interfaceType;
        }
    }
}
