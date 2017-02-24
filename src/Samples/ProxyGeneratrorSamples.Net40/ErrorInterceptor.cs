using MassiveDynamicProxyGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxyGeneratrorSamples.Net40
{
    /// <summary>
    /// Interceptor for handling exceptions from normal and async methods.
    /// </summary>
    /// <seealso cref="MassiveDynamicProxyGenerator.ICallableInterceptor" />
    public abstract class ErrorInterceptor : ICallableInterceptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorInterceptor"/> class.
        /// </summary>
        public ErrorInterceptor()
        {

        }

        /// <summary>
        /// Intercepts the specified invocation.
        /// </summary>
        /// <param name="invocation">The invocation informations.</param>
        public void Intercept(ICallableInvocation invocation)
        {
            try
            {
                invocation.Process();
                if (invocation.ReturnValue is Task)
                {
                    invocation.ReturnValue = ((Task)invocation.ReturnValue).ContinueWith(task =>
                      {
                          if (task.IsFaulted)
                          {
                              Exception agregateException = task.Exception;
                              if (agregateException.InnerException != null)
                              {
                                  agregateException = agregateException.InnerException;
                              }

                              this.HandleException(agregateException);
                          }
                      });
                }
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }
        }

        /// <summary>
        /// Handles the exception throws during call method or execute async <see cref="Task"/>.
        /// </summary>
        /// <param name="ex">The handled <see cref="Exception"/>.</param>
        protected abstract void HandleException(Exception ex);
    }
}
