using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;

namespace MassiveDynamicProxyGenerator.SimpleInjector.Tests
{
    [TestClass]
    public class ProxyGeneratorFactoryTests
    {
        [TestMethod]
        public void ProxyGeneratorFactory_Init()
        {
            ProxyGeneratorFactory.Factory.ShouldNotBeNull();
        }

        [TestMethod]
        public void ProxyGeneratorFactory_GetInstance()
        {
            ProxyGeneratorFactory.Factory.GetInstance().ShouldNotBeNull();
        }

        [TestMethod]
        public void ProxyGeneratorFactory_OverrideFactory()
        {
            ProxyGenerator generator = new ProxyGenerator();

            ProxyGeneratorFactory.Factory.GetInstance().ShouldNotBeSameAs(generator);

            ProxyGeneratorFactory.OverrideFactory(new TestGeneratorFactory(generator));

            ProxyGeneratorFactory.Factory.GetInstance().ShouldBeSameAs(generator);
        }

        private class TestGeneratorFactory : IProxyGeneratorFactory
        {
            private readonly IProxyGenerator generator;

            public TestGeneratorFactory(IProxyGenerator generator)
            {
                this.generator = generator;
            }

            public IProxyGenerator GetInstance()
            {
                return this.generator;
            }
        }
    }
}
