using MassiveDynamicProxyGenerator.Tests.TestInterfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;

namespace MassiveDynamicProxyGenerator.Tests
{
    [TestClass]
    public class CallableInterceptorAsyncAdapterTests
    {

        #region Synchronious method

        [TestMethod]
        public void CallableInterceptorAsyncAdapter_SynchoniosMethod_Sucess()
        {
            object invocationMetaData = new object();
            Mock<IMethodWraper> methodWraper = new Mock<IMethodWraper>();
            methodWraper.Setup(t => t.OnEnterInvoke(It.IsNotNull<ICallableInvocation>()))
                .Callback<ICallableInvocation>(invocation =>
                {
                    invocation.MethodName.ShouldBe("GetCount");
                }).Returns(invocationMetaData);

            methodWraper.Setup(t => t.OnExitInvoke(It.IsNotNull<ICallableInvocation>(), invocationMetaData))
                .Callback<ICallableInvocation, object>((invocation, _) =>
                {
                    invocation.MethodName.ShouldBe("GetCount");
                });

            Mock<IAsyncInterface> asyncInterfaceMock = new Mock<IAsyncInterface>(MockBehavior.Strict);
            asyncInterfaceMock.Setup(t => t.GetCount()).Returns(5);

            ICallableInterceptor interceptor = new CallableInterceptorAsyncImpl(methodWraper.Object);

            ProxyGenerator generator = new ProxyGenerator();

            IAsyncInterface instance = generator.GenerateDecorator<IAsyncInterface>(interceptor, asyncInterfaceMock.Object);

            instance.GetCount().ShouldBe(5);

            methodWraper.VerifyAll();
            asyncInterfaceMock.VerifyAll();
        }

        [TestMethod]
        public void CallableInterceptorAsyncAdapter_SynchoniosMethod_HasnleException()
        {
            object invocationMetaData = new object();
            Mock<IMethodWraper> methodWraper = new Mock<IMethodWraper>();
            methodWraper.Setup(t => t.OnEnterInvoke(It.IsNotNull<ICallableInvocation>()))
                .Callback<ICallableInvocation>(invocation =>
                {
                    invocation.MethodName.ShouldBe("GetCount");
                }).Returns(invocationMetaData);

            methodWraper.Setup(t => t.HandleException(It.IsNotNull<ICallableInvocation>(), It.IsNotNull<Exception>(), invocationMetaData))
                .Callback<ICallableInvocation, Exception, object>((invocation, ex, _) =>
                {
                    ex.ShouldBeOfType<InvalidTimeZoneException>();
                    invocation.MethodName.ShouldBe("GetCount");
                    invocation.ReturnValue = 5;
                }).Returns(true);

            Mock<IAsyncInterface> asyncInterfaceMock = new Mock<IAsyncInterface>(MockBehavior.Strict);
            asyncInterfaceMock.Setup(t => t.GetCount()).Throws(new InvalidTimeZoneException("some error"));

            ICallableInterceptor interceptor = new CallableInterceptorAsyncImpl(methodWraper.Object);

            ProxyGenerator generator = new ProxyGenerator();

            IAsyncInterface instance = generator.GenerateDecorator<IAsyncInterface>(interceptor, asyncInterfaceMock.Object);

            instance.GetCount().ShouldBe(5);

            methodWraper.VerifyAll();
            asyncInterfaceMock.VerifyAll();
        }

        [TestMethod]
        public void CallableInterceptorAsyncAdapter_SynchoniosMethod_RetrowException()
        {
            object invocationMetaData = new object();
            Mock<IMethodWraper> methodWraper = new Mock<IMethodWraper>();
            methodWraper.Setup(t => t.OnEnterInvoke(It.IsNotNull<ICallableInvocation>()))
                .Callback<ICallableInvocation>(invocation =>
                {
                    invocation.MethodName.ShouldBe("GetCount");
                }).Returns(invocationMetaData);

            methodWraper.Setup(t => t.HandleException(It.IsNotNull<ICallableInvocation>(), It.IsNotNull<Exception>(), invocationMetaData))
                .Throws(new InvalidProgramException("onw"));

            Mock<IAsyncInterface> asyncInterfaceMock = new Mock<IAsyncInterface>(MockBehavior.Strict);
            asyncInterfaceMock.Setup(t => t.GetCount()).Throws(new InvalidTimeZoneException("some error"));

            ICallableInterceptor interceptor = new CallableInterceptorAsyncImpl(methodWraper.Object);

            ProxyGenerator generator = new ProxyGenerator();

            IAsyncInterface instance = generator.GenerateDecorator<IAsyncInterface>(interceptor, asyncInterfaceMock.Object);

            ExceptionAssertion.SouldException<InvalidProgramException>(() => instance.GetCount());

            methodWraper.VerifyAll();
            asyncInterfaceMock.VerifyAll();
        }

