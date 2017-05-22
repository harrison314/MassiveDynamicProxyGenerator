using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;

namespace MassiveDynamicProxyGenerator.SimpleInjector.Tests.Services
{
    public class MessageDependentIInstanceProvicer : IInstanceProvicer
    {
        private readonly ITypeA typeA;
        private readonly ITypeB typeB;

        public MessageDependentIInstanceProvicer(ITypeA typeA, ITypeB typeB)
        {
            typeA.ShouldNotBeNull();
            typeB.ShouldNotBeNull();

            this.typeA = typeA;
            this.typeB = typeB;
        }

        public void Dispose()
        {
        }

        public object GetInstance()
        {
            this.typeA.GetType().ShouldNotBeNull();
            this.typeB.GetType().ShouldNotBeNull();

            return new MessageService();
        }
    }
}
