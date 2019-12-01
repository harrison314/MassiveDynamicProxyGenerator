using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PerformaceExamples
{
    public class RemoteCallDispatchProxy : DispatchProxy
    {
        private IRemoteCall remoteCall;

        public RemoteCallDispatchProxy()
        {
            this.remoteCall = null;
        }

        public static T Create<T>(IRemoteCall remoteCall) where T : class
        {
            T proxy = RemoteCallDispatchProxy.Create<T, RemoteCallDispatchProxy>();
            (proxy as RemoteCallDispatchProxy).remoteCall = remoteCall;

            return proxy;
        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            return this.remoteCall.Call(targetMethod.Name, targetMethod.ReturnType, args);
        }
    }
}
