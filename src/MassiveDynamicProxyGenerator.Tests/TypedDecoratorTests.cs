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
    public class TypedDecoratorTests
    {
        [TestMethod]
        public void GenerateGenericDecorator_CallParent_CallParent()
        {
            Mock<INonReturn> parent = new Mock<INonReturn>(MockBehavior.Strict);
            parent.Setup(t => t.EmptyMethod()).Verifiable();

            Mock<ICallableInterceptor> interceptor = new Mock<ICallableInterceptor>(MockBehavior.Strict);
            interceptor.Setup(t => t.Intercept(It.Is<ICallableInvocation>(q => q.MethodName == nameof(INonReturn.EmptyMethod))))
                .Callback<ICallableInvocation>(invocation =>
              {
                  invocation.Process();
              });

            ProxyGenerator generator = new ProxyGenerator();
            INonReturn instance = generator.GenerateDecorator<INonReturn>(interceptor.Object, parent.Object);

            instance.ShouldNotBeNull();

            instance.EmptyMethod();

            parent.VerifyAll();
            interceptor.VerifyAll();
        }

        [TestMethod]
        public void GenerateGenericDecorator_Nop_ReturnDefault()
        {
            Mock<IReturnTypes> parent = new Mock<IReturnTypes>(MockBehavior.Strict);

            Mock<ICallableInterceptor> interceptor = new Mock<ICallableInterceptor>(MockBehavior.Strict);
            interceptor.Setup(t => t.Intercept(It.Is<ICallableInvocation>(q => q.MethodName == nameof(IReturnTypes.GetLength))))
                .Callback<ICallableInvocation>(invocation =>
                {
                });

            ProxyGenerator generator = new ProxyGenerator();
            IReturnTypes instance = generator.GenerateDecorator<IReturnTypes>(interceptor.Object, parent.Object);

            instance.ShouldNotBeNull();

            instance.GetLength("any string").ShouldBe(default(int));

            parent.VerifyAll();
            interceptor.VerifyAll();
        }

        [TestMethod]
        public void GenerateGenericDecorator_Call_TransferParameters()
        {
            string arg1 = "some string";
            Exception arg2 = new InvalidTimeZoneException("Ivalid timezone");
            StringBuilder arg3 = new StringBuilder("op54");
            Func<Exception, string> arg4 = t => t.Message;

            Mock<INonReturn> parent = new Mock<INonReturn>(MockBehavior.Strict);
            parent.Setup(t => t.MoreArguments(arg1, arg2, arg3, arg4)).Verifiable();

            Mock<ICallableInterceptor> interceptor = new Mock<ICallableInterceptor>(MockBehavior.Strict);
            interceptor.Setup(t => t.Intercept(It.Is<ICallableInvocation>(q => q.MethodName == nameof(INonReturn.MoreArguments))))
                .Callback<ICallableInvocation>(invocation =>
                {
                    invocation.Process();
                });

            ProxyGenerator generator = new ProxyGenerator();
            INonReturn instance = generator.GenerateDecorator<INonReturn>(interceptor.Object, parent.Object);

            instance.ShouldNotBeNull();

            instance.MoreArguments(arg1, arg2, arg3, arg4);

            parent.VerifyAll();
            interceptor.VerifyAll();
        }

        [TestMethod]
        public void GenerateGenericDecorator_CallParent_RetValueChange()
        {
            const int retValue = 20;
            const int newReturnValue = 6;

            Mock<IReturnTypes> parent = new Mock<IReturnTypes>(MockBehavior.Strict);
            parent.Setup(t => t.GetLength(It.IsNotNull<string>()))
                .Returns(retValue).Verifiable();

            Mock<ICallableInterceptor> interceptor = new Mock<ICallableInterceptor>(MockBehavior.Strict);
            interceptor.Setup(t => t.Intercept(It.IsNotNull<ICallableInvocation>()))
                .Callback<ICallableInvocation>(invocation =>
                {
                    invocation.Process();
                    invocation.ReturnValue.ShouldBe(retValue);
                    invocation.ReturnValue = newReturnValue;
                });

            ProxyGenerator generator = new ProxyGenerator();
            IReturnTypes instance = generator.GenerateDecorator<IReturnTypes>(interceptor.Object, parent.Object);

            instance.ShouldNotBeNull();

            instance.GetLength("any").ShouldBe(newReturnValue);

            parent.VerifyAll();
            interceptor.VerifyAll();
        }

        [TestMethod]
        public void GenerateGenericDecorator_CallParent_ChangeParameter()
        {
            const int retValue = 20;
            const string parameter = "lorem ipsun dental";

            Mock<IReturnTypes> parent = new Mock<IReturnTypes>(MockBehavior.Strict);
            parent.Setup(t => t.GetLength(parameter))
                .Returns(retValue).Verifiable();

            Mock<ICallableInterceptor> interceptor = new Mock<ICallableInterceptor>(MockBehavior.Strict);
            interceptor.Setup(t => t.Intercept(It.IsNotNull<ICallableInvocation>()))
                .Callback<ICallableInvocation>(invocation =>
                {
                    invocation.Arguments[0] = parameter;
                    invocation.Process();
                });

            ProxyGenerator generator = new ProxyGenerator();
            IReturnTypes instance = generator.GenerateDecorator<IReturnTypes>(interceptor.Object, parent.Object);

            instance.ShouldNotBeNull();

            instance.GetLength("any").ShouldBe(retValue);

            parent.VerifyAll();
            interceptor.VerifyAll();
        }

        [TestMethod]
        public void GenerateGenericDecorator_Generic_CreateInstance()
        {
            StringBuilder sbInstance = new StringBuilder();

            Mock<IGenericInterface<StringBuilder>> realMock = new Mock<IGenericInterface<StringBuilder>>(MockBehavior.Strict);
            realMock.Setup(t => t.Get("any")).Returns(sbInstance).Verifiable();
            realMock.Setup(t => t.PushNew("new", sbInstance)).Verifiable();
            realMock.SetupGet(t => t.Value).Returns(sbInstance).Verifiable();
            realMock.SetupSet(t => t.Value = It.IsNotNull<StringBuilder>()).Verifiable();

            Mock<ICallableInterceptor> interceptor = new Mock<ICallableInterceptor>(MockBehavior.Strict);
            interceptor.Setup(t => t.Intercept(It.IsNotNull<ICallableInvocation>()))
                .Callback<ICallableInvocation>(invocation =>
                {
                    invocation.Process();
                });

            ProxyGenerator generator = new ProxyGenerator();

            IGenericInterface<StringBuilder> instance = generator.GenerateDecorator<IGenericInterface<StringBuilder>>(interceptor.Object, realMock.Object);
            instance.ShouldNotBeNull();

            instance.Get("any").ShouldBeSameAs(sbInstance);
            instance.PushNew("new", sbInstance);
            instance.Value = new StringBuilder(); ;
            instance.Value.ShouldBeSameAs(sbInstance);

            realMock.VerifyAll();
        }
    }
}
