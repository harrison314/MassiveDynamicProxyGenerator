using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Linq.Expressions;
using SimpleInjector;

namespace MassiveDynamicProxyGenerator.SimpleInjector.InstanceProxy
{
    internal class OpenInstanceProxyBuildProxy
    {
        private static readonly MethodInfo InstancePorxyMethod = typeof(IProxyGenerator).GetTypeInfo()
            .GetMethod(nameof(IProxyGenerator.GenerateInstanceProxy), new Type[] { typeof(Type), typeof(IInstanceProvicer), });

        private readonly IProxyGenerator generator;
        private readonly Type serviseType;
        private readonly Type instanceProducerType;

        public OpenInstanceProxyBuildProxy(IProxyGenerator generator, Type serviseType, Type instanceProducerType)
        {
            this.generator = generator;
            this.serviseType = serviseType;
            this.instanceProducerType = instanceProducerType;
        }

        public void ResolveUnregisteredType(object sender, UnregisteredTypeEventArgs unregistredTypeArgs)
        {
            if (this.CheckTypeToIntercept(unregistredTypeArgs.UnregisteredServiceType))
            {
                Type producerType = this.instanceProducerType.MakeGenericType(typeArguments: unregistredTypeArgs.UnregisteredServiceType.GetTypeInfo().GetGenericArguments());
                InstanceProducer producer = ((Container)sender).GetRegistration(producerType);

                Expression typeOfInstance = Expression.Constant(unregistredTypeArgs.UnregisteredServiceType, typeof(Type));
                Expression generator = Expression.Constant(this.generator, typeof(IProxyGenerator));
                Expression instanceProvider = Expression.Convert(producer.BuildExpression(), typeof(IInstanceProvicer));
                Expression crateInstance = Expression.Call(generator, InstancePorxyMethod, typeOfInstance, instanceProvider);

                unregistredTypeArgs.Register(Expression.Convert(crateInstance, unregistredTypeArgs.UnregisteredServiceType));
            }
        }

        protected bool CheckTypeToIntercept(Type interfaceType)
        {
            return interfaceType.GetTypeInfo().IsInterface && TypeHelper.IsGenericConstructedOf(this.serviseType, interfaceType);
        }
    }
}
