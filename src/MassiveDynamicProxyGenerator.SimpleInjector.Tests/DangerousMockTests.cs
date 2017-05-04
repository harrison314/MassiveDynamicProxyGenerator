using System;
using Shouldly;
using SimpleInjector;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MassiveDynamicProxyGenerator.SimpleInjector.Tests.Services;
using MassiveDynamicProxyGenerator.SimpleInjector.Dangerous;

namespace MassiveDynamicProxyGenerator.SimpleInjector.Tests
{
    [TestClass]
    public class DangerousMockTests
    {
        [TestMethod]
        public void MockAllUnregistred_GetTypes_Sucess()
        {
            Container container = new Container();
            container.Register<IMessageService, MessageService>();
            container.Register<ITestMessager, TestMessager>();
            container.RegisterAllUnregistredAsMock();

            container.Verify();

            container.GetInstance<IGenericService<StringComparer>>()
                .ShouldNotBeNull();

            container.GetInstance<ITestMessager>()
               .ShouldNotBeNull();
        }
    }
}
