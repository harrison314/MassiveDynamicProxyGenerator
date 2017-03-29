using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator
{
    /// <summary>
    /// Adapter for <see cref="ICallableInterceptor"/> for wrap async result tasks.
    /// Usage for error handling eg. in WCF client.
    /// </summary>
    /// <typeparam name="T">Type of internal data created during invocation method.</typeparam>
    public abstract class CallableInterceptorAsyncAdapter<T> : ICallableInterceptor
            where T : class
    {
        private Dictionary<Type, Func<object, ICallableInvocation, T, object>> wraperCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="CallableInterceptorAsyncAdapter{T}"/> class.
        /// </summary>
        protected CallableInterceptorAsyncAdapter()
        {
            this.wraperCache = new Dictionary<Type, Func<object, ICallableInvocation, T, object>>();
            this.wraperCache.Add(typeof(Task), this.WrapTask);
        }

        /// <summary>
        /// Intercepts the specified invocation. And provide invoke methods <see cref="OnEnterInvoke(ICallableInvocation)"/>,
        /// <see cref="OnExitInvoke(ICallableInvocation, T)"/> and <see cref="HandleException(ICallableInvocation, Exception, T)"/>.
        /// </summary>
        /// <param name="invocation">The invocation informations.</param>
        public void Intercept(ICallableInvocation invocation)
        {
            CallableInterceptorAsyncInvocation m = new CallableInterceptorAsyncInvocation(invocation);
            T invocationData = this.OnEnterInvoke(m);
            try
            {
                m.Process();
                if (invocation.ReturnValue is Task)
                {
                    invocation.ReturnValue = this.Wrap(invocation.ReturnValue, m, invocationData);
                }
                else
                {
                    this.OnExitInvoke(m, invocationData);
                }
            }
            catch (Exception ex)
            {
                if (!this.HandleException(invocation, ex, invocationData))
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Called during interception method before invoke real method.
        /// </summary>
        /// <param name="invocation">The invocation informations.</param>
        /// <returns>Value providet to other method called during interception, default is default value of <typeparamref name="T"/>.</returns>
        protected virtual T OnEnterInvoke(ICallableInvocation invocation)
        {
            return default(T);
        }

        /// <summary>
        /// Called during interception method after succesfull invoke real method.
        /// </summary>
        /// <param name="invocation">The invocation informations.</param>
        /// <param name="invocationData">The internal user data.</param>
        protected virtual void OnExitInvoke(ICallableInvocation invocation, T invocationData)
        {
        }

        /// <summary>
        /// Called during interception method if real method throw exception.
        /// </summary>
        /// <param name="invocation">The invocation informations.</param>
        /// <param name="ex">The hrowed exception.</param>
        /// <param name="invocationData">The internal user data.</param>
        /// <returns>
        /// If return <c>true</c> then interception return default value,
        /// otherwise exception is rethrow.
        /// </returns>
        protected virtual bool HandleException(ICallableInvocation invocation, Exception ex, T invocationData)
        {
            return false;
        }

        private object Wrap(object returnValue, ICallableInvocation invocation, T invocationData)
        {
            Type taskType = returnValue.GetType();
            Func<object, ICallableInvocation, T, object> wraper;
            if (this.wraperCache.TryGetValue(taskType, out wraper))
            {
                return wraper.Invoke(returnValue, invocation, invocationData);
            }
            else
            {
                wraper = this.EmitGenericWraper(taskType);
                this.wraperCache[taskType] = wraper;

                return wraper.Invoke(returnValue, invocation, invocationData);
            }
        }

        private Func<object, ICallableInvocation, T, object> EmitGenericWraper(Type taskType)
        {
            ParameterExpression parameterTask = Expression.Parameter(typeof(object), "task");
            ParameterExpression parameterInvocation = Expression.Parameter(typeof(ICallableInvocation), "invocation");
            ParameterExpression parameterInvocationData = Expression.Parameter(typeof(T), "invocationData");

            MethodInfo mi = typeof(CallableInterceptorAsyncAdapter<T>).GetTypeInfo().GetMethod(nameof(this.WrapGenericTask), BindingFlags.NonPublic | BindingFlags.Instance);

            MethodInfo gmi = mi.MakeGenericMethod(taskType.GetTypeInfo().GetGenericArguments().First());

            Expression body = Expression.Call(Expression.Constant(this), gmi, parameterTask, parameterInvocation, parameterInvocationData);

            return Expression.Lambda<Func<object, ICallableInvocation, T, object>>(body, parameterTask, parameterInvocation, parameterInvocationData).Compile();
        }

        private object WrapTask(object task, ICallableInvocation invocation, T invocationData)
        {
            Task nonGenerictask = (Task)task;
            return nonGenerictask.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    if (!this.HandleException(invocation, t.Exception.InnerException, invocationData))
                    {
                        throw t.Exception.InnerException;
                    }
                }
                else
                {
                    this.OnExitInvoke(invocation, invocationData);
                }
            });
        }

        private object WrapGenericTask<TTask>(object task, ICallableInvocation invocation, T invocationData)
        {
            Task<TTask> nonGenerictask = (Task<TTask>)task;
            return nonGenerictask.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    if (!this.HandleException(invocation, t.Exception.InnerException, invocationData))
                    {
                        throw t.Exception.InnerException;
                    }

                    return default(TTask);
                }
                else
                {
                    this.OnExitInvoke(invocation, invocationData);
                    return t.Result;
                }
            });
        }
    }
}
