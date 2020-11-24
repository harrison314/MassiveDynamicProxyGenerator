using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using SimpleInjector;

namespace MassiveDynamicProxyGenerator.SimpleInjector.Registrations
{
    internal class ProxyWithTypeInterceptorRegistration : Registration
    {
        protected static readonly MethodInfo GenerateProxyMethod = typeof(IProxyGenerator).GetTypeInfo()
                   .GetMethod(nameof(IProxyGenerator.GenerateProxy), new[] { typeof(Type), typeof(IInterceptor) });

        private readonly Type interceptorType;
        private readonly IProxyGenerator generator;

        public ProxyWithTypeInterceptorRegistration(Lifestyle lifestyle, Container container, Type implementationType, Type interceptorType, IProxyGenerator generator)
            : base(lifestyle, container, implementationType)
        {
            this.interceptorType = interceptorType;

            this.generator = generator;
        }

        public override Expression BuildExpression()
        {
            InstanceProducer producer = this.Container.GetRegistration(this.interceptorType, false);
            Expression interceptorSourse = (producer != null) ? producer.BuildExpression() : Expression.New(this.interceptorType);

            Expression generator = Expression.Constant(this.generator, typeof(IProxyGenerator));
            Expression typeOfInstance = Expression.Constant(this.ImplementationType, typeof(Type));
            Expression crateInstance = Expression.Call(generator, GenerateProxyMethod, typeOfInstance, interceptorSourse);

           return Expression.Convert(crateInstance, this.ImplementationType);
        }
    }
}
