using MassiveDynamicProxyGenerator.Tests.TestInterfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.Tests
{
    [TestClass]
    public class NullInterceptorTests
    {
        [TestMethod]
        public void NUllAsyncInterceptor_NoTask_ReturnDefault()
        {
            ProxygGenerator generator = new ProxygGenerator();
            IReturnTypes instance = generator.GenerateProxy<IReturnTypes>(new NullInterceptor());

            instance.ShouldNotBeNull();

            instance.CreateSb(string.Empty).ShouldBe(default(StringBuilder));
            instance.GetLenght(string.Empty).ShouldBe(default(int));
            instance.GetVoid();
        }
    }
}
