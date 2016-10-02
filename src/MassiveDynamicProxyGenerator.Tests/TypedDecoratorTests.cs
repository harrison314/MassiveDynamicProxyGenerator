using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Should;
using MassiveDynamicProxyGenerator.Tests.TestInterfaces;

namespace MassiveDynamicProxyGenerator.Tests
{
    [TestClass]
    public class TypedDecoratorTests
    {
        [TestMethod]
        public void GenerateDecorator_CallParent_CallParent()
        {
            Mock<INonReturn> parent = new Mock<INonReturn>(MockBehavior.Strict);
            parent.Setup(t => t.EmptyMethod()).Verifiable();

            Mock<ICallableInterceptor> interceptor = new Mock<ICallableInterceptor>(MockBehavior.Strict);
            interceptor.Setup(t => t.Intercept(It.Is<ICallableInvocation>(q => q.MethodName == nameof(INonReturn.EmptyMethod))))
                .Callback<ICallableInvocation>(invocation =>
              {
                  invocation.Process();
              });

            ProxygGenerator generator = new ProxygGenerator();
            INonReturn instance = generator.GenerateDecorator<INonReturn>(interceptor.Object, parent.Object);

            instance.ShouldNotBeNull();

            instance.EmptyMethod();

            parent.VerifyAll();
            interceptor.VerifyAll();
        }

        [TestMethod]
        public void GenerateDecorator_Nop_ReturnDefault()
        {
            Mock<IReturnTypes> parent = new Mock<IReturnTypes>(MockBehavior.Strict);

            Mock<ICallableInterceptor> interceptor = new Mock<ICallableInterceptor>(MockBehavior.Strict);
            interceptor.Setup(t => t.Intercept(It.Is<ICallableInvocation>(q => q.MethodName == nameof(IReturnTypes.GetLenght))))
                .Callback<ICallableInvocation>(invocation =>
                {
                });

            ProxygGenerator generator = new ProxygGenerator();
            IReturnTypes instance = generator.GenerateDecorator<IReturnTypes>(interceptor.Object, parent.Object);

            instance.ShouldNotBeNull();

            instance.GetLenght("any string").ShouldEqual(default(int));

            parent.VerifyAll();
            interceptor.VerifyAll();
        }

        [TestMethod]
        public void GenerateDecorator_Call_TransferParameters()
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

            ProxygGenerator generator = new ProxygGenerator();
            INonReturn instance = generator.GenerateDecorator<INonReturn>(interceptor.Object, parent.Object);

            instance.ShouldNotBeNull();

            instance.MoreArguments(arg1, arg2, arg3, arg4);

            parent.VerifyAll();
            interceptor.VerifyAll();
        }

        [TestMethod]
        public void GenerateDecorator_CallParent_RetValueChange()
        {
            const int retValue = 20;
            const int newReturnValue = 6;

            Mock<IReturnTypes> parent = new Mock<IReturnTypes>(MockBehavior.Strict);
            parent.Setup(t => t.GetLenght(It.IsNotNull<string>()))
                .Returns(retValue).Verifiable();

            Mock<ICallableInterceptor> interceptor = new Mock<ICallableInterceptor>(MockBehavior.Strict);
            interceptor.Setup(t => t.Intercept(It.IsNotNull<ICallableInvocation>()))
                .Callback<ICallableInvocation>(invocation =>
                {
                    invocation.Process();
                    invocation.ReturnValue.ShouldEqual(retValue);
                    invocation.ReturnValue = newReturnValue;
                });

            ProxygGenerator generator = new ProxygGenerator();
            IReturnTypes instance = generator.GenerateDecorator<IReturnTypes>(interceptor.Object, parent.Object);

            instance.ShouldNotBeNull();

            instance.GetLenght("any").ShouldEqual(newReturnValue);

            parent.VerifyAll();
            interceptor.VerifyAll();
        }

        [TestMethod]
        public void GenerateDecorator_Callparent_ChangeParameter()
        {
            const int retValue = 20;
            const string parameter = "lorem ipsun dental";

            Mock<IReturnTypes> parent = new Mock<IReturnTypes>(MockBehavior.Strict);
            parent.Setup(t => t.GetLenght(parameter))
                .Returns(retValue).Verifiable();

            Mock<ICallableInterceptor> interceptor = new Mock<ICallableInterceptor>(MockBehavior.Strict);
            interceptor.Setup(t => t.Intercept(It.IsNotNull<ICallableInvocation>()))
                .Callback<ICallableInvocation>(invocation =>
                {
                    invocation.Arguments[0] = parameter;
                    invocation.Process();
                });

            ProxygGenerator generator = new ProxygGenerator();
            IReturnTypes instance = generator.GenerateDecorator<IReturnTypes>(interceptor.Object, parent.Object);

            instance.ShouldNotBeNull();

            instance.GetLenght("any").ShouldEqual(retValue);

            parent.VerifyAll();
            interceptor.VerifyAll();
        }
    }
}
