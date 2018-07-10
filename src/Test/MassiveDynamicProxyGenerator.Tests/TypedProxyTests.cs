﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public void GenerateProxy_NonParametric_Succ()
        {
            Mock<IInterceptor> interceptor = new Mock<IInterceptor>(MockBehavior.Strict);
            interceptor.Setup(t => t.Intercept(It.Is<IInvocation>(q => q.Arguments.Length == 0 && q.MethodName == "EmptyMethod")))
                .Verifiable();

            ProxyGenerator generator = new ProxyGenerator();

            INonReturn instance = generator.GenerateProxy<INonReturn>(interceptor.Object);
            instance.ShouldNotBeNull();

            instance.EmptyMethod();

            interceptor.VerifyAll();
        }

        [TestMethod]
        public void GenerateProxy_ValueTypeParameter_Succ()
        {
            Mock<IInterceptor> interceptor = new Mock<IInterceptor>(MockBehavior.Strict);
            interceptor.Setup(t => t.Intercept(It.Is<IInvocation>(q => q.Arguments.Length == 1 &&
            ((int)q.Arguments[0]) == 12
            && q.MethodName == "OneArgument")))
                .Verifiable();

            ProxyGenerator generator = new ProxyGenerator();

            INonReturn instance = generator.GenerateProxy<INonReturn>(interceptor.Object);
            instance.ShouldNotBeNull();

            instance.OneArgument(12);

            interceptor.VerifyAll();
        }

        [TestMethod]
        public void GenerateProxy_RefTypeParameter_Succ()
        {
            StringBuilder sbExcepted = new StringBuilder("klmn");
            Mock<IInterceptor> interceptor = new Mock<IInterceptor>(MockBehavior.Strict);
            interceptor.Setup(t => t.Intercept(It.Is<IInvocation>(q => q.Arguments.Length == 1 &&
            ((StringBuilder)q.Arguments[0]).Equals(sbExcepted)
            && q.MethodName == "OneArgument")))
                .Verifiable();

            ProxyGenerator generator = new ProxyGenerator();

            INonReturn instance = generator.GenerateProxy<INonReturn>(interceptor.Object);
            instance.ShouldNotBeNull();

            instance.OneArgument(sbExcepted);

            interceptor.VerifyAll();
        }

        [TestMethod]
        public void GenerateProxy_ReturnValueType_Succ()
        {
            Mock<IInterceptor> interceptor = new Mock<IInterceptor>(MockBehavior.Strict);
            interceptor.Setup(t => t.Intercept(It.Is<IInvocation>(p => p.MethodName == "GetLength")))
                .Callback<IInvocation>((invocation) =>
                {
                    invocation.ReturnValue = 13;
                })
                .Verifiable();
            ProxyGenerator generator = new ProxyGenerator();

            IReturnTypes instance = generator.GenerateProxy<IReturnTypes>(interceptor.Object);
            instance.ShouldNotBeNull();

            int a = instance.GetLength("Test");
            a.ShouldBe(13);
        }

        [TestMethod]
        public void GenerateProxy_Call_CreateInstance()
        {
            Mock<IInterceptor> interceptor = new Mock<IInterceptor>(MockBehavior.Strict);
            interceptor.Setup(t => t.Intercept(It.Is<IInvocation>(p => p.MethodName == "CreateSb")))
                .Callback<IInvocation>((a) =>
                {
                    a.ReturnValue = new StringBuilder("Nanana");
                })
                .Verifiable();

            ProxyGenerator generator = new ProxyGenerator();

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
            interceptor.Setup(t => t.Intercept(It.Is<IInvocation>(p => p.MethodName == "GetStruct")))
                .Callback<IInvocation>((a) =>
                {
                    MyStruct initVal = MyStruct.InitializeDefault();
                    initVal.Address = 456852;
                    a.ReturnValue = initVal;
                })
                .Verifiable();

            ProxyGenerator generator = new ProxyGenerator();

            IReturnTypes instance = generator.GenerateProxy<IReturnTypes>(interceptor.Object);
            instance.ShouldNotBeNull();

            MyStruct structa = instance.GetStruct();
            structa.Address.ShouldBe(456852);
        }

        [TestMethod]
        public void GenerateProxy_Call_RetVoid()
        {
            Mock<IInterceptor> interceptor = new Mock<IInterceptor>(MockBehavior.Strict);
            interceptor.Setup(t => t.Intercept(It.Is<IInvocation>(p => p.MethodName == "GetVoid")))
                .Verifiable();

            ProxyGenerator generator = new ProxyGenerator();

            IReturnTypes instance = generator.GenerateProxy<IReturnTypes>(interceptor.Object);
            instance.ShouldNotBeNull();

            instance.GetVoid();
        }

        [TestMethod]
        public void GenerateProxy_WithoutProperty_ThrowNotImplement()
        {
            Mock<IInterceptor> interceptor = new Mock<IInterceptor>(MockBehavior.Strict);

            ProxyGenerator generator = new ProxyGenerator();

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
            interceptor.Setup(t => t.Intercept(It.IsAny<IInvocation>()))
                .Verifiable();

            ProxyGenerator generator = new ProxyGenerator();

            IGrapth instance = generator.GenerateProxy<IGrapth>(interceptor.Object, true);
            instance.ShouldNotBeNull();

            instance.DisplayName = "New text";
        }


        [TestMethod]
        public void GenerateProxy_Generic_CreateInstance()
        {
            Mock<IInterceptor> interceptor = new Mock<IInterceptor>(MockBehavior.Strict);
            interceptor.Setup(t => t.Intercept(It.IsAny<IInvocation>()))
                .Verifiable();

            ProxyGenerator generator = new ProxyGenerator();

            IGenericInterface<long> instance = generator.GenerateProxy<IGenericInterface<long>>(interceptor.Object, true);
            instance.ShouldNotBeNull();

            instance.Get("any name").ShouldBe(default(long));
            instance.PushNew("new", 156L);
            instance.Value = 45;
            instance.Value.ShouldBe(default(long));
        }
    }
}