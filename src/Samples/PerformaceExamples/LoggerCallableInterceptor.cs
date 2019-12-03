using MassiveDynamicProxyGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformaceExamples
{
    public class LoggerCallableInterceptor : ICallableInterceptor
    {
        private readonly IRemoteCall remoteCall;

        public LoggerCallableInterceptor(IRemoteCall remoteCall)
        {
            this.remoteCall = remoteCall;
        }

        public void Intercept(ICallableInvocation invocation)
        {
            this.remoteCall.Call(invocation.MethodName, invocation.ReturnType, invocation.Arguments);
            invocation.Process();
        }
    }
}
