using MassiveDynamicProxyGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformaceExamples
{
    public class RemoteCallInterceptor : IInterceptor
    {
        private readonly IRemoteCall remoteCall;

        public RemoteCallInterceptor(IRemoteCall remoteCall)
        {
            this.remoteCall = remoteCall ?? throw new ArgumentNullException(nameof(remoteCall));
        }

        public void Intercept(IInvocation invocation)
        {
            invocation.ReturnValue = this.remoteCall.Call(invocation.MethodName, invocation.ReturnType, invocation.Arguments);
        }
    }
}
