using MassiveDynamicProxyGenerator.Tests.TestInterfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Shouldly;

namespace MassiveDynamicProxyGenerator.Tests
{
    [TestClass]
    public class TypedInstanceProxyTests
    {
        [TestMethod]
        public void GenerateInstanceProxy_Call_CreateInstance()
        {
            Mock<IReturnTypes> realMock = new Mock<IReturnTypes>(MockBehavior.Strict);
            realMock.Setup(t => t.GetLenght("789")).Returns(789).Verifiable();
            realMock.Setup(t => t.GetLenght("456")).Returns(456).Verifiable();

            Mock<IInstanceProvicer> instaceprovoder = new Mock<IInstanceProvicer>(MockBehavior.Strict);
            instaceprovoder.Setup(t => t.GetInstance())
                .Returns(realMock.Object)
                .Verifiable();

            ProxygGenerator generator = new ProxygGenerator();

            IReturnTypes instance = generator.GenerateInstanceProxy<IReturnTypes>(instaceprovoder.Object);
            instance.GetLenght("789").ShouldBe(789);
            instance.GetLenght("456").ShouldBe(456);

            instaceprovoder.VerifyAll();
            realMock.VerifyAll();
        }

        [TestMethod]
        public void GenerateInstanceProxy_Call_TransferStruct()
        {
            Mock<IReturnTypes> realMock = new Mock<IReturnTypes>(MockBehavior.Strict);
            realMock.Setup(t => t.GetStruct()).Returns(new MyStruct()
            {
                Address = 45,
                Asci = 'o',
                PointAbc = "hero",
                Pointer = IntPtr.Zero,
                UsLong = 458L
            }).Verifiable();

            Mock<IInstanceProvicer> instaceprovoder = new Mock<IInstanceProvicer>(MockBehavior.Strict);
            instaceprovoder.Setup(t => t.GetInstance())
                .Returns(realMock.Object)
                .Verifiable();

            ProxygGenerator generator = new ProxygGenerator();

            IReturnTypes instance = generator.GenerateInstanceProxy<IReturnTypes>(instaceprovoder.Object);
            instance.GetStruct().UsLong.ShouldBe(458L);

            instaceprovoder.VerifyAll();
            realMock.VerifyAll();
        }

        [TestMethod]
        public void GenerateInstanceProxy_Call_TransferClass()
        {
            StringBuilder sb = new StringBuilder();
            Mock<IReturnTypes> realMock = new Mock<IReturnTypes>(MockBehavior.Strict);
            realMock.Setup(t => t.CreateSb("aaa")).Returns(sb).Verifiable();

            Mock<IInstanceProvicer> instaceprovoder = new Mock<IInstanceProvicer>(MockBehavior.Strict);
            instaceprovoder.Setup(t => t.GetInstance())
                .Returns(realMock.Object)
                .Verifiable();

            ProxygGenerator generator = new ProxygGenerator();

            IReturnTypes instance = generator.GenerateInstanceProxy<IReturnTypes>(instaceprovoder.Object);
            instance.CreateSb("aaa").ShouldBeSameAs(sb);

            instaceprovoder.VerifyAll();
            realMock.VerifyAll();
        }


        [TestMethod]
        public void GenerateInstanceProxy_Call_ReturnVoid()
        {
            Exception ex = new Exception();
            StringBuilder sb = new StringBuilder();
            Func<Exception, string> transform = (e) => e.Message;
            Mock<INonReturn> realMock = new Mock<INonReturn>(MockBehavior.Strict);
            realMock.Setup(t => t.MoreArguments("a", ex, sb, transform)).Verifiable();

            Mock<IInstanceProvicer> instaceProvoder = new Mock<IInstanceProvicer>(MockBehavior.Strict);
            instaceProvoder.Setup(t => t.GetInstance())
                .Returns(realMock.Object)
                .Verifiable();

            ProxygGenerator generator = new ProxygGenerator();

            INonReturn instance = generator.GenerateInstanceProxy<INonReturn>(instaceProvoder.Object);
            instance.MoreArguments("a", ex, sb, transform);

            instaceProvoder.VerifyAll();
            realMock.VerifyAll();
        }

        [TestMethod]
        public void GenerateInstanceProxy_Generic_CreateInstance()
        {
            StringBuilder sbInstance = new StringBuilder();
            Mock<IInterceptor> interceptor = new Mock<IInterceptor>(MockBehavior.Strict);
            interceptor.Setup(t => t.Intercept(It.IsAny<IInvocation>()))
                .Verifiable();

            Mock<IGenericInterface<StringBuilder>> realMock = new Mock<IGenericInterface<StringBuilder>>(MockBehavior.Strict);
            realMock.Setup(t => t.Get("any")).Returns(sbInstance).Verifiable();
            realMock.Setup(t => t.PushNew("new", sbInstance)).Verifiable();
            realMock.SetupGet(t => t.Value).Returns(sbInstance).Verifiable();
            realMock.SetupSet(t => t.Value = It.IsNotNull<StringBuilder>()).Verifiable();

            Mock<IInstanceProvicer> instaceProvoder = new Mock<IInstanceProvicer>(MockBehavior.Strict);
            instaceProvoder.Setup(t => t.GetInstance())
                .Returns(realMock.Object)
                .Verifiable();

            ProxygGenerator generator = new ProxygGenerator();

            IGenericInterface<StringBuilder> instance = generator.GenerateInstanceProxy<IGenericInterface<StringBuilder>>(instaceProvoder.Object);
            instance.ShouldNotBeNull();

            instance.Get("any").ShouldBeSameAs(sbInstance);
            instance.PushNew("new", sbInstance);
            instance.Value = new StringBuilder(); ;
            instance.Value.ShouldBeSameAs(sbInstance);

            realMock.VerifyAll();
        }
    }
}
