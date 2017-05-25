using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleInjector;
using Shouldly;
using MassiveDynamicProxyGenerator.SimpleInjector;
using MassiveDynamicProxyGenerator.SimpleInjector.Tests.Services;

namespace MassiveDynamicProxyGenerator.SimpleInjector.Tests
{
    [TestClass]
    public class InstanceProxyTests
    {
        [TestMethod]
        public void InstanceProvider_InstanceNew_Sucess()
        {
            Container container = this.CrateDefaultContaner();
            container.RegisterInstanceProxy(typeof(IMessageService), new MessageServiceInstanceProvider());

            container.Verify();

            IMessageService messageService = container.GetInstance<IMessageService>();
            messageService.ShouldNotBeNull();

            messageService.GetCountOfMessagesInFront().ShouldBe(12);
        }

        [TestMethod]
        public void InstanceProvider_InstanceNewWithLifestyle_Sucess()
        {
            Container container = this.CrateDefaultContaner();
            container.RegisterInstanceProxy(typeof(IMessageService), new MessageServiceInstanceProvider(), Lifestyle.Transient);

            container.Verify();

            IMessageService messageService = container.GetInstance<IMessageService>();
            messageService.ShouldNotBeNull();

            messageService.GetCountOfMessagesInFront().ShouldBe(12);
        }

        [TestMethod]
        public void InstanceProvider_InstanceNewGeneric_Sucess()
        {
            Container container = this.CrateDefaultContaner();
            container.RegisterInstanceProxy<IMessageService>(new MessageServiceInstanceProvider());

            container.Verify();

            IMessageService messageService = container.GetInstance<IMessageService>();
            messageService.ShouldNotBeNull();

            messageService.GetCountOfMessagesInFront().ShouldBe(12);
        }

        [TestMethod]
        public void InstanceProvider_InstanceNewGenericWithLifeStyle_Sucess()
        {
            Container container = this.CrateDefaultContaner();
            container.RegisterInstanceProxy<IMessageService>(new MessageServiceInstanceProvider(), Lifestyle.Singleton);

            container.Verify();

            IMessageService messageService = container.GetInstance<IMessageService>();
            messageService.ShouldNotBeNull();

            messageService.GetCountOfMessagesInFront().ShouldBe(12);
        }

        [TestMethod]
        public void InstanceProvider_InstanceFuncNew_Sucess()
        {
            Container container = this.CrateDefaultContaner();
            container.RegisterInstanceProxy(typeof(IMessageService), () => new MessageService());

            container.Verify();

            IMessageService messageService = container.GetInstance<IMessageService>();
            messageService.ShouldNotBeNull();

            messageService.GetCountOfMessagesInFront().ShouldBe(12);
        }

        [TestMethod]
        public void InstanceProvider_InstanceFuncNewWithLifeStyle_Sucess()
        {
            Container container = this.CrateDefaultContaner();
            container.RegisterInstanceProxy(typeof(IMessageService), () => new MessageService(), Lifestyle.Singleton);

            container.Verify();

            IMessageService messageService = container.GetInstance<IMessageService>();
            messageService.ShouldNotBeNull();

            messageService.GetCountOfMessagesInFront().ShouldBe(12);
        }

        [TestMethod]
        public void InstanceProvider_InstanceFuncNewGeneric_Sucess()
        {
            Container container = this.CrateDefaultContaner();
            container.RegisterInstanceProxy<IMessageService>(() => new MessageService());

            container.Verify();

            IMessageService messageService = container.GetInstance<IMessageService>();
            messageService.ShouldNotBeNull();

            messageService.GetCountOfMessagesInFront().ShouldBe(12);
        }

        [TestMethod]
        public void InstanceProvider_InstanceFuncNewGenericWithLifeStyle_Sucess()
        {
            Container container = this.CrateDefaultContaner();
            container.RegisterInstanceProxy<IMessageService>(() => new MessageService(), Lifestyle.Singleton);

            container.Verify();

            IMessageService messageService = container.GetInstance<IMessageService>();
            messageService.ShouldNotBeNull();

            messageService.GetCountOfMessagesInFront().ShouldBe(12);
        }

        private Container CrateDefaultContaner()
        {
            Container container = new Container();
            container.Register<ITypeA, TypeA>();
            container.Register<ITypeB, TypeB>();
            container.Register<ITypeC, TypeC>();

            return container;
        }
    }
}
