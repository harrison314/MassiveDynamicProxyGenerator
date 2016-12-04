using MassiveDynamicProxyGenerator;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApplication.IocExtensions
{
    public static class IocImplementCommonServiceExtension
    {
        public static IServiceCollection ImplementCommonServiceProvider<TService>(this IServiceCollection collection)
            where TService : class
        {
            IProxygGenerator generator = new ProxygGenerator();

            collection.AddTransient<TService>(sp => generator.GenerateProxy<TService>(new ServiceProviderInterceptor(sp)));

            return collection;
        }
    }
}
