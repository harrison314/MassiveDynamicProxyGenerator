using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using MassiveDynamicProxyGenerator.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using MassiveDynamicProxyGenerator.DependencyInjection.Test.Services;
using Moq;
using Shouldly;

namespace MassiveDynamicProxyGenerator.DependencyInjection.Test
{
    [TestClass]
    public class InterceptionTests
    {
        #region ICallable interceptor

        public class InreceptorDependtypeB : ICallableInterceptor
        {
            private readonly ITypeB typeB;

            public InreceptorDependtypeB(ITypeB typeB)
            {
                this.typeB = typeB;
            }

            public void Intercept(ICallableInvocation invocation)
            {
                this.typeB.FooForB();
                invocation.Process();
            }
        }

        #endregion

        [TestMethod]
        public void AddInterceptDecorator_Generic_Intercepted()
        {
            Mock<ITypeB> typeBMock = new Mock<ITypeB>(MockBehavior.Strict);
            typeBMock.Setup(t => t.FooForB()).Verifiable();

            ServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddTransient<IMessageService, MessageService>();
            serviceCollection.AddSingleton<ITypeB>(typeBMock.Object);

            serviceCollection.AddInterceptedDecorator<IMessageService, InreceptorDependtypeB>();

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            IMessageService typeA = serviceProvider.GetRequiredService<IMessageService>();
            typeA.ShouldNotBeNull();
            typeA.ShouldNotBeOfType<MessageService>();

            typeA.Send("test@test.sk", "body");

            typeBMock.VerifyAll();
        }

        [TestMethod]
        public void AddInterceptDecorator_NonGeneric_Intercepted()
        {
            Mock<ITypeB> typeBMock = new Mock<ITypeB>(MockBehavior.Strict);
            typeBMock.Setup(t => t.FooForB()).Verifiable();

            ServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddTransient<IMessageService, MessageService>();
            serviceCollection.AddSingleton<ITypeB>(typeBMock.Object);

            serviceCollection.AddInterceptedDecorator(typeof(IMessageService), typeof(InreceptorDependtypeB));

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            IMessageService typeA = serviceProvider.GetRequiredService<IMessageService>();
            typeA.ShouldNotBeNull();
            typeA.ShouldNotBeOfType<MessageService>();

            typeA.Send("test@test.sk", "body");

            typeBMock.VerifyAll();
        }

        [TestMethod]
        public void AddInterceptDecorator_NonGenericWithInterceptorInstance_Intercepted()
        {
            ServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddTransient<IMessageService, MessageService>();

            bool isCall = false;
            serviceCollection.AddInterceptedDecorator(typeof(IMessageService), new CallableInterceptorAdapter(invocation=>
            {
                isCall = true;
                invocation.MethodName.ShouldBe("Send");
            }));

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            IMessageService typeA = serviceProvider.GetRequiredService<IMessageService>();
            typeA.ShouldNotBeNull();
            typeA.ShouldNotBeOfType<MessageService>();

            typeA.Send("test@test.sk", "body");

            isCall.ShouldBeTrue("Interceptor was not call.");
        }

        [TestMethod]
        public void AddInterceptDecorator_NonGenericWithInterceptorFactory_Intercepted()
        {
            ServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddTransient<IMessageService, MessageService>();

            bool isCall = false;
            ICallableInterceptor interceptor = new CallableInterceptorAdapter(invocation =>
            {
                isCall = true;
                invocation.MethodName.ShouldBe("Send");
            });

            serviceCollection.AddInterceptedDecorator(typeof(IMessageService), (sp)=>
            {
                sp.ShouldNotBeNull();
                return interceptor;
            });

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            IMessageService typeA = serviceProvider.GetRequiredService<IMessageService>();
            typeA.ShouldNotBeNull();
            typeA.ShouldNotBeOfType<MessageService>();

            typeA.Send("test@test.sk", "body");

            isCall.ShouldBeTrue("Interceptor was not call.");
        }

