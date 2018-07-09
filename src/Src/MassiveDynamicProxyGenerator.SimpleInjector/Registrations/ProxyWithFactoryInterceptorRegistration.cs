using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using SimpleInjector;

namespace MassiveDynamicProxyGenerator.SimpleInjector.Registrations
{
    internal class ProxyWithFactoryInterceptorRegistration : Registration
    {
        protected static readonly MethodInfo GenerateProxyMethod = typeof(IProxyGenerator).GetTypeInfo()
                  .GetMethod(nameof(IProxyGenerator.GenerateProxy), new[] { typeof(Type), typeof(IInterceptor) });

        private readonly Type implementationType;
        private readonly Func<IInterceptor> factory;
        private readonly IProxyGenerator generator;

        public ProxyWithFactoryInterceptorRegistration(Lifestyle lifestyle, Container container, Type implementationType, Func<IInterceptor> factory, IProxyGenerator generator)
            : base(lifestyle, container)
        {
            this.implementationType = implementationType;
            this.factory = factory;

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
            Expression interceptorSourse = Expression.Invoke(Expression.Constant(this.factory, typeof(Func<IInterceptor>)));

            Expression generator = Expression.Constant(this.generator, typeof(IProxyGenerator));
            Expression typeOfInstance = Expression.Constant(this.implementationType, typeof(Type));
            Expression crateInstance = Expression.Call(generator, GenerateProxyMethod, typeOfInstance, interceptorSourse);

            return Expression.Convert(crateInstance, this.implementationType);
        }
    }
}
