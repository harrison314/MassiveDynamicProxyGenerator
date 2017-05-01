using System;
using Shouldly;
using SimpleInjector;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MassiveDynamicProxyGenerator.SimpleInjector.Tests.Services;

namespace MassiveDynamicProxyGenerator.SimpleInjector.Tests
{
    [TestClass]
    public class MockTests
    {
        [TestMethod]
        public void MockGeneric_Default_CheckInstance()
        {
            Container container = this.CrateDefaultContaner();

            container.RegisterMock<IMessageService>();

            container.Verify();

            IMessageService service= container.GetInstance<IMessageService>();

            service.ShouldNotBeNull();
            service.GetCountOfMessagesInFront().ShouldBe(default(int));
        }

        [TestMethod]
        public void Mock_Default_CheckInstance()
        {
            Container container = this.CrateDefaultContaner();

            container.RegisterMock(typeof(IMessageService));

            container.Verify();

            IMessageService service = container.GetInstance<IMessageService>();

            service.ShouldNotBeNull();
            service.GetCountOfMessagesInFront().ShouldBe(default(int));
        }

        [TestMethod]
        public void Mock_GenericService_CheckInstance()
        {
            Container container = this.CrateDefaultContaner();

            container.RegisterMock(typeof(IGenericService<>));

            container.Verify();

            IGenericService<int> service = container.GetInstance<IGenericService<int>>();

            service.ShouldNotBeNull();
            service.GetLast().ShouldBe(default(int));

            container.GetInstance<IGenericService<string>>().ShouldNotBeNull();
            container.GetInstance<IGenericService<IServiceProvider>>().ShouldNotBeNull();
        }


        private Container CrateDefaultContaner()
        {
            Container container = new Container();

            return container;
        }
    }
}
