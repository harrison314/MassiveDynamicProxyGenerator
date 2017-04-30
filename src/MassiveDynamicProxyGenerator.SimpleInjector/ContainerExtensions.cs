using System;
using MassiveDynamicProxyGenerator;
using MassiveDynamicProxyGenerator.SimpleInjector;

namespace SimpleInjector
{
    /// <summary>
    /// Extensions for SimpleInjector <see cref="Container"/> using MassiveDynamicProxyGenerator.
    /// </summary>
    /// <seealso cref="Container"/>
    /// <seealso cref="ProxygGenerator"/>
    public static class ContainerExtensions
    {
        public static void RegisterMock<TService>(this Container container) 
            where TService : class
        {

            TypeGuards.CheckIsInterface(typeof(TService));
            ProxygGenerator generator = new ProxygGenerator();


#if NET40
            container.Register(typeof(TService), () => generator.GenerateProxy<TService>(new NullInterceptor()));
#else
            container.Register(typeof(TService), () => generator.GenerateProxy<TService>(new NullAsyncInterceptor()));
#endif
        }

        public static void RegisterMock(this Container container, Type mockType)
        {
            if (mockType == null) throw new ArgumentNullException(nameof(mockType));

            TypeGuards.CheckIsInterface(mockType);

            ProxygGenerator generator = new ProxygGenerator();

#if NET40
            container.Register(mockType, () => generator.GenerateProxy(mockType, new NullInterceptor()));
#else
            container.Register(mockType, () => generator.GenerateProxy(mockType, new NullAsyncInterceptor()));
#endif
        }

        public static void RegisterInterceptedpDecorator<TService, TInterceptor>(this Container container)
            where TService:class
            where TInterceptor: ICallableInterceptor
        {
            //ProxygGenerator generator = new ProxygGenerator();

            //container.RegisterDecorator(typeof(TService), (decoratorPredicateConntext, type)=>
            //{
            //    generator.GenerateDecorator<TService>()
            //    },
        }
    }
}
