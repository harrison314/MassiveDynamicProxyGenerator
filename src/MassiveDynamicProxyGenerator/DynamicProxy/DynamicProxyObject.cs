using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.DynamicProxy
{
    public class DynamicProxyObject : DynamicObject
    {
        private readonly IInterceptor interceptor;

        public DynamicProxyObject(IInterceptor interceptor)
        {
            if (interceptor == null)
            {
                throw new ArgumentNullException(nameof(interceptor));
            }

            this.interceptor = interceptor;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            DynamicInvocation invocation = new DynamicInvocation();
            invocation.Arguments = args;
            invocation.MethodName = binder.Name;
            invocation.ReturnType = binder.ReturnType;

            this.interceptor.Intercept(invocation, true);

            result = invocation.ReturnValue;

            return true;
        }
    }
}
