using MassiveDynamicProxyGenerator.Tests.TestInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.Tests.Examples
{
    internal class ReturnTypes : IReturnTypes
    {
        private IInterceptor interceptor;
        private IReturnTypes parent;

        public ReturnTypes(IInterceptor interceptorParam)
        {
            this.interceptor = interceptorParam;
            this.parent = null;
        }

        public StringBuilder CreateSb(string arg)
        {
            InvocationExample invocation = new InvocationExample();
            invocation.Arguments = new object[]
            {
                arg
            };

            invocation.ReturnValue = null;
            invocation.MethodName = "CreateSb";
            invocation.OriginalType = typeof(IReturnTypes);
            invocation.ArgumentTypes = new Type[]
            {
                typeof(string),
            };

            this.interceptor.Intercept(invocation);

            return (StringBuilder)invocation.ReturnValue;
        }


        public int GetLenght(string arg)
        {
            InvocationExample invocation = new InvocationExample();
            invocation.Arguments = new object[]
            {
                arg
            };

            invocation.ReturnValue = null;
            invocation.MethodName = "GetLenght";
            invocation.OriginalType = typeof(IReturnTypes);
            invocation.ArgumentTypes = new Type[]
            {
                typeof(string),
            };

            this.interceptor.Intercept(invocation);

            return (int)invocation.ReturnValue;
        }

        public void GetVoid()
        {
            InvocationExample invocation = new InvocationExample();
            invocation.Arguments = new object[]
            {
            };

            invocation.ReturnValue = null;
            invocation.MethodName = "GetVoid";
            invocation.OriginalType = typeof(IReturnTypes);
            invocation.ArgumentTypes = new Type[]
            {
            };

            this.interceptor.Intercept(invocation);
        }

        public string AnyStr()
        {
            return "amber";
        }

        public MyStruct GetStruct()
        {
            throw new NotImplementedException();
        }

        public int PulseEx(int a, string message, Exception ex)
        {
            return 45;
        }

        public void Pulse()
        {
            Caller caller = new Caller(this.PulseInternal);

            caller.Call();
        }

        private void PulseInternal(IInvocation actt)
        {
            if (actt == null) throw new ArgumentNullException(nameof(actt));

            object[] tmp = actt.Arguments;
            actt .ReturnValue = this.parent.PulseEx((int)tmp[0],
                (string)tmp[1],
                (Exception)tmp[2]);
        }

        private void PulseInternal2(IInvocation actt)
        {

            object[] tmp = actt.Arguments;
            this.parent.GetVoid();
        }
    }
}
