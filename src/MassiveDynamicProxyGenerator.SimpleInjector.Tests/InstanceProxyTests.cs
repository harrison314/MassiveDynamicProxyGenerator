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
        public void InstanceProvider_TypedNew_Sucess()
        {
            Container container = this.CrateDefaultContaner();
            container.RegisterInstanceProxy(typeof(IMessageService), typeof(MessageServiceInstanceProvider));

            container.Verify();

            IMessageService messageService = container.GetInstance<IMessageService>();
            messageService.ShouldNotBeNull();

            messageService.GetCountOfMessagesInFront().ShouldBe(12);
        }

        [TestMethod]
        public void InstanceProvider_TypedNewGeneric_Sucess()
        {
            Container container = this.CrateDefaultContaner();
            container.RegisterInstanceProxy<IMessageService, MessageServiceInstanceProvider>();

            container.Verify();

            IMessageService messageService = container.GetInstance<IMessageService>();
            messageService.ShouldNotBeNull();

            messageService.GetCountOfMessagesInFront().ShouldBe(12);
        }

        [TestMethod]
        public void InstanceProvider_TypedIoc_Sucess()
        {
            Container container = this.CrateDefaultContaner();
            container.Register<MessageDependentIInstanceProvicer>();
            container.RegisterInstanceProxy(typeof(IMessageService), typeof(MessageDependentIInstanceProvicer));

            container.Verify();

            IMessageService messageService = container.GetInstance<IMessageService>();
            messageService.ShouldNotBeNull();

            messageService.GetCountOfMessagesInFront().ShouldBe(12);
        }

        [TestMethod]
        public void InstanceProvider_TypedIocGeneric_Sucess()
        {
            Container container = this.CrateDefaultContaner();
            container.Register<MessageDependentIInstanceProvicer>();
            container.RegisterInstanceProxy<IMessageService, MessageDependentIInstanceProvicer>();

            container.Verify();

            IMessageService messageService = container.GetInstance<IMessageService>();
            messageService.ShouldNotBeNull();

            messageService.GetCountOfMessagesInFront().ShouldBe(12);
        }

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
        public void InstanceProvider_InstanceFuncNew_Sucess()
        {
            Container container = this.CrateDefaultContaner();
            container.RegisterInstanceProxy(typeof(IMessageService), ()=> new MessageService());

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

        private Container CrateDefaultContaner()
        {
            Container container = new Container();
            //container.Register<IMessageService, MessageService>();
            container.Register<ITypeA, TypeA>();
            container.Register<ITypeB, TypeB>();
            container.Register<ITypeC, TypeC>();

            return container;
        }
    }
}
