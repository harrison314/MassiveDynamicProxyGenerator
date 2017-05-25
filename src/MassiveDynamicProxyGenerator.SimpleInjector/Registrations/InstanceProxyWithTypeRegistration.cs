using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Reflection;
using System.Text;
using SimpleInjector;

namespace MassiveDynamicProxyGenerator.SimpleInjector.Registrations
{
    internal class InstanceProxyWithTypeRegistration : Registration
    {
        protected static readonly MethodInfo GenerateInstanceProxyMethod = typeof(IProxygGenerator).GetTypeInfo()
                  .GetMethod(nameof(IProxygGenerator.GenerateInstanceProxy), new[] { typeof(Type), typeof(IInstanceProvicer) });

        private readonly Type implementationType;
        private readonly Type instanceProviderType;
        private readonly ProxygGenerator generator;

        public InstanceProxyWithTypeRegistration(Lifestyle lifestyle, Container container, Type implementationType, Type instanceProviderType, ProxygGenerator generator)
            : base(lifestyle, container)
        {
            this.implementationType = implementationType;
            this.instanceProviderType = instanceProviderType;

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
            InstanceProducer producer = this.Container.GetCurrentRegistrations().LastOrDefault(t=>t.ServiceType == this.instanceProviderType);
            Expression interceptorSourse = (producer != null) ? producer.BuildExpression() : Expression.New(this.instanceProviderType);

            Expression generator = Expression.Constant(this.generator, typeof(ProxygGenerator));
            Expression typeOfInstance = Expression.Constant(this.implementationType, typeof(Type));
            Expression crateInstance = Expression.Call(generator, GenerateInstanceProxyMethod, typeOfInstance, interceptorSourse);

            return Expression.Convert(crateInstance, this.implementationType);
        }
    }
}