        #endregion

        #region Task Method

        [TestMethod]
        public async Task CallableInterceptorAsyncAdapter_TaskMethod_Sucess()
        {
            object invocationMetaData = new object();
            IGrapth graph = (new Mock<IGrapth>().Object);
            Mock<IMethodWraper> methodWraper = new Mock<IMethodWraper>();
            methodWraper.Setup(t => t.OnEnterInvoke(It.IsNotNull<ICallableInvocation>()))
                .Callback<ICallableInvocation>(invocation =>
                {
                    invocation.MethodName.ShouldBe("WriteContextAsync");
                }).Returns(invocationMetaData);

            methodWraper.Setup(t => t.OnExitInvoke(It.IsNotNull<ICallableInvocation>(), invocationMetaData))
                .Callback<ICallableInvocation, object>((invocation, _) =>
                {
                    invocation.MethodName.ShouldBe("WriteContextAsync");
                });

            Mock<IAsyncInterface> asyncInterfaceMock = new Mock<IAsyncInterface>(MockBehavior.Strict);
            asyncInterfaceMock.Setup(t => t.WriteContextAsync(graph)).Returns(Task.CompletedTask);

            ICallableInterceptor interceptor = new CallableInterceptorAsyncImpl(methodWraper.Object);

            ProxyGenerator generator = new ProxyGenerator();

            IAsyncInterface instance = generator.GenerateDecorator<IAsyncInterface>(interceptor, asyncInterfaceMock.Object);

            await instance.WriteContextAsync(graph);

            methodWraper.VerifyAll();
            asyncInterfaceMock.VerifyAll();
        }

        [TestMethod]
        public async Task CallableInterceptorAsyncAdapter_TaskMethod_HasnleException()
        {
            object invocationMetaData = new object();
            IGrapth graph = (new Mock<IGrapth>().Object);

            Mock<IMethodWraper> methodWraper = new Mock<IMethodWraper>();
            methodWraper.Setup(t => t.OnEnterInvoke(It.IsNotNull<ICallableInvocation>()))
                .Callback<ICallableInvocation>(invocation =>
                {
                    invocation.MethodName.ShouldBe("WriteContextAsync");
                }).Returns(invocationMetaData);

            methodWraper.Setup(t => t.HandleException(It.IsNotNull<ICallableInvocation>(), It.IsNotNull<Exception>(), invocationMetaData))
                .Callback<ICallableInvocation, Exception, object>((invocation, ex, _) =>
                {
                    ex.ShouldBeOfType<InvalidTimeZoneException>();
                    invocation.MethodName.ShouldBe("WriteContextAsync");
                }).Returns(true);

            Mock<IAsyncInterface> asyncInterfaceMock = new Mock<IAsyncInterface>(MockBehavior.Strict);
            asyncInterfaceMock.Setup(t => t.WriteContextAsync(graph)).Returns(this.ThrowAsyncException());

            ICallableInterceptor interceptor = new CallableInterceptorAsyncImpl(methodWraper.Object);

            ProxyGenerator generator = new ProxyGenerator();

            IAsyncInterface instance = generator.GenerateDecorator<IAsyncInterface>(interceptor, asyncInterfaceMock.Object);

            await instance.WriteContextAsync(graph);

            methodWraper.VerifyAll();
            asyncInterfaceMock.VerifyAll();
        }

