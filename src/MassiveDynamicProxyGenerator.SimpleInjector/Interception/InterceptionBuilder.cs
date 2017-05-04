using System;
using System.Linq.Expressions;
using System.Reflection;
using SimpleInjector;

namespace MassiveDynamicProxyGenerator.SimpleInjector.Interception
{
    internal class InterceptionBuilder
    {
        private readonly Predicate<Type> predicate;
        private readonly Type interceptorType;

        public Expression GeneratorSourse
        {
            get;
            set;
        }

        public InterceptionBuilder(Predicate<Type> predicate, ProxygGenerator generator, Type interceptorType)
        {
            this.predicate = predicate;

            this.GeneratorSourse = Expression.Constant(generator, typeof(ProxygGenerator));
            this.interceptorType = interceptorType;
        }

        public void ReguildExpresion(object sender, ExpressionBuiltEventArgs buildArgs)
        {
            if (this.predicate(buildArgs.RegisteredServiceType) && buildArgs.RegisteredServiceType.GetTypeInfo().IsInterface && buildArgs.RegisteredServiceType!= this.interceptorType)
            {

                Container container = (Container)sender;
                InstanceProducer producer = container.GetRegistration(this.interceptorType, false);
                Expression interceptorSourse = (producer != null) ? producer.BuildExpression() : Expression.New(this.interceptorType);


                System.Reflection.MethodInfo crateInterceptor = typeof(IProxygGenerator).GetTypeInfo()
                    .GetMethod(nameof(IProxygGenerator.GenerateDecorator), new[] { typeof(Type), typeof(ICallableInterceptor), typeof(object) });

                Expression decoratorType = Expression.Constant(buildArgs.RegisteredServiceType, typeof(Type));

                MethodCallExpression call = Expression.Call(this.GeneratorSourse,
                    crateInterceptor,
                    decoratorType,
                    interceptorSourse,
                    buildArgs.Expression);

                Expression craetorExpression = Expression.ConvertChecked(call, buildArgs.RegisteredServiceType);

                buildArgs.Expression = craetorExpression;
            }
        }
    }
}
