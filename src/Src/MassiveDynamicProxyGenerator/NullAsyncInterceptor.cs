using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace MassiveDynamicProxyGenerator
{
#if !NET40

    /// <summary>
    /// Null interceptor usages in async await methods.
    /// Intercept method returns Tasks.
    /// This interceptor is preferred in the .Net Framework 4.5 and more and .Net Core.
    /// </summary>
    /// <seealso cref="MassiveDynamicProxyGenerator.IInterceptor" />
    public class NullAsyncInterceptor : IInterceptor
    {
        private static NullAsyncInterceptor instance;

        private readonly bool requeiredPostfix;

        /// <summary>
        /// Gets the singlton instance.
        /// </summary>
        /// <value>
        /// The singlton instance.
        /// </value>
        public static NullAsyncInterceptor Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// Initializes static members of the <see cref="NullAsyncInterceptor"/> class.
        /// </summary>
        static NullAsyncInterceptor()
        {
            instance = new NullAsyncInterceptor();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NullAsyncInterceptor"/> class.
        /// For detect async mehod used postfix &quot;Async&quot;.
        /// </summary>
        public NullAsyncInterceptor()
        {
            this.requeiredPostfix = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NullAsyncInterceptor"/> class.
        /// </summary>
        /// <param name="requeiredPostfix">if set to <c>true</c> rqeuire postfix &quot;Async&quot;.</param>
        public NullAsyncInterceptor(bool requeiredPostfix)
        {
            this.requeiredPostfix = requeiredPostfix;
        }

        /// <summary>
        /// Intercept call of method.
        /// </summary>
        /// <param name="invocation">Invocation informations.</param>
        public void Intercept(IInvocation invocation)
        {
            if ((!this.requeiredPostfix) || invocation.MethodName.EndsWith("Async", StringComparison.Ordinal))
            {
                if (invocation.ReturnType == typeof(Task))
                {
                    invocation.ReturnValue = Task.Delay(0);
                    return;
                }

                if (invocation.ReturnType.GetTypeInfo().IsGenericType
                    && invocation.ReturnType.GetTypeInfo().GetGenericTypeDefinition() == typeof(Task<>))
                {
                    Type[] genericTypes = invocation.ReturnType.GetTypeInfo().GenericTypeArguments;

                    invocation.ReturnValue = this.CreatetaskWithDefaultValue(genericTypes[0]);
                    return;
                }
            }
        }

        private object CreatetaskWithDefaultValue(Type type)
        {
            MethodInfo methodInfo = typeof(Task).GetTypeInfo().GetMethod(nameof(Task.FromResult), BindingFlags.Static | BindingFlags.Public);
            MethodInfo genericMethodInfo = methodInfo.MakeGenericMethod(type);

            Expression createTaskExpr = Expression.Call(genericMethodInfo, Expression.Default(type));
            Expression convertedTask = Expression.Convert(createTaskExpr, typeof(object));

            Expression<Func<object>> e = Expression.Lambda<Func<object>>(convertedTask);
            return e.Compile().Invoke();
        }
    }

#endif
}