        [TestMethod]
        public async Task CallableInterceptorAsyncAdapter_TaskMethod_RetrowException()
        {
            object invocationMetaData = new object();
            IGrapth graph = (new Mock<IGrapth>().Object);

            Mock<IMethodWraper> methodWraper = new Mock<IMethodWraper>();
            methodWraper.Setup(t => t.OnEnterInvoke(It.IsNotNull<ICallableInvocation>()))
                .Callback<ICallableInvocation>(invocation =>
                {
                    invocation.MethodName.ShouldBe("WriteContextAsync");
                }).Returns(invocationMetaData);

            methodWraper.Setup(t => t.HandleException(It.IsNotNull<ICallableInvocation>(), It.IsNotNull<Exception>(), invocationMetaData))
                .Throws(new InvalidProgramException("onw"));

            Mock<IAsyncInterface> asyncInterfaceMock = new Mock<IAsyncInterface>(MockBehavior.Strict);
            asyncInterfaceMock.Setup(t => t.WriteContextAsync(graph)).Throws(new InvalidTimeZoneException("some error"));

            ICallableInterceptor interceptor = new CallableInterceptorAsyncImpl(methodWraper.Object);

            ProxyGenerator generator = new ProxyGenerator();

            IAsyncInterface instance = generator.GenerateDecorator<IAsyncInterface>(interceptor, asyncInterfaceMock.Object);

            await ExceptionAssertion.SouldExceptionAsync<InvalidProgramException>(() => instance.WriteContextAsync(graph));

            methodWraper.VerifyAll();
            asyncInterfaceMock.VerifyAll();
        }

        #endregion

        #region Task generic Method


        [TestMethod]
        public async Task CallableInterceptorAsyncAdapter_TaskGenericMethod_Sucess()
        {
            object invocationMetaData = new object();
            IGrapth graph = (new Mock<IGrapth>().Object);
            Mock<IMethodWraper> methodWraper = new Mock<IMethodWraper>();
            methodWraper.Setup(t => t.OnEnterInvoke(It.IsNotNull<ICallableInvocation>()))
                .Callback<ICallableInvocation>(invocation =>
                {
                    invocation.MethodName.ShouldBe("ReadContextAsync");
                }).Returns(invocationMetaData);

            methodWraper.Setup(t => t.OnExitInvoke(It.IsNotNull<ICallableInvocation>(), invocationMetaData))
                .Callback<ICallableInvocation, object>((invocation, _) =>
                {
                    invocation.MethodName.ShouldBe("ReadContextAsync");
                });

            Mock<IAsyncInterface> asyncInterfaceMock = new Mock<IAsyncInterface>(MockBehavior.Strict);
            asyncInterfaceMock.Setup(t => t.ReadContextAsync()).Returns(Task.FromResult(graph));

            ICallableInterceptor interceptor = new CallableInterceptorAsyncImpl(methodWraper.Object);

            ProxyGenerator generator = new ProxyGenerator();

            IAsyncInterface instance = generator.GenerateDecorator<IAsyncInterface>(interceptor, asyncInterfaceMock.Object);

            IGrapth graphResult = await instance.ReadContextAsync();
            graphResult.ShouldBe(graph);

            methodWraper.VerifyAll();
            asyncInterfaceMock.VerifyAll();
        }

        [TestMethod]
        public async Task CallableInterceptorAsyncAdapter_TaskGenericMethod_HasnleException()
        {
            object invocationMetaData = new object();
            IGrapth graph = (new Mock<IGrapth>().Object);

            Mock<IMethodWraper> methodWraper = new Mock<IMethodWraper>();
            methodWraper.Setup(t => t.OnEnterInvoke(It.IsNotNull<ICallableInvocation>()))
                .Callback<ICallableInvocation>(invocation =>
                {
                    invocation.MethodName.ShouldBe("ReadContextAsync");
                }).Returns(invocationMetaData);

            methodWraper.Setup(t => t.HandleException(It.IsNotNull<ICallableInvocation>(), It.IsNotNull<Exception>(), invocationMetaData))
                .Callback<ICallableInvocation, Exception, object>((invocation, ex, _) =>
                {
                    ex.ShouldBeOfType<InvalidTimeZoneException>();
                    invocation.MethodName.ShouldBe("ReadContextAsync");
                }).Returns(true);

            Mock<IAsyncInterface> asyncInterfaceMock = new Mock<IAsyncInterface>(MockBehavior.Strict);
            asyncInterfaceMock.Setup(t => t.ReadContextAsync()).Returns(this.ThrowsAsyncGenericException());

            ICallableInterceptor interceptor = new CallableInterceptorAsyncImpl(methodWraper.Object);

            ProxyGenerator generator = new ProxyGenerator();

            IAsyncInterface instance = generator.GenerateDecorator<IAsyncInterface>(interceptor, asyncInterfaceMock.Object);

            IGrapth graphResult = await instance.ReadContextAsync();
            graphResult.ShouldBe(default(IGrapth));

            methodWraper.VerifyAll();
            asyncInterfaceMock.VerifyAll();
        }

