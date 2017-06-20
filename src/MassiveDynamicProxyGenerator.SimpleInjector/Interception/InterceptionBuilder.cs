using System;
using System.Linq.Expressions;
using System.Reflection;
using SimpleInjector;

namespace MassiveDynamicProxyGenerator.SimpleInjector.Interception
{
    /// <summary>
    /// Builder for rebuild expression in SimpleInjector using for interception.
    /// </summary>
    internal abstract class InterceptionBuilder
    {
        protected static readonly MethodInfo CrateInterceptorMethod = typeof(IProxygGenerator).GetTypeInfo()
                    .GetMethod(nameof(IProxygGenerator.GenerateDecorator), new[] { typeof(Type), typeof(ICallableInterceptor), typeof(object) });

        public Expression GeneratorSourse
        {
            get;
            protected set;
        }

        public InterceptionBuilder(IProxygGenerator generator)
        {
            this.GeneratorSourse = Expression.Constant(generator, typeof(IProxygGenerator));
        }

        public void ReguildExpresion(object sender, ExpressionBuiltEventArgs buildArgs)
        {
            if (buildArgs.RegisteredServiceType.GetTypeInfo().IsInterface && this.CheckTypeToIntercept(buildArgs.RegisteredServiceType))
            {
                Container container = (Container)sender;
                Expression interceptorSourse = this.BuildInterceptionExpression(container, buildArgs.RegisteredServiceType);

                Expression decoratorType = Expression.Constant(buildArgs.RegisteredServiceType, typeof(Type));

                MethodCallExpression call = Expression.Call(this.GeneratorSourse,
                    CrateInterceptorMethod,
                    decoratorType,
                    interceptorSourse,
                    buildArgs.Expression);

                Expression craetorExpression = Expression.ConvertChecked(call, buildArgs.RegisteredServiceType);

                buildArgs.Expression = craetorExpression;
            }
        }

        /// <summary>
        /// Builds the expression generated <see cref="ICallableInterceptor"/>.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="typeToIntercept">The type to intercept.</param>
        /// <returns>Expression generated <see cref="ICallableInterceptor"/></returns>
        protected abstract Expression BuildInterceptionExpression(Container container, Type typeToIntercept);

        /// <summary>
        /// Check type to intercept. If typeToIntercept by inteception then return true, otherwise false.
        /// </summary>
        /// <param name="typeToIntercept">The type to intercept.</param>
        /// <returns>If typeToIntercept by inteception then return true, otherwise false.</returns>
        protected virtual bool CheckTypeToIntercept(Type typeToIntercept)
        {
            return true;
        }
    }
}
