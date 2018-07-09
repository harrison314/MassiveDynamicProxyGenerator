using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using MassiveDynamicProxyGenerator;
using MassiveDynamicProxyGenerator.SimpleInjector;
using SimpleInjector;

namespace MassiveDynamicProxyGenerator.SimpleInjector.Dangerous
{
    public static partial class DangerousContainerExtensions
    {
        /// <summary>
        /// Register mocks for all unregistered types in container.
        /// This operation use only design mode or tests.
        /// </summary>
        /// <param name="container">The container.</param>
        public static void RegisterAllUnregisteredAsMock(this Container container)
        {
            IProxyGenerator generator = ProxyGeneratorFactory.Factory.GetInstance();

            container.ResolveUnregisteredType += (sender, arguments) =>
            {
                if (!arguments.Handled && arguments.UnregisteredServiceType.GetTypeInfo().IsInterface)
                {
#if NET40
                    arguments.Register(() => generator.GenerateProxy(arguments.UnregisteredServiceType, new NullInterceptor()));
#else
                    arguments.Register(() => generator.GenerateProxy(arguments.UnregisteredServiceType, new NullAsyncInterceptor()));
#endif
                }
            };
        }
    }
}
