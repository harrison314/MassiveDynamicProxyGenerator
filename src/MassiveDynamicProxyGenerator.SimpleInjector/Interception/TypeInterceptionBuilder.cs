using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using SimpleInjector;

namespace MassiveDynamicProxyGenerator.SimpleInjector.Interception
{
    internal class TypeInterceptionBuilder : InterceptionBuilder
    {
        private readonly Predicate<Type> predicate;
        private readonly Type interceptorType;

        public TypeInterceptionBuilder(Predicate<Type> predicate, IProxyGenerator generator, Type interceptorType)
            : base(generator)
        {
            this.interceptorType = interceptorType;
            this.predicate = predicate;
        }

        protected override Expression BuildInterceptionExpression(Container container, Type typeToIntercept)
        {
            InstanceProducer producer = container.GetRegistration(this.interceptorType, false);
            Expression interceptorSourse = (producer != null) ? producer.BuildExpression() : Expression.New(this.interceptorType);

            return interceptorSourse;
        }

        protected override bool CheckTypeToIntercept(Type typeToIntercept)
        {
            return this.predicate(typeToIntercept) && this.interceptorType != typeToIntercept;
        }
    }
}
