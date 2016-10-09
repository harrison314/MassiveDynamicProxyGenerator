using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace MassiveDynamicProxyGenerator.Utils
{
    internal static class SettingsUtils
    {
        public static T Apply<T>(Action<T> action)
            where T : new()
        {
            T instance = Expression.Lambda<Func<T>>(Expression.New(typeof(T))).Compile().Invoke();
            action.Invoke(instance);

            return instance;
        }

        public static T Apply<T>(Action<T> action, T instance)
        {
            action.Invoke(instance);

            return instance;
        }
    }
}
