using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MassiveDynamicProxyGenerator
{
    internal class CallableInterceptorAsyncInvocation : ICallableInvocation
    {
        private readonly ICallableInvocation parent;
        private bool superssProcess;

        public object ReturnValue
        {
            get
            {
                return this.parent.ReturnValue;
            }

            set
            {
                this.superssProcess = true;
                this.parent.ReturnValue = value;
            }
        }

        public object[] Arguments
        {
            get
            {
                return this.parent.Arguments;
            }
        }

        public Type OriginalType
        {
            get
            {
                return this.parent.OriginalType;
            }
        }

        public string MethodName
        {
            get
            {
                return this.parent.MethodName;
            }
        }

        public Type[] ArgumentTypes
        {
            get
            {
                return this.parent.ArgumentTypes;
            }
        }

        public Type ReturnType
        {
            get
            {
                return this.parent.ReturnType;
            }
        }

        public CallableInterceptorAsyncInvocation(ICallableInvocation parent)
        {
            this.parent = parent;
            this.superssProcess = false;
        }

        public MethodBase GetConcreteMethod()
        {
            return this.parent.GetConcreteMethod();
        }

        public void Process()
        {
            if (!this.superssProcess)
            {
                this.parent.Process();
            }
        }
    }
}