using System;
using Shouldly;
using SimpleInjector;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MassiveDynamicProxyGenerator.SimpleInjector.Tests.Services;

namespace MassiveDynamicProxyGenerator.SimpleInjector.Tests
{
    [TestClass]
    public class InterceptedpDecoratorTests
    {
        [TestMethod]
        public  void Intercept_Default_AnotherType()
        {
            Container container = this.CrateDefaultContaner();
            container.RegisterSingleton<CallableInterceptorAdapter>(new CallableInterceptorAdapter(invodcation =>
            {
                invodcation.Process();
                if(invodcation.MethodName == nameof(IMessageService.GetCountOfMessagesInFront))
                {
                    invodcation.ReturnValue = 42;
                }
            }));


            container.RegisterInterceptedDecorator(typeof(CallableInterceptorAdapter), t => !t.Name.StartsWith("IType"));

            container.Verify();

            IMessageService service = container.GetInstance<IMessageService>();
            service.ShouldNotBeNull();
            service.ShouldNotBeOfType<MessageService>();
            service.GetCountOfMessagesInFront().ShouldBe(42);

            container.GetInstance<ITypeB>().ShouldBeOfType<TypeB>();

        }

        [TestMethod]
        public void Intercept_WithInstance_AnotherType()
        {
            Container container = this.CrateDefaultContaner();
            CallableInterceptorAdapter adapter = new CallableInterceptorAdapter((invodcation) =>
            {
                invodcation.Process();
                if (invodcation.MethodName == nameof(IMessageService.GetCountOfMessagesInFront))
                {
                    invodcation.ReturnValue = 42;
                }
            });


            container.RegisterInterceptedDecorator(adapter, t => !t.Name.StartsWith("IType"));

            container.Verify();

            IMessageService service = container.GetInstance<IMessageService>();
            service.ShouldNotBeNull();
            service.ShouldNotBeOfType<MessageService>();
            service.GetCountOfMessagesInFront().ShouldBe(42);

            container.GetInstance<ITypeB>().ShouldBeOfType<TypeB>();

        }

        [TestMethod]
        public void Intercept_WithFactory_AnotherType()
        {
            Container container = this.CrateDefaultContaner();
            CallableInterceptorAdapter adapter = new CallableInterceptorAdapter((invodcation) =>
            {
                invodcation.Process();
                if (invodcation.MethodName == nameof(IMessageService.GetCountOfMessagesInFront))
                {
                    invodcation.ReturnValue = 42;
                }
            });


            Func<ICallableInterceptor> factory = () => adapter;

            container.RegisterInterceptedDecorator(factory, t => !t.Name.StartsWith("IType"));

            container.Verify();

            IMessageService service = container.GetInstance<IMessageService>();
            service.ShouldNotBeNull();
            service.ShouldNotBeOfType<MessageService>();
            service.GetCountOfMessagesInFront().ShouldBe(42);

            container.GetInstance<ITypeB>().ShouldBeOfType<TypeB>();
        }


        private Container CrateDefaultContaner()
        {
            Container container = new Container();
            container.Register<IMessageService, MessageService>();
            container.Register<ITypeA, TypeA>();
            container.Register<ITypeB, TypeB>();
            container.Register<ITypeC, TypeC>();
            return container;
        }
    }
}
