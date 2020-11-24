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
        protected static readonly MethodInfo GenerateInstanceProxyMethod = typeof(IProxyGenerator).GetTypeInfo()
                  .GetMethod(nameof(IProxyGenerator.GenerateInstanceProxy), new[] { typeof(Type), typeof(IInstanceProvicer) });

        private readonly Type instanceProviderType;
        private readonly IProxyGenerator generator;

        public InstanceProxyWithTypeRegistration(Lifestyle lifestyle, Container container, Type implementationType, Type instanceProviderType, IProxyGenerator generator)
            : base(lifestyle, container, implementationType, null)
        {
            this.instanceProviderType = instanceProviderType;
            this.generator = generator;
        }

        public override Expression BuildExpression()
        {
            InstanceProducer producer = this.Container.GetCurrentRegistrations().LastOrDefault(t => t.ServiceType == this.instanceProviderType);
            Expression interceptorSourse = (producer != null) ? producer.BuildExpression() : Expression.New(this.instanceProviderType);

            Expression generator = Expression.Constant(this.generator, typeof(IProxyGenerator));
            Expression typeOfInstance = Expression.Constant(this.ImplementationType, typeof(Type));
            Expression crateInstance = Expression.Call(generator, GenerateInstanceProxyMethod, typeOfInstance, interceptorSourse);

            return Expression.Convert(crateInstance, this.ImplementationType);
        }
    }
}
