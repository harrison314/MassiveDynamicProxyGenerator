﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using MassiveDynamicProxyGenerator.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using MassiveDynamicProxyGenerator.DependencyInjection.Test.Services;
using Moq;
using Shouldly;

namespace MassiveDynamicProxyGenerator.DependencyInjection.Test
{
    [TestClass]
    public class AddProxyTests
    {
        [TestMethod]
        public void AddProxy_GenericInstance_Register()
        {
            ServiceCollection serviceCollection = new ServiceCollection();

            bool isCall = false;
            InterceptorAdapter interceptor = new InterceptorAdapter(intercept =>
            {
                isCall = true;
                intercept.MethodName.ShouldBe("Send");

            });

            serviceCollection.AddProxy<IMessageService>(interceptor);

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            IMessageService typeA = serviceProvider.GetRequiredService<IMessageService>();
            typeA.ShouldNotBeNull();

            typeA.Send("some@email.com", "body");
            isCall.ShouldBeTrue("Interceptor can not call.");
        }

        [TestMethod]
        public void AddProxy_GenericAction_Register()
        {
            ServiceCollection serviceCollection = new ServiceCollection();

            bool isCall = false;

            serviceCollection.AddProxy<IMessageService>(intercept =>
            {
                isCall = true;
                intercept.MethodName.ShouldBe("Send");

            });

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            IMessageService typeA = serviceProvider.GetRequiredService<IMessageService>();
            typeA.ShouldNotBeNull();

            typeA.Send("some@email.com", "body");
            isCall.ShouldBeTrue("Interceptor can not call.");
        }

        [TestMethod]
        public void AddProxy_GenericWithNullInterceptor_Register()
        {
            ServiceCollection serviceCollection = new ServiceCollection();


            serviceCollection.AddProxy<IMessageService>();

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            IMessageService typeA = serviceProvider.GetRequiredService<IMessageService>();
            typeA.ShouldNotBeNull();
        }
    }
}
