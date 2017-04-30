//using System;
//using System.Collections.Generic;
//using System.Text;
//using SimpleInjector;
//using System.Linq.Expressions;
//using System.Diagnostics;

//namespace MassiveDynamicProxyGenerator.SimpleInjector
//{
//    internal class InterceptionHelper
//    {
//        private readonly Container container;
//        private readonly ProxygGenerator generator;

//        public Func<ExpressionBuiltEventArgs, Expression> BuildInterceptorExpression
//        {
//            get;
//            set;
//        }

//        public Func<Type, bool> Predicate
//        {
//            get;
//            set;
//        }

//        public InterceptionHelper(Container container, ProxygGenerator generator)
//        {
//            if(container ==null)
//            {
//                throw new ArgumentNullException(nameof(container));
//            }

//            if (generator == null)
//            {
//                throw new ArgumentNullException(nameof(generator));
//            }

//            this.container = container;
//            this.generator = generator;

//            this.Predicate = t => true;
//        }

//        [DebuggerStepThrough]
//        public void OnExpressionBuilt(object sender, ExpressionBuiltEventArgs e)
//        {
//            if (this.Predicate(e.RegisteredServiceType))
//            {
//               // ThrowIfServiceTypeNotAnInterface(e);
//                e.Expression = this.BuildProxyExpression(e);
//            }
//        }

//        [DebuggerStepThrough]
//        private Expression BuildProxyExpression(ExpressionBuiltEventArgs e)
//        {
//            var interceptor = this.BuildInterceptorExpression(e);

//            //this.generator.GenerateDecorator()
//            // Create call to
//            // (ServiceType)Interceptor.CreateProxy(Type, IInterceptor, object)
//            // nahradit za
//            // (ServiceType)generator.
//            var proxyExpression =
//                Expression.Convert(
//                    Expression.Call(NonGenericInterceptorCreateProxyMethod,
//                        Expression.Constant(e.RegisteredServiceType, typeof(Type)),
//                        interceptor,
//                        e.Expression),
//                    e.RegisteredServiceType);

//            if (e.Expression is ConstantExpression && interceptor is ConstantExpression)
//            {
//                return Expression.Constant(CreateInstance(proxyExpression),
//                    e.RegisteredServiceType);
//            }

//            return proxyExpression;
//        }

//        [DebuggerStepThrough]
//        private static object CreateInstance(Expression expression)
//        {
//            var instanceCreator = Expression.Lambda<Func<object>>(expression,
//                new ParameterExpression[0])
//                .Compile();

//            return instanceCreator();
//        }
//    }
//}
