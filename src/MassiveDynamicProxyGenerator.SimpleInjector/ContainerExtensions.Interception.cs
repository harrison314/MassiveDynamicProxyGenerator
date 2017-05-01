using MassiveDynamicProxyGenerator;
using System;
using System.Collections.Generic;
using System.Text;
using SimpleInjector;

namespace SimpleInjector
{
    public static partial class ContainerExtensions
    {
        public static void RegisterInterceptedpDecorator<TService, TInterceptor>(this Container container)
            where TService : class
            where TInterceptor : ICallableInterceptor
        {
            //ProxygGenerator generator = new ProxygGenerator();

            //container.RegisterDecorator(typeof(TService), (decoratorPredicateConntext, type)=>
            //{
            //    generator.GenerateDecorator<TService>()
            //    },
        }
    }
}
