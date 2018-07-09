using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace MassiveDynamicProxyGenerator.Utils
{
    /// <summary>
    /// Settings utils.
    /// </summary>
    internal static class SettingsUtils
    {
        /// <summary>
        /// Applies the specified action to settings instance.
        /// </summary>
        /// <typeparam name="T">Type of settings.</typeparam>
        /// <param name="action">The action.</param>
        /// <returns>The settings.</returns>
        public static T Apply<T>(Action<T> action)
            where T : new()
        {
            T instance = Expression.Lambda<Func<T>>(Expression.New(typeof(T))).Compile().Invoke();
            action.Invoke(instance);

            return instance;
        }

        /// <summary>
        /// Applies the specified action to settings instance.
        /// </summary>
        /// <typeparam name="T">Type of settings.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="instance">The default instance.</param>
        /// <returns>
        /// The settings.
        /// </returns>
        public static T Apply<T>(Action<T> action, T instance)
        {
            action.Invoke(instance);

            return instance;
        }
    }
}
