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
    public class MockInterceptorTests
    {
        [TestMethod]
        public void MockInterceptor_ReturnInDictionary_ReturnDefault()
        {
            ProxyGenerator generator = new ProxyGenerator();
            IReturnTypes instance = generator.GenerateProxy<IReturnTypes>(new MockInterceptor(new Dictionary<Type, object>()));

            instance.ShouldNotBeNull();

            instance.CreateSb(string.Empty).ShouldBe(default(StringBuilder));
            instance.GetLength(string.Empty).ShouldBe(default(int));
            instance.GetVoid();
        }

        [TestMethod]
        public void MockInterceptor_ReturnInFunction_ReturnDefault()
        {
            ProxyGenerator generator = new ProxyGenerator();
            IReturnTypes instance = generator.GenerateProxy<IReturnTypes>(new MockInterceptor(type => null));

            instance.ShouldNotBeNull();

            instance.CreateSb(string.Empty).ShouldBe(default(StringBuilder));
            instance.GetLength(string.Empty).ShouldBe(default(int));
            instance.GetVoid();
        }

        [TestMethod]
        public void MockInterceptor_ReturnInDictionary_ReturnInt()
        {
            Dictionary<Type, object> mockValues = new Dictionary<Type, object>()
            {
                { typeof(int), 42 }
            };

            ProxyGenerator generator = new ProxyGenerator();
            IReturnTypes instance = generator.GenerateProxy<IReturnTypes>(new MockInterceptor(mockValues));

            instance.ShouldNotBeNull();

            instance.CreateSb(string.Empty).ShouldBe(default(StringBuilder));
            instance.GetLength(string.Empty).ShouldBe(42);
            instance.GetVoid();
        }

        [TestMethod]
        public void MockInterceptor_ReturnInFunction_ReturnInt()
        {
            Func<Type, object> retValueFactory = type =>
            {
                if (type == typeof(int))
                {
                    return 42;
                }

                return null;
            };

            ProxyGenerator generator = new ProxyGenerator();
            IReturnTypes instance = generator.GenerateProxy<IReturnTypes>(new MockInterceptor(retValueFactory));

            instance.ShouldNotBeNull();

            instance.CreateSb(string.Empty).ShouldBe(default(StringBuilder));
            instance.GetLength(string.Empty).ShouldBe(42);
            instance.GetVoid();
        }

        [TestMethod]
        public void MockInterceptor_ReturnInDictionary_ReturnStringBuilder()
        {
            StringBuilder defaultSb = new StringBuilder("hello");
            Dictionary<Type, object> mockValues = new Dictionary<Type, object>()
            {
                { typeof(StringBuilder), defaultSb }
            };

            ProxyGenerator generator = new ProxyGenerator();
            IReturnTypes instance = generator.GenerateProxy<IReturnTypes>(new MockInterceptor(mockValues));

            instance.ShouldNotBeNull();

            instance.CreateSb(string.Empty).ShouldBe(defaultSb);
            instance.GetLength(string.Empty).ShouldBe(default(int));
            instance.GetVoid();
        }

        [TestMethod]
        public void MockInterceptor_ReturnInFunction_ReturnStringBuilder()
        {
            StringBuilder defaultSb = new StringBuilder("hello");
            Func<Type, object> retValueFactory = type =>
            {
                if (type == typeof(StringBuilder))
                {
                    return defaultSb;
                }

                return null;
            };

            ProxyGenerator generator = new ProxyGenerator();
            IReturnTypes instance = generator.GenerateProxy<IReturnTypes>(new MockInterceptor(retValueFactory));

            instance.ShouldNotBeNull();

            instance.CreateSb(string.Empty).ShouldBe(defaultSb);
            instance.GetLength(string.Empty).ShouldBe(default(int));
            instance.GetVoid();
        }
    }
}
