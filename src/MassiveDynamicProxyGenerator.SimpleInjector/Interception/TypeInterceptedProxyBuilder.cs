using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using SimpleInjector;

namespace MassiveDynamicProxyGenerator.SimpleInjector.Interception
{
    internal class TypeInterceptedProxyBuilder : InterceptedProxyBulder
    {
        private readonly Type serviseType;
        private readonly Type interceptorType;

        public TypeInterceptedProxyBuilder(ProxygGenerator generator, Type serviseType, Type interceptorType)
            : base(generator)
        {
            this.serviseType = serviseType;
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
            return this.serviseType == interfaceType;
        }
    }
}
