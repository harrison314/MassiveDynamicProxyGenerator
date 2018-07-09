using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.Tests.TestInterfaces
{
    public interface IMethodWraper
    {
        bool HandleException(ICallableInvocation invocation, Exception ex, object invocationData);

        object OnEnterInvoke(ICallableInvocation invocation);

        void OnExitInvoke(ICallableInvocation invocation, object invocationData);
    }
}
