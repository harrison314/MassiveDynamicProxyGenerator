using System;
using System.Linq.Expressions;
using System.Reflection;
using SimpleInjector;

namespace MassiveDynamicProxyGenerator.SimpleInjector.Interception
{
    internal abstract class InterceptionBuilder
    {
        public Expression GeneratorSourse
        {
            get;
            protected set;
        }

        public InterceptionBuilder(ProxygGenerator generator)
        {
            this.GeneratorSourse = Expression.Constant(generator, typeof(ProxygGenerator));
        }

        public void ReguildExpresion(object sender, ExpressionBuiltEventArgs buildArgs)
        {
            if (buildArgs.RegisteredServiceType.GetTypeInfo().IsInterface && this.ChecktypeToIntercept(buildArgs.RegisteredServiceType))
            {
                Container container = (Container)sender;
                Expression interceptorSourse = this.BuildInterceptionExpression(container, buildArgs.RegisteredServiceType);

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

        protected abstract Expression BuildInterceptionExpression(Container container, Type typeToIntercept);

        protected virtual bool ChecktypeToIntercept(Type typeToIntercept)
        {
            return true;
        }
    }
}
