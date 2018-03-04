using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MassiveDynamicProxyGenerator.DependencyInjection.Test.Services;
using Moq;
using Shouldly;
using MassiveDynamicProxyGenerator.Microsoft.DependencyInjection;

namespace MassiveDynamicProxyGenerator.DependencyInjection.Test
{
    [TestClass]
    public class InstanceProxyTests
    {
        [TestMethod]
        public void AddInstanceProxy_TypeInstance_Create()
        {
            Mock<IInstanceProvicer> instanceProviderMock = new Mock<IInstanceProvicer>(MockBehavior.Strict);
            instanceProviderMock.Setup(t => t.GetInstance()).Returns(new MessageService()).Verifiable();

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddInstanceProxy(typeof(IMessageService), instanceProviderMock.Object);

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            IMessageService typeA = serviceProvider.GetRequiredService<IMessageService>();
            typeA.ShouldNotBeNull();
            typeA.ShouldNotBeOfType<MessageService>();

            typeA.Send("test@test.sk", "body");

            instanceProviderMock.Verify();
        }

        [TestMethod]
        public void AddInstanceProxy_TypeInstanceWithLifestyle_Create()
        {
            Mock<IInstanceProvicer> instanceProviderMock = new Mock<IInstanceProvicer>(MockBehavior.Strict);
            instanceProviderMock.Setup(t => t.GetInstance()).Returns(new MessageService()).Verifiable();

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddInstanceProxy(typeof(IMessageService), instanceProviderMock.Object, ServiceLifetime.Singleton);

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            IMessageService typeA = serviceProvider.GetRequiredService<IMessageService>();
            typeA.ShouldNotBeNull();
            typeA.ShouldNotBeOfType<MessageService>();

            typeA.Send("test@test.sk", "body");

            instanceProviderMock.Verify();
        }

        [TestMethod]
        public void AddInstanceProxy_GenericInstance_Crate()
        {
            Mock<IInstanceProvicer> instanceProviderMock = new Mock<IInstanceProvicer>(MockBehavior.Strict);
            instanceProviderMock.Setup(t => t.GetInstance()).Returns(new MessageService()).Verifiable();

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddInstanceProxy<IMessageService>(instanceProviderMock.Object);

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            IMessageService typeA = serviceProvider.GetRequiredService<IMessageService>();
            typeA.ShouldNotBeNull();
            typeA.ShouldNotBeOfType<MessageService>();

            typeA.Send("test@test.sk", "body");

            instanceProviderMock.Verify();
        }

        [TestMethod]
        public void AddInstanceProxy_GenericInstanceWithlifestyle_Crate()
        {
            Mock<IInstanceProvicer> instanceProviderMock = new Mock<IInstanceProvicer>(MockBehavior.Strict);
            instanceProviderMock.Setup(t => t.GetInstance()).Returns(new MessageService()).Verifiable();

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddInstanceProxy<IMessageService>(instanceProviderMock.Object, ServiceLifetime.Singleton);

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            IMessageService typeA = serviceProvider.GetRequiredService<IMessageService>();
            typeA.ShouldNotBeNull();
            typeA.ShouldNotBeOfType<MessageService>();

            typeA.Send("test@test.sk", "body");

            instanceProviderMock.Verify();
        }

        [TestMethod]
        public void AddInstanceProxy_TypeFactory_Crate()
        {
            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddInstanceProxy(typeof(IMessageService), () => new MessageService());

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            IMessageService typeA = serviceProvider.GetRequiredService<IMessageService>();
            typeA.ShouldNotBeNull();
            typeA.ShouldNotBeOfType<MessageService>();

            typeA.Send("test@test.sk", "body");
        }

        [TestMethod]
        public void AddInstanceProxy_TypeFactoryWithLifeStyle_Crate()
        {
            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddInstanceProxy(typeof(IMessageService), () => new MessageService(), ServiceLifetime.Singleton);

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            IMessageService typeA = serviceProvider.GetRequiredService<IMessageService>();
            typeA.ShouldNotBeNull();
            typeA.ShouldNotBeOfType<MessageService>();

            typeA.Send("test@test.sk", "body");
        }

        [TestMethod]
        public void AddInstanceProxy_GenericFactory_Crate()
        {
            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddInstanceProxy(typeof(IMessageService), () => new MessageService());

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            IMessageService typeA = serviceProvider.GetRequiredService<IMessageService>();
            typeA.ShouldNotBeNull();
            typeA.ShouldNotBeOfType<MessageService>();

            typeA.Send("test@test.sk", "body");
        }

        [TestMethod]
        public void AddInstanceProxy_GenericFactoryWithLifeStyle_Crate()
        {
            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddInstanceProxy(typeof(IMessageService), () => new MessageService(), ServiceLifetime.Singleton);

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            IMessageService typeA = serviceProvider.GetRequiredService<IMessageService>();
            typeA.ShouldNotBeNull();
            typeA.ShouldNotBeOfType<MessageService>();

            typeA.Send("test@test.sk", "body");
        }
    }
}