        [TestMethod]
        public async Task CallableInterceptorAsyncAdapter_TaskGenericMethod_RetrowException()
        {
            object invocationMetaData = new object();
            IGrapth graph = (new Mock<IGrapth>().Object);

            Mock<IMethodWraper> methodWraper = new Mock<IMethodWraper>();
            methodWraper.Setup(t => t.OnEnterInvoke(It.IsNotNull<ICallableInvocation>()))
                .Callback<ICallableInvocation>(invocation =>
                {
                    invocation.MethodName.ShouldBe("ReadContextAsync");
                }).Returns(invocationMetaData);

            methodWraper.Setup(t => t.HandleException(It.IsNotNull<ICallableInvocation>(), It.IsNotNull<Exception>(), invocationMetaData))
                .Throws(new InvalidProgramException("onw"));

            Mock<IAsyncInterface> asyncInterfaceMock = new Mock<IAsyncInterface>(MockBehavior.Strict);
            asyncInterfaceMock.Setup(t => t.ReadContextAsync()).Throws(new InvalidTimeZoneException("some error"));

            ICallableInterceptor interceptor = new CallableInterceptorAsyncImpl(methodWraper.Object);

            ProxyGenerator generator = new ProxyGenerator();

            IAsyncInterface instance = generator.GenerateDecorator<IAsyncInterface>(interceptor, asyncInterfaceMock.Object);

            await ExceptionAssertion.SouldExceptionAsync<InvalidProgramException>(async () => await instance.ReadContextAsync());

            methodWraper.VerifyAll();
            asyncInterfaceMock.VerifyAll();
        }

        #endregion

        #region Test supression call

        [TestMethod]
        public async Task CallableInterceptorAsyncAdapter_TaskGenericMethod_SupressParent()
        {
            object invocationMetaData = new object();
            IGrapth graph = (new Mock<IGrapth>().Object);
            Mock<IMethodWraper> methodWraper = new Mock<IMethodWraper>();
            methodWraper.Setup(t => t.OnEnterInvoke(It.IsNotNull<ICallableInvocation>()))
                .Callback<ICallableInvocation>(invocation =>
                {
                    invocation.MethodName.ShouldBe("ReadContextAsync");
                    invocation.ReturnValue = Task.FromResult(graph);
                }).Returns(invocationMetaData);

            methodWraper.Setup(t => t.OnExitInvoke(It.IsNotNull<ICallableInvocation>(), invocationMetaData));

            Mock<IAsyncInterface> asyncInterfaceMock = new Mock<IAsyncInterface>(MockBehavior.Strict);

            ICallableInterceptor interceptor = new CallableInterceptorAsyncImpl(methodWraper.Object);

            ProxyGenerator generator = new ProxyGenerator();

            IAsyncInterface instance = generator.GenerateDecorator<IAsyncInterface>(interceptor, asyncInterfaceMock.Object);

            IGrapth graphResult = await instance.ReadContextAsync();
            graphResult.ShouldBe(graph);

            methodWraper.VerifyAll();
            asyncInterfaceMock.VerifyAll();
        }
        #endregion

        private async Task ThrowAsyncException()
        {
            await Task.Delay(50);
            throw new InvalidTimeZoneException("async exception");
        }

        private async Task<IGrapth> ThrowsAsyncGenericException()
        {
            await Task.Delay(50);
            throw new InvalidTimeZoneException("async generic exception");
        }
    }
}
