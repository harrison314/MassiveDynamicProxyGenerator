using System;
using System.Collections.Generic;
using System.Text;

namespace MassiveDynamicProxyGenerator.DependencyInjection.Test.Services
{
    public class TypeADependInterceptor : IInterceptor
    {
        private readonly ITypeA typeA;

        public TypeADependInterceptor(ITypeA typeA)
        {
            this.typeA = typeA;
        }

        public void Intercept(IInvocation invocation)
        {
            
        }
    }
}
