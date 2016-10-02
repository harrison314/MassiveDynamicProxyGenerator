using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.TypedDecorator
{
    internal class CallableInterceptorDescriptor
    {
        public Type Type
        {
            get;
            protected set;
        }

        public MethodInfo Intercept
        {
            get;
            protected set;
        }

        public CallableInterceptorDescriptor()
        {
            this.Type = typeof(ICallableInterceptor);
            this.Intercept = this.Type.GetMethod(nameof(ICallableInterceptor.Intercept), new Type[] { typeof(ICallableInvocation) });
        }
    }
}
