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
    public class DynamicObjectProxyTests
    {
        //[TestMethod]
        //public void GenerateDynamicObjectProxy_CallVoidMethod_Call()
        //{
        //    Mock<IInterceptor> interceptorMoq = new Mock<IInterceptor>(MockBehavior.Strict);
        //    interceptorMoq.Setup(t => t.Intercept(It.IsNotNull<IInvocation>(), true))
        //        .Callback<IInvocation, bool>((invocation, isDynamic) =>
        //        {
        //            invocation.MethodName.ShouldBe("AnyCall");
        //            invocation.ReturnType.ShouldBe(typeof(object));
        //            invocation.Arguments.ShouldBeEmpty();
        //        });

        //    ProxygGenerator generator = new ProxygGenerator();
        //    dynamic proxy = generator.GenerateDynamicObjectProxy(interceptorMoq.Object);

        //    int a = proxy.AnyCall();

        //    interceptorMoq.VerifyAll();
        //}

        //[TestMethod]
        //public void GenerateDynamicObjectProxy_CallReturnMethod_Call()
        //{
        //    throw new NotImplementedException();
        //}

        //[TestMethod]
        //public void GenerateDynamicObjectProxy_CallParameterMethod_Call()
        //{
        //    throw new NotImplementedException();
        //}

        //[TestMethod]
        //public void GenerateDynamicObjectProxy_PropertyGet_Call()
        //{
        //    throw new NotImplementedException();
        //}

        //[TestMethod]
        //public void GenerateDynamicObjectProxy_PropertySet_Call()
        //{
        //    throw new NotImplementedException();
        //}

        //[TestMethod]
        //public void GenerateDynamicObjectProxy_CallMethod_ThrowException()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
