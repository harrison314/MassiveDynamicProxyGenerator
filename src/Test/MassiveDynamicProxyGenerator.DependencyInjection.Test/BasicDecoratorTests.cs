using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MassiveDynamicProxyGenerator.DependencyInjection.Test.Services;
using Moq;
using Shouldly;
using MassiveDynamicProxyGenerator.Microsoft.DependencyInjection;

namespace MassiveDynamicProxyGenerator.DependencyInjection.Test
{
    [TestClass]
    public class BasicDecoratorTests
    {
        #region Tested types

        public interface ICallTesting
        {
            void CallInbBase();

            void CallInDecorator();
        }

        public class BaseTypeA : ITypeA
        {
            private readonly ICallTesting callTesting;

            public BaseTypeA(ICallTesting callTesting)
            {
                this.callTesting = callTesting;
            }

            public void FooForA()
            {
                this.callTesting.CallInbBase();
            }
        }

        public class DecoratedTypeA : ITypeA
        {
            private readonly ITypeA parent;
            private readonly ICallTesting callTesting;

            public DecoratedTypeA(ITypeA parent, ICallTesting callTesting)
            {
                this.parent = parent;
                this.callTesting = callTesting;
            }

            public void FooForA()
            {
                this.parent.FooForA();
                this.callTesting.CallInDecorator();
            }
        }

        #endregion

        [TestMethod]
        public void BasicDecorator_DecorateNonGeneric_Success()
        {
            Mock<ICallTesting> callTestingMock = new Mock<ICallTesting>(MockBehavior.Strict);
            callTestingMock.Setup(t => t.CallInbBase()).Verifiable();
            callTestingMock.Setup(t => t.CallInDecorator()).Verifiable();

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<ICallTesting>(callTestingMock.Object);
            serviceCollection.AddTransient<ITypeA, BaseTypeA>();

            serviceCollection.AddDecorator<ITypeA, DecoratedTypeA>();

            IServiceProvider serviceProvider = serviceCollection.BuildIntercepedServiceProvider();

            ITypeA instance = serviceProvider.GetRequiredService<ITypeA>();

            instance.FooForA();

            callTestingMock.VerifyAll();
        }


        #region Generic test types

        public class BaseGenericService<T> : IGenericService<T>
        {
            private readonly ICallTesting callTesting;

            public BaseGenericService(ICallTesting callTesting)
            {
                this.callTesting = callTesting;
            }

            public T GetLast()
            {
                throw new NotImplementedException();
            }

            public T Transform(T item)
            {
                this.callTesting.CallInbBase();

                return item;
            }
        }

        public class DecoratedGenericService<T> : IGenericService<T>
        {
            private readonly IGenericService<T> parent;
            private readonly ICallTesting callTesting;

            public DecoratedGenericService(IGenericService<T> parent, ICallTesting callTesting)
            {
                this.parent = parent;
                this.callTesting = callTesting;
            }

            public T GetLast()
            {
                throw new NotImplementedException();
            }

            public T Transform(T item)
            {
                T porcessed = this.parent.Transform(item);
                this.callTesting.CallInDecorator();

                return porcessed;
            }
        }
        #endregion

        [TestMethod]
        public void BasicDecorator_DecorateGeneric_Throw()
        {
            Mock<ICallTesting> callTestingMock = new Mock<ICallTesting>(MockBehavior.Strict);
            callTestingMock.Setup(t => t.CallInbBase()).Verifiable();
            callTestingMock.Setup(t => t.CallInDecorator()).Verifiable();

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<ICallTesting>(callTestingMock.Object);
            serviceCollection.AddTransient(typeof(IGenericService<>), typeof(BaseGenericService<>));

            Action action = new Action(() => serviceCollection.AddDecorator(typeof(IGenericService<>), typeof(DecoratedGenericService<>)));

            action.ShouldThrow<ArgumentException>();

            // serviceCollection.AddDecorator(typeof(IGenericService<>), typeof(DecoratedGenericService<>));

            //IServiceProvider serviceProvider = serviceCollection.BuldIntercepedServiceProvider();

            //IGenericService<int> instance = serviceProvider.GetRequiredService<IGenericService<int>>();

            //instance.Transform(458).ShouldBe(458);

            //callTestingMock.VerifyAll();
        }
    }
}
