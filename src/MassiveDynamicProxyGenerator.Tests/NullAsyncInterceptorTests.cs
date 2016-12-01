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
    public class NullAsyncInterceptorTests
    {
        [TestMethod]
        public void NUllAsyncInterceptor_NoTask_ReturnDefault()
        {
            ProxygGenerator generator = new ProxygGenerator();
            IReturnTypes instance = generator.GenerateProxy<IReturnTypes>(new NullAsyncInterceptor());

            instance.ShouldNotBeNull();

            instance.CreateSb(string.Empty).ShouldBe(default(StringBuilder));
            instance.GetLenght(string.Empty).ShouldBe(default(int));
            instance.GetVoid();
        }

        [TestMethod]
        public void NUllAsyncInterceptor_NoTask_Void()
        {
            ProxygGenerator generator = new ProxygGenerator();
            IReturnTypes instance = generator.GenerateProxy<IReturnTypes>(new NullAsyncInterceptor());

            instance.ShouldNotBeNull();

            instance.GetVoid();
        }

        [TestMethod]
        public async Task NUllAsyncInterceptor_Task_Async()
        {
            Mock<IGrapth> graphMock = new Mock<IGrapth>(MockBehavior.Strict);

            ProxygGenerator generator = new ProxygGenerator();
            IAsyncInterface instance = generator.GenerateProxy<IAsyncInterface>(new NullAsyncInterceptor());

            instance.ShouldNotBeNull();

            Task task = instance.WriteContextAsync(graphMock.Object);
            task.ShouldNotBeNull();

            await task;
        }

        [TestMethod]
        public async Task NUllAsyncInterceptor_TaskGeneric_Async()
        {
            Mock<IGrapth> graphMock = new Mock<IGrapth>(MockBehavior.Strict);

            ProxygGenerator generator = new ProxygGenerator();
            IAsyncInterface instance = generator.GenerateProxy<IAsyncInterface>(new NullAsyncInterceptor());

            instance.ShouldNotBeNull();

            Task<int> task1 = instance.GetCountAsync();
            task1.ShouldNotBeNull();
            int value1 = await task1;
            value1.ShouldBe(default(int));

            Task<IGrapth> task2 = instance.ReadContextAsync();
            task2.ShouldNotBeNull();
            IGrapth value2 = await task2;
            value2.ShouldBe(default(IGrapth));
        }
    }
}