        [TestMethod]
        public void AddInterceptDecorator_NonGenericWithPredicate_Intercepted()
        {
            Mock<ITypeB> typeBMock = new Mock<ITypeB>(MockBehavior.Strict);
            typeBMock.Setup(t => t.FooForB()).Verifiable();

            ServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddTransient<IMessageService, MessageService>();
            serviceCollection.AddTransient<ITypeA, TypeA>();
            serviceCollection.AddTransient<ITypeC, TypeC>();
            serviceCollection.AddTransient<IMessageService, MessageService>();
            serviceCollection.AddSingleton<ITypeB>(typeBMock.Object);

            serviceCollection.AddInterceptedDecorator(type => type.Name.EndsWith("A") || type.Name.EndsWith("C") || type.Name.EndsWith("Service"),
                typeof(InreceptorDependtypeB));

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            IMessageService typeA = serviceProvider.GetRequiredService<IMessageService>();
            typeA.ShouldNotBeNull();
            typeA.ShouldNotBeOfType<MessageService>();

            serviceProvider.GetRequiredService<ITypeA>().ShouldNotBeOfType<TypeA>();
            serviceProvider.GetRequiredService<ITypeC>().ShouldNotBeOfType<TypeC>();

            typeA.Send("test@test.sk", "body");

            typeBMock.VerifyAll();
        }

        [TestMethod]
        public void AddInterceptDecorator_FactoryWithPredicate_Intercepted()
        {
            Mock<ITypeB> typeBMock = new Mock<ITypeB>(MockBehavior.Strict);
            typeBMock.Setup(t => t.FooForB()).Verifiable();

            ServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddTransient<IMessageService, MessageService>();
            serviceCollection.AddTransient<ITypeA, TypeA>();
            serviceCollection.AddTransient<ITypeC, TypeC>();
            serviceCollection.AddTransient<IMessageService, MessageService>();
            serviceCollection.AddSingleton<ITypeB>(typeBMock.Object);



            bool isCall = false;
            ICallableInterceptor interceptor = new CallableInterceptorAdapter(invocation =>
            {
                isCall = true;
                invocation.MethodName.ShouldBe("Send");
            });

            serviceCollection.AddInterceptedDecorator(type => type.Name.EndsWith("A") || type.Name.EndsWith("C") || type.Name.EndsWith("Service"), (sp) =>
            {
                sp.ShouldNotBeNull();
                return interceptor;
            });

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            IMessageService typeA = serviceProvider.GetRequiredService<IMessageService>();
            typeA.ShouldNotBeNull();
            typeA.ShouldNotBeOfType<MessageService>();

            serviceProvider.GetRequiredService<ITypeA>().ShouldNotBeOfType<TypeA>();
            serviceProvider.GetRequiredService<ITypeB>().ShouldNotBeOfType<TypeB>();

            typeA.Send("test@test.sk", "body");

            isCall.ShouldBeTrue("Interceptor was not call.");
        }

        [TestMethod]
        public void AddInterceptDecorator_InstanceWithPredicate_Intercepted()
        {
            Mock<ITypeB> typeBMock = new Mock<ITypeB>(MockBehavior.Strict);
            typeBMock.Setup(t => t.FooForB()).Verifiable();

            ServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddTransient<IMessageService, MessageService>();
            serviceCollection.AddTransient<ITypeA, TypeA>();
            serviceCollection.AddTransient<ITypeC, TypeC>();
            serviceCollection.AddTransient<IMessageService, MessageService>();
            serviceCollection.AddSingleton<ITypeB>(typeBMock.Object);

            bool isCall = false;
            ICallableInterceptor interceptor = new CallableInterceptorAdapter(invocation =>
            {
                isCall = true;
                invocation.MethodName.ShouldBe("Send");
            });

            serviceCollection.AddInterceptedDecorator(type => type.Name.EndsWith("A") || type.Name.EndsWith("C") || type.Name.EndsWith("Service"), interceptor);

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            IMessageService typeA = serviceProvider.GetRequiredService<IMessageService>();
            typeA.ShouldNotBeNull();
            typeA.ShouldNotBeOfType<MessageService>();

            serviceProvider.GetRequiredService<ITypeA>().ShouldNotBeOfType<TypeA>();
            serviceProvider.GetRequiredService<ITypeC>().ShouldNotBeOfType<TypeC>();

            typeA.Send("test@test.sk", "body");

            isCall.ShouldBeTrue("Interceptor was not call.");
        }
    }
}
