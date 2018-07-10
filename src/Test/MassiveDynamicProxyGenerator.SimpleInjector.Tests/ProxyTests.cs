using System;
using Shouldly;
using SimpleInjector;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MassiveDynamicProxyGenerator.SimpleInjector.Tests.Services;

namespace MassiveDynamicProxyGenerator.SimpleInjector.Tests
{
    [TestClass]
    public class ProxyTests
    {
        [TestMethod]
        public void Proxy_TypeInterceptor_Sucess()
        {
            Container container = this.CrateDefaultContaner();
            container.RegisterInstance<InterceptorAdapter>(new InterceptorAdapter(invodcation =>
            {
                if (invodcation.MethodName == nameof(IMessageService.GetCountOfMessagesInFront))
                {
                    invodcation.ReturnValue = 42;
                }
            }));

            container.RegisterProxy(typeof(IMessageService), typeof(InterceptorAdapter));
            container.Verify();

            container.GetInstance<IMessageService>().ShouldNotBeNull();
            container.GetInstance<IMessageService>().GetCountOfMessagesInFront().ShouldBe(42);
        }

        [TestMethod]
        public void Proxy_OpenGenericTypeInterceptor_Sucess()
        {
            Container container = this.CrateDefaultContaner();
            container.RegisterInstance<InterceptorAdapter>(new InterceptorAdapter(invodcation =>
            {
                if (invodcation.MethodName == nameof(IGenericService<int>.GetLast))
                {
                    invodcation.ReturnValue = 42;
                }
            }));

            container.RegisterProxy(typeof(IGenericService<>), typeof(InterceptorAdapter));
            container.Verify();

            container.GetInstance<IGenericService<int>>().ShouldNotBeNull();
            container.GetInstance<IGenericService<string>>().ShouldNotBeNull();
            container.GetInstance<IGenericService<Action<int>>>().ShouldNotBeNull();
            container.GetInstance<IGenericService<int>>().GetLast().ShouldBe(42);
        }

        [TestMethod]
        public void Proxy_InstanceInterceptor_Sucess()
        {
            Container container = this.CrateDefaultContaner();
            InterceptorAdapter adapter = new InterceptorAdapter(invodcation =>
            {
                if (invodcation.MethodName == nameof(IMessageService.GetCountOfMessagesInFront))
                {
                    invodcation.ReturnValue = 42;
                }
            });

            container.RegisterProxy(typeof(IMessageService), adapter);
            container.Verify();

            container.GetInstance<IMessageService>().ShouldNotBeNull();
            container.GetInstance<IMessageService>().GetCountOfMessagesInFront().ShouldBe(42);
        }

        [TestMethod]
        public void Proxy_OpenGenericInstanceInterceptor_Sucess()
        {
            Container container = this.CrateDefaultContaner();
            InterceptorAdapter adapter = new InterceptorAdapter(invodcation =>
            {
                if (invodcation.MethodName == nameof(IGenericService<int>.GetLast))
                {
                    invodcation.ReturnValue = 42;
                }
            });

            container.RegisterProxy(typeof(IGenericService<>), adapter);
            container.Verify();

            container.GetInstance<IGenericService<int>>().ShouldNotBeNull();
            container.GetInstance<IGenericService<string>>().ShouldNotBeNull();
            container.GetInstance<IGenericService<Action<int>>>().ShouldNotBeNull();
            container.GetInstance<IGenericService<int>>().GetLast().ShouldBe(42);
        }

        [TestMethod]
        public void Proxy_FactoryInterceptor_Sucess()
        {
            Container container = this.CrateDefaultContaner();
            InterceptorAdapter adapter = new InterceptorAdapter(invodcation =>
            {
                if (invodcation.MethodName == nameof(IMessageService.GetCountOfMessagesInFront))
                {
                    invodcation.ReturnValue = 42;
                }
            });

            Func<IInterceptor> factory = () => adapter;

            container.RegisterProxy(typeof(IMessageService), factory);
            container.Verify();

            container.GetInstance<IMessageService>().ShouldNotBeNull();
            container.GetInstance<IMessageService>().GetCountOfMessagesInFront().ShouldBe(42);
        }

        [TestMethod]
        public void Proxy_OpenGenericFactoryInterceptor_Sucess()
        {
            Container container = this.CrateDefaultContaner();
            InterceptorAdapter adapter = new InterceptorAdapter(invodcation =>
            {
                if (invodcation.MethodName == nameof(IGenericService<int>.GetLast))
                {
                    invodcation.ReturnValue = 42;
                }
            });

            Func<IInterceptor> factory = () => adapter;

            container.RegisterProxy(typeof(IGenericService<>), factory);
            container.Verify();

            container.GetInstance<IGenericService<int>>().ShouldNotBeNull();
            container.GetInstance<IGenericService<string>>().ShouldNotBeNull();
            container.GetInstance<IGenericService<Action<int>>>().ShouldNotBeNull();
            container.GetInstance<IGenericService<int>>().GetLast().ShouldBe(42);
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
