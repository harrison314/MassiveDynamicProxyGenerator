using MassiveDynamicProxyGenerator.Tests.TestInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.Tests.Examples
{
    internal class NonReturnExample : INonReturn
    {
        private IInterceptor interceptor;

        public NonReturnExample(IInterceptor interceptorParam)
        {
            this.interceptor = interceptorParam; 
        }
        public void EmptyMethod()
        {
            InvocationExample invocation = new InvocationExample();
            invocation.Arguments = new object[0];
            invocation.ReturnValue = null;
            invocation.MethodName = "EmptyMethod";
            invocation.OriginalType = typeof(INonReturn);

            this.interceptor.Intercept(invocation, false);
        }

        public void MoreArguments(string a, Exception parentEx, StringBuilder builder, Func<Exception, string> transformation)
        {
            InvocationExample invocation = new InvocationExample();
            invocation.Arguments = new object[]
            {
                a,
                parentEx,
                builder,
                transformation
            };

            invocation.ReturnValue = null;
            invocation.MethodName = "MoreArguments";
            invocation.OriginalType = typeof(INonReturn);

            this.interceptor.Intercept(invocation, false);
        }

        public void OneArgument(StringBuilder a)
        {
            InvocationExample invocation = new InvocationExample();
            invocation.Arguments = new object[]
            {
                a
            };

            invocation.ReturnValue = null;
            invocation.MethodName = "OneArgument";
            invocation.OriginalType = typeof(INonReturn);

            this.interceptor.Intercept(invocation, false);
        }

        public void OneArgument(string a)
        {
            InvocationExample invocation = new InvocationExample();
            invocation.Arguments = new object[]
            {
                a
            };

            invocation.ReturnValue = null;
            invocation.MethodName = "OneArgument";
            invocation.OriginalType = typeof(INonReturn);

            this.interceptor.Intercept(invocation, false);
        }

        public void OneArgument(int a)
        {
            InvocationExample invocation = new InvocationExample();
            invocation.Arguments = new object[]
            {
                a
            };

            invocation.ReturnValue = null;
            invocation.MethodName = "OneArgument";
            invocation.OriginalType = typeof(INonReturn);

            this.interceptor.Intercept(invocation, false);
        }

        public void TwoArguments(int a, StringBuilder sb)
        {
            InvocationExample invocation = new InvocationExample();
            invocation.Arguments = new object[]
            {
                a,
                sb
            };

            invocation.ReturnValue = null;
            invocation.MethodName = "TwoArguments";
            invocation.OriginalType = typeof(INonReturn);
            invocation.ArgumentTypes = new Type[]
            {
                typeof(int),
                typeof(StringBuilder)
            };

            this.interceptor.Intercept(invocation, false);
        }

        public void NotImplement()
        {
            throw new NotImplementedException();
        }

        public string NotImplementSr()
        {
            throw new NotImplementedException();
        }
    }
}
