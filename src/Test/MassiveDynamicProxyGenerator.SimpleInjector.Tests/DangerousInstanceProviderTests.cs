using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using Shouldly;
using MassiveDynamicProxyGenerator.SimpleInjector.Dangerous;
using MassiveDynamicProxyGenerator.SimpleInjector.Tests.Services;

namespace MassiveDynamicProxyGenerator.SimpleInjector.Tests
{
    [TestClass]
    public class DangerousInstanceProviderTests
    {
        [TestMethod]
        public void InstanceProvider_TypedNew_Sucess()
        {
            Container container = this.CrateDefaultContaner();
            container.RegisterInstanceProxy(typeof(IMessageService), typeof(MessageServiceInstanceProvider), Lifestyle.Scoped);

            container.Verify();

            using (ThreadScopedLifestyle.BeginScope(container))
            {
                IMessageService messageService = container.GetInstance<IMessageService>();
                messageService.ShouldNotBeNull();

                messageService.GetCountOfMessagesInFront().ShouldBe(12);
            }
        }

        [TestMethod]
        public void InstanceProvider_TypedNewGeneric_Sucess()
        {
            Container container = this.CrateDefaultContaner();
            container.RegisterInstanceProxy<IMessageService, MessageServiceInstanceProvider>(Lifestyle.Scoped);

            container.Verify();

            using (ThreadScopedLifestyle.BeginScope(container))
            {
                IMessageService messageService = container.GetInstance<IMessageService>();
                messageService.ShouldNotBeNull();

                messageService.GetCountOfMessagesInFront().ShouldBe(12);
            }
        }

        [TestMethod]
        public void InstanceProvider_TypedIoc_Sucess()
        {
            Container container = this.CrateDefaultContaner();
            container.Register<MessageDependentIInstanceProvicer>(Lifestyle.Scoped);
            container.RegisterInstanceProxy(typeof(IMessageService), typeof(MessageDependentIInstanceProvicer), Lifestyle.Scoped);

            container.Verify();

            using (ThreadScopedLifestyle.BeginScope(container))
            {
                IMessageService messageService = container.GetInstance<IMessageService>();
                messageService.ShouldNotBeNull();

                messageService.GetCountOfMessagesInFront().ShouldBe(12);
            }
        }

        [TestMethod]
        public void InstanceProvider_TypedIocGeneric_Sucess()
        {
            Container container = this.CrateDefaultContaner();
            container.Register<MessageDependentIInstanceProvicer>(Lifestyle.Scoped);
            container.RegisterInstanceProxy<IMessageService, MessageDependentIInstanceProvicer>(Lifestyle.Scoped);

            container.Verify();

            using (ThreadScopedLifestyle.BeginScope(container))
            {
                IMessageService messageService = container.GetInstance<IMessageService>();
                messageService.ShouldNotBeNull();

                messageService.GetCountOfMessagesInFront().ShouldBe(12);
            }
        }

        [TestMethod]
        public void InstanceProvider_TypedNewOpenGeneric_Sucess()
        {
            Container container = this.CrateDefaultContaner();
            container.Register(typeof(GenericInstanceProducer<>), typeof(GenericInstanceProducer<>), Lifestyle.Scoped);
            container.RegisterInstanceProxy(typeof(IGenericService<>), typeof(GenericInstanceProducer<>), Lifestyle.Scoped);

            container.Verify();

            using (ThreadScopedLifestyle.BeginScope(container))
            {
                IGenericService<string> service = container.GetInstance<IGenericService<string>>();
                service.ShouldNotBeNull();
            }
        }

        private Container CrateDefaultContaner()
        {
            Container container = new Container();
            container.Options.DefaultScopedLifestyle = new ThreadScopedLifestyle();
            container.Register<ITypeA, TypeA>(Lifestyle.Scoped);
            container.Register<ITypeB, TypeB>(Lifestyle.Scoped);
            container.Register<ITypeC, TypeC>(Lifestyle.Scoped);

            return container;
        }
    }
}
