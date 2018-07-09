using System;
using System.Collections.Generic;
using System.Text;

namespace MassiveDynamicProxyGenerator.SimpleInjector
{
    internal class FuncInstanceProvider : IInstanceProvicer
    {
        private readonly Func<object> factory;

        public FuncInstanceProvider(Func<object> factory)
        {
            this.factory = factory;
        }

        public void Dispose()
        {
        }

        public object GetInstance()
        {
            return this.factory();
        }
    }
}
