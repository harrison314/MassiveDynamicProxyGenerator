using System;
using System.Collections.Generic;
using System.Text;

namespace MassiveDynamicProxyGenerator.Microsoft.DependencyInjection
{
    internal class FuncInstanceProvider : IInstanceProvicer
    {
        private readonly Func<object> instaceFyctory;

        public FuncInstanceProvider(Func<object> instaceFyctory)
        {
            this.instaceFyctory = instaceFyctory;
        }

        public void Dispose()
        {
            
        }

        public object GetInstance()
        {
            return this.instaceFyctory.Invoke();
        }
    }
}
