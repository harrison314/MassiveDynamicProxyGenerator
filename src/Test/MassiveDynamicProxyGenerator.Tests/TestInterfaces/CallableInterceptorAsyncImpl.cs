using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.Tests.TestInterfaces
{
    public class CallableInterceptorAsyncImpl : CallableInterceptorAsyncAdapter<object>
    {
        private readonly IMethodWraper wraper;

        public CallableInterceptorAsyncImpl(IMethodWraper wraper)
            :base()
        {
            if (wraper == null) throw new ArgumentNullException(nameof(wraper));

            this.wraper = wraper;
        }

        protected override bool HandleException(ICallableInvocation invocation, Exception ex, object invocationData)
        {
            return this.wraper.HandleException(invocation, ex, invocationData);
        }

        protected override object OnEnterInvoke(ICallableInvocation invocation)
        {
            return this.wraper.OnEnterInvoke(invocation);
        }

        protected override void OnExitInvoke(ICallableInvocation invocation, object invocationData)
        {
            this.wraper.OnExitInvoke(invocation, invocationData);
        }
    }
}
