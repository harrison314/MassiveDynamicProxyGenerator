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
    // add style corp
    // add cache pre vicere typy
    // http://osherove.com/blog/2005/4/3/naming-standards-for-unit-tests.html
    [TestClass]
    public class TestMultiProxy
    {
        [TestMethod]
        public void GenerateProxyMultiInterface_Default_CallMethods() //TODO: premenovat
        {
            Mock<IInterceptor> interceptor = new Mock<IInterceptor>(MockBehavior.Strict);
            interceptor.Setup(t => t.Intercept(It.Is<IInvocation>(q => q.Arguments.Length == 0 && q.MethodName == "EmptyMethod"), false))
                .Verifiable();
            interceptor.Setup(t => t.Intercept(It.Is<IInvocation>(q => q.Arguments.Length == 0 && q.MethodName == "Dispose"), false))
                .Verifiable();

            ProxygGenerator generator = new ProxygGenerator();

            INonReturn instance = generator.GenerateProxy<INonReturn>(interceptor.Object, typeof(IDisposable), typeof(IReturnTypes));
            instance.ShouldNotBeNull();

            Assert.IsTrue(instance is IDisposable);
            Assert.IsTrue(instance is IReturnTypes);

            instance.EmptyMethod();

            (instance as IDisposable).Dispose();

            interceptor.VerifyAll();
        }

        [TestMethod]
        public void GenerateProxyMultiInterface_Object_CallMethods() //TODO: premenovat
        {
            Mock<IInterceptor> interceptor = new Mock<IInterceptor>(MockBehavior.Strict);
            interceptor.Setup(t => t.Intercept(It.Is<IInvocation>(q => q.Arguments.Length == 0 && q.MethodName == "EmptyMethod"), false))
                .Verifiable();
            interceptor.Setup(t => t.Intercept(It.Is<IInvocation>(q => q.Arguments.Length == 0 && q.MethodName == "Dispose"), false))
                .Verifiable();

            ProxygGenerator generator = new ProxygGenerator();

            object instance = generator.GenerateProxy(interceptor.Object, typeof(INonReturn), typeof(IDisposable), typeof(IReturnTypes));
            instance.ShouldNotBeNull();

            Assert.IsTrue(instance is IDisposable);
            Assert.IsTrue(instance is IReturnTypes);

            (instance as INonReturn).EmptyMethod();

            (instance as IDisposable).Dispose();

            interceptor.VerifyAll();
        }

        [TestMethod]
        public void GenerateProxyMultiinterface_MultiInherence_CallMethods()
        {
            Mock<IInterceptor> interceptor = new Mock<IInterceptor>(MockBehavior.Strict);
            interceptor.Setup(t => t.Intercept(It.Is<IInvocation>(q => q.Arguments.Length == 1 && q.MethodName == "AddChild"), false))
                .Verifiable();
            interceptor.Setup(t => t.Intercept(It.Is<IInvocation>(q => q.Arguments.Length == 0 && q.MethodName == "Dispose"), false))
                .Verifiable();

            ProxygGenerator generator = new ProxygGenerator();

            ICompositeInterface instance = generator.GenerateProxy<ICompositeInterface>(interceptor.Object, typeof(IDisposable), typeof(IReturnTypes));
            instance.ShouldNotBeNull();

            Assert.IsTrue(instance is IDisposable);
            Assert.IsTrue(instance is IReturnTypes);

            instance.AddChild(null);

            (instance as IDisposable).Dispose();

            interceptor.VerifyAll();
        }
    }
}
