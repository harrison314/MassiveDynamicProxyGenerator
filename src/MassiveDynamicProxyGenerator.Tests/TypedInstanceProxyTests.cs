using MassiveDynamicProxyGenerator.Tests.TestInterfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Should;

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
            instance.GetLenght("789").ShouldEqual(789);
            instance.GetLenght("456").ShouldEqual(456);

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
            instance.GetStruct().UsLong.ShouldEqual(458L);

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

            Mock<IInstanceProvicer> instaceprovoder = new Mock<IInstanceProvicer>(MockBehavior.Strict);
            instaceprovoder.Setup(t => t.GetInstance())
                .Returns(realMock.Object)
                .Verifiable();

            ProxygGenerator generator = new ProxygGenerator();

            INonReturn instance = generator.GenerateInstanceProxy<INonReturn>(instaceprovoder.Object);
            instance.MoreArguments("a", ex, sb, transform);

            instaceprovoder.VerifyAll();
            realMock.VerifyAll();
        }
    }
}
