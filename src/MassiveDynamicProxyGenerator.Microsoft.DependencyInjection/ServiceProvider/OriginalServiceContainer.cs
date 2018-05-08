using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace MassiveDynamicProxyGenerator.Microsoft.DependencyInjection.ServiceProvider
{
    internal class OriginalServiceContainer<T> : IOriginalService<T> where T : class
    {
        public T ServiceInstance
        {
            get;
            protected set;
        }

        public OriginalServiceContainer(T instance)
        {
            this.ServiceInstance = instance;
        }

        public static object BuildOriginalService(Type genericType, object serviceInstance)
        {
            Type originalType = typeof(OriginalServiceContainer<>).MakeGenericType(genericType);
            ConstructorInfo constructor = originalType.GetConstructor(new Type[] { genericType });

            Expression instance = Expression.Constant(serviceInstance, genericType);
            Expression newExpr = Expression.New(constructor, instance);

            return Expression.Lambda<Func<object>>(newExpr).Compile().Invoke();
        }
    }
}
