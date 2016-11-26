using MassiveDynamicProxyGenerator;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;

namespace SampleWebApplication.Services.Interceptors
{
    public class PerformaceInterceptor : ICallableInterceptor
    {
        private readonly ILogger<PerformaceInterceptor> logger;

        public PerformaceInterceptor(ILogger<PerformaceInterceptor> logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            this.logger = logger;
        }

        public void Intercept(ICallableInvocation invocation)
        {
            this.logger.LogTrace("Call method {0}.{1}", invocation.OriginalType.Name, invocation.MethodName);

            bool isAsyncCall = this.IsAsyncOperation(invocation);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            invocation.Process();
            if (isAsyncCall)
            {
                Task returnTask = (Task)invocation.ReturnValue;
                returnTask.ContinueWith(_ =>
                {
                    stopwatch.Stop();
                    this.logger.LogTrace("Method {0}.{1} spend {2} ms.", invocation.OriginalType.Name, invocation.MethodName, stopwatch.ElapsedMilliseconds);
                });
            }
            else
            {
                stopwatch.Stop();
                this.logger.LogTrace("Method {0}.{1} spend {2} ms.", invocation.OriginalType.Name, invocation.MethodName, stopwatch.ElapsedMilliseconds);
            }
        }

        private bool IsAsyncOperation(ICallableInvocation invocation)
        {
            if (!invocation.MethodName.EndsWith("Async", StringComparison.Ordinal))
            {
                return false;
            }

            if (invocation.ReturnType == typeof(void) || invocation.ReturnType == typeof(Task))
            {
                return true;
            }

            if (invocation.ReturnType.GetTypeInfo().IsGenericType && invocation.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                return true;
            }

            return false;
        }
    }
}
