using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Linq.Expressions;
using SimpleInjector;

namespace MassiveDynamicProxyGenerator.SimpleInjector.InstanceProxy
{
    //internal class OpenInstanceProxyBuildProxy
    //{
    //    private readonly IProxygGenerator generator;
    //    private readonly Type serviseType;

    //    public OpenInstanceProxyBuildProxy()
    //    {

    //    }

    //    public void ResolveUnregisteredType(object sender, UnregisteredTypeEventArgs unregistredTypeArgs)
    //    {
    //        if (unregistredTypeArgs.UnregisteredServiceType.GetTypeInfo().IsInterface && this.CheckTypeToIntercept(unregistredTypeArgs.UnregisteredServiceType))
    //        {
    //            Expression generator = Expression.Constant(this.generator, typeof(ProxygGenerator));
    //            Expression interceptor = this.BuildInterceptionExpression((Container)sender, unregistredTypeArgs.UnregisteredServiceType);
    //            Expression typeOfInstance = Expression.Constant(unregistredTypeArgs.UnregisteredServiceType, typeof(Type));
    //            Expression crateInstance = Expression.Call(generator, GenerateProxyMethod, typeOfInstance, interceptor);

    //            unregistredTypeArgs.Register(Expression.Convert(crateInstance, unregistredTypeArgs.UnregisteredServiceType));
    //        }
    //    }

    //    protected bool CheckTypeToIntercept(Type interfaceType)
    //    {
    //        return TypeHelper.IsGenericConstructedOf(this.serviseType, interfaceType);
    //    }
    //}
}
