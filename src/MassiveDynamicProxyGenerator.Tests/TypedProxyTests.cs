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
    public class TypedProxyTests
    {
        [TestMethod]
        public void TestNonParametric()
        {
            Mock<IInterceptor> interceptor = new Mock<IInterceptor>(MockBehavior.Strict);
            interceptor.Setup(t => t.Intercept(It.Is<IInvocation>(q => q.Arguments.Length == 0 && q.MethodName == "EmptyMethod"), false))
                .Verifiable();

            ProxygGenerator generator = new ProxygGenerator();

            INonReturn instance = generator.GenerateProxy<INonReturn>(interceptor.Object);
            instance.ShouldNotBeNull();

            instance.EmptyMethod();

            interceptor.VerifyAll();
        }

        [TestMethod]
        public void TestValueTypeParameter()
        {
            Mock<IInterceptor> interceptor = new Mock<IInterceptor>(MockBehavior.Strict);
            interceptor.Setup(t => t.Intercept(It.Is<IInvocation>(q => q.Arguments.Length == 1 &&
            ((int)q.Arguments[0]) == 12
            && q.MethodName == "OneArgument"), false))
                .Verifiable();

            ProxygGenerator generator = new ProxygGenerator();

            INonReturn instance = generator.GenerateProxy<INonReturn>(interceptor.Object);
            instance.ShouldNotBeNull();

            instance.OneArgument(12);

            interceptor.VerifyAll();
        }

        [TestMethod]
        public void TestRefTypeParameter()
        {
            StringBuilder sbExcepted = new StringBuilder("klmn");
            Mock<IInterceptor> interceptor = new Mock<IInterceptor>(MockBehavior.Strict);
            interceptor.Setup(t => t.Intercept(It.Is<IInvocation>(q => q.Arguments.Length == 1 &&
            ((StringBuilder)q.Arguments[0]).Equals(sbExcepted)
            && q.MethodName == "OneArgument"), false))
                .Verifiable();

            ProxygGenerator generator = new ProxygGenerator();

            INonReturn instance = generator.GenerateProxy<INonReturn>(interceptor.Object);
            instance.ShouldNotBeNull();

            instance.OneArgument(sbExcepted);

            interceptor.VerifyAll();
        }

        [TestMethod]
        public void TestReturnValueType()
        {
            Mock<IInterceptor> interceptor = new Mock<IInterceptor>(MockBehavior.Strict);
            interceptor.Setup(t => t.Intercept(It.Is<IInvocation>(p => p.MethodName == "GetLenght"), false))
                .Callback<IInvocation, bool>((aa, b) =>
                {
                    aa.ReturnValue = 13;
                })
                .Verifiable();
            ProxygGenerator generator = new ProxygGenerator();

            IReturnTypes instance = generator.GenerateProxy<IReturnTypes>(interceptor.Object);
            instance.ShouldNotBeNull();

            int a = instance.GetLenght("Test");
            a.ShouldBe(13);
        }

        [TestMethod]
        public void GenerateProxy_Call_CreateInstance()
        {
            Mock<IInterceptor> interceptor = new Mock<IInterceptor>(MockBehavior.Strict);
            interceptor.Setup(t => t.Intercept(It.Is<IInvocation>(p => p.MethodName == "CreateSb"), false))
                .Callback<IInvocation, bool>((a, b) =>
                {
                    a.ReturnValue = new StringBuilder("Nanana");
                })
                .Verifiable();

            ProxygGenerator generator = new ProxygGenerator();

            IReturnTypes instance = generator.GenerateProxy<IReturnTypes>(interceptor.Object);
            instance.ShouldNotBeNull();

            StringBuilder sb = instance.CreateSb("Test");
            sb.ShouldNotBeNull();
            sb.ToString().ShouldBe("Nanana");
        }

        [TestMethod]
        public void GenerateProxy_Call_RetStruct()
        {
            Mock<IInterceptor> interceptor = new Mock<IInterceptor>(MockBehavior.Strict);
            interceptor.Setup(t => t.Intercept(It.Is<IInvocation>(p => p.MethodName == "GetStruct"), false))
                .Callback<IInvocation, bool>((a, b) =>
                {
                    MyStruct initVal = MyStruct.InitializeDefault();
                    initVal.Address = 456852;
                    a.ReturnValue = initVal;
                })
                .Verifiable();

            ProxygGenerator generator = new ProxygGenerator();

            IReturnTypes instance = generator.GenerateProxy<IReturnTypes>(interceptor.Object);
            instance.ShouldNotBeNull();

            MyStruct structa = instance.GetStruct();
            structa.Address.ShouldBe(456852);
        }

        [TestMethod]
        public void GenerateProxy_Call_RetVoid()
        {
            Mock<IInterceptor> interceptor = new Mock<IInterceptor>(MockBehavior.Strict);
            interceptor.Setup(t => t.Intercept(It.Is<IInvocation>(p => p.MethodName == "GetVoid"), false))
                .Verifiable();

            ProxygGenerator generator = new ProxygGenerator();

            IReturnTypes instance = generator.GenerateProxy<IReturnTypes>(interceptor.Object);
            instance.ShouldNotBeNull();

            instance.GetVoid();
        }

        [TestMethod]
        public void GenerateProxy_WithoutProperty_ThrowNotImplement()
        {
            Mock<IInterceptor> interceptor = new Mock<IInterceptor>(MockBehavior.Strict);


            ProxygGenerator generator = new ProxygGenerator();

            IGrapth instance = generator.GenerateProxy<IGrapth>(interceptor.Object);
            instance.ShouldNotBeNull();

            ExceptionAssertion.SouldException<NotImplementedException>(() =>
            {
                instance.DisplayName = "New text";
            });
        }

        [TestMethod]
        public void GenerateProxy_WithProperty_SetValue()
        {
            Mock<IInterceptor> interceptor = new Mock<IInterceptor>(MockBehavior.Strict);
            interceptor.Setup(t => t.Intercept(It.IsAny<IInvocation>(), false))
                .Verifiable();

            ProxygGenerator generator = new ProxygGenerator();

            IGrapth instance = generator.GenerateProxy<IGrapth>(interceptor.Object, true);
            instance.ShouldNotBeNull();

            instance.DisplayName = "New text";
        }
    }
}
