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
        protected static readonly MethodInfo GenerateProxyMethod = typeof(IProxygGenerator).GetTypeInfo()
                   .GetMethod(nameof(IProxygGenerator.GenerateProxy), new[] { typeof(Type), typeof(IInterceptor) });

        private readonly Type implementationType;
        private readonly Type interceptorType;
        private readonly ProxygGenerator generator;

        public ProxyWithTypeInterceptorRegistration(Lifestyle lifestyle, Container container, Type implementationType, Type interceptorType, ProxygGenerator generator)
            : base(lifestyle, container)
        {
            this.implementationType = implementationType;
            this.interceptorType = interceptorType;

            this.generator = generator;
        }

        public override Type ImplementationType
        {
            get
            {
                return this.implementationType;
            }
        }

        public override Expression BuildExpression()
        {
            InstanceProducer producer = this.Container.GetRegistration(this.interceptorType, false);
            Expression interceptorSourse = (producer != null) ? producer.BuildExpression() : Expression.New(this.interceptorType);

            Expression generator = Expression.Constant(this.generator, typeof(ProxygGenerator));
            Expression typeOfInstance = Expression.Constant(this.implementationType, typeof(Type));
            Expression crateInstance = Expression.Call(generator, GenerateProxyMethod, typeOfInstance, interceptorSourse);

           return Expression.Convert(crateInstance, this.implementationType);
        }
    }
}
