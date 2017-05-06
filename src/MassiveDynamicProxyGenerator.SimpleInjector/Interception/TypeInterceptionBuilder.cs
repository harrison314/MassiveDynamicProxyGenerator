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

        public TypeInterceptionBuilder(Predicate<Type> predicate, ProxygGenerator generator, Type interceptorType)
            : base(predicate, generator)
        {
            if (interceptorType == null)
            {
                throw new ArgumentNullException(nameof(interceptorType));
            }

            this.interceptorType = interceptorType;
            this.predicate = predicate;
        }

        protected override Expression BuildInterceptionExpression(Container container, Type typeToIntercept)
        {
            InstanceProducer producer = container.GetRegistration(this.interceptorType, false);
            Expression interceptorSourse = (producer != null) ? producer.BuildExpression() : Expression.New(this.interceptorType);

            return interceptorSourse;
        }

        protected override bool ChecktypeToIntercept(Type typeToIntercept)
        {
            return this.predicate(typeToIntercept)  && this.interceptorType != typeToIntercept;
        }
    }
}
