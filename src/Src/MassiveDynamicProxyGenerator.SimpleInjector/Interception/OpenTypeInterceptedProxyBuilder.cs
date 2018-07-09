using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using SimpleInjector;

namespace MassiveDynamicProxyGenerator.SimpleInjector.Interception
{
    internal class OpenTypeInterceptedProxyBuilder : InterceptedProxyBuilder
    {
        private readonly Type serviceType;
        private readonly Type interceptorType;

        public OpenTypeInterceptedProxyBuilder(IProxyGenerator generator, Type serviceType, Type interceptorType)
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
            return TypeHelper.IsGenericConstructedOf(this.serviceType, interfaceType);
        }
    }
}
