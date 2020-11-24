using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using SimpleInjector;

namespace MassiveDynamicProxyGenerator.SimpleInjector.Registrations
{
    internal class ProxyWithInstanceInterceptorRegistration : Registration
    {
        protected static readonly MethodInfo GenerateProxyMethod = typeof(IProxyGenerator).GetTypeInfo()
                  .GetMethod(nameof(IProxyGenerator.GenerateProxy), new[] { typeof(Type), typeof(IInterceptor) });

        private readonly IInterceptor instance;
        private readonly IProxyGenerator generator;

        public ProxyWithInstanceInterceptorRegistration(Lifestyle lifestyle, Container container, Type implementationType, IInterceptor instance, IProxyGenerator generator)
            : base(lifestyle, container, implementationType)
        {
            this.instance = instance;

            this.generator = generator;
        }

        public override Expression BuildExpression()
        {
            Expression interceptorSourse = Expression.Constant(this.instance, typeof(IInterceptor));

            Expression generator = Expression.Constant(this.generator, typeof(IProxyGenerator));
            Expression typeOfInstance = Expression.Constant(this.ImplementationType, typeof(Type));
            Expression crateInstance = Expression.Call(generator, GenerateProxyMethod, typeOfInstance, interceptorSourse);

            return Expression.Convert(crateInstance, this.ImplementationType);
        }
    }
}
