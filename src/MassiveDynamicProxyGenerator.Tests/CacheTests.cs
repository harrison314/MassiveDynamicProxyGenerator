using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Shouldly;
using MassiveDynamicProxyGenerator.Tests.TestInterfaces;

namespace MassiveDynamicProxyGenerator.Tests
{
    [TestClass]
    public class CacheTests
    {
        [TestMethod]
        public void CachedGenerateInstance_SingleInterface_MultipleInstances()
        {
            Mock<IInstanceProvicer> instanceProviderMock = new Mock<IInstanceProvicer>();

            ProxyGenerator genartor1 = new ProxyGenerator(cfg =>
            {
                cfg.UseLocalCache = true;
                
            });

            ProxyGenerator genartor2 = new ProxyGenerator(cfg =>
            {
                cfg.UseLocalCache = true;
            });

            IGrapth instance1 = genartor1.GenerateInstanceProxy<IGrapth>(instanceProviderMock.Object);
            IGrapth instance2 = genartor2.GenerateInstanceProxy<IGrapth>(instanceProviderMock.Object);

            instance1.ShouldNotBeNull();
            instance2.ShouldNotBeNull();
            instance1.ShouldNotBySameTypeAs(instance2);
        }

        [TestMethod]
        public void CachedGenerateInstance_MultyInterface_MultipleInstances()
        {
            Mock<IInterceptor> instanceProviderMock = new Mock<IInterceptor>();

            ProxyGenerator genartor1 = new ProxyGenerator(cfg =>
            {
                cfg.UseLocalCache = true;

            });

            ProxyGenerator genartor2 = new ProxyGenerator(cfg =>
            {
                cfg.UseLocalCache = true;
            });

            IGrapth instance1 = genartor1.GenerateProxy<IGrapth>(instanceProviderMock.Object, typeof(IDisposable), typeof(IPrototype));
            IDisposable instance2 = genartor2.GenerateProxy<IDisposable>(instanceProviderMock.Object, typeof(IDisposable), typeof(IGrapth));

            instance1.ShouldNotBeNull();
            instance2.ShouldNotBeNull();
            instance1.ShouldNotBySameTypeAs(instance2);
        }

        [TestMethod]
        public void CachedGenerateInstance_SingleInterface_SingleInstances()
        {

            Mock<IInterceptor> instanceProviderMock = new Mock<IInterceptor>();

            ProxyGenerator genartor1 = new ProxyGenerator(cfg =>
            {
                cfg.UseLocalCache = false;
            });

            ProxyGenerator genartor2 = new ProxyGenerator(cfg =>
            {
                cfg.UseLocalCache = false;
            });

            IGrapth instance1 = genartor1.GenerateProxy<IGrapth>(instanceProviderMock.Object);
            IGrapth instance2 = genartor2.GenerateProxy<IGrapth>(instanceProviderMock.Object);

            instance1.ShouldNotBeNull();
            instance2.ShouldNotBeNull();
            instance1.ShouldBySameTypeAs(instance2);
        }

        [TestMethod]
        public void CachedGenerateInstance_MultyInterface_SingleInstances()
        {
            Mock<IInterceptor> instanceProviderMock = new Mock<IInterceptor>();

            ProxyGenerator genartor1 = new ProxyGenerator(cfg =>
            {
                cfg.UseLocalCache = false;

            });

            ProxyGenerator genartor2 = new ProxyGenerator(cfg =>
            {
                cfg.UseLocalCache = false;
            });

            IGrapth instance1 = genartor1.GenerateProxy<IGrapth>(instanceProviderMock.Object, typeof(IDisposable), typeof(IPrototype));
            IDisposable instance2 = genartor2.GenerateProxy<IDisposable>(instanceProviderMock.Object, typeof(IPrototype), typeof(IGrapth));

            instance1.ShouldNotBeNull();
            instance2.ShouldNotBeNull();
            instance1.ShouldBySameTypeAs(instance2);
        }
    }
}
