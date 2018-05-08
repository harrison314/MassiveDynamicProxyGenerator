using MassiveDynamicProxyGenerator.Microsoft.DependencyInjection.ServiceProvider;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.Microsoft.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Creates an <see cref="System.IServiceProvider"/> containing services from the provided <see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection"/> with interception.
        /// </summary>
        /// <param name="services">The <see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection"/> containing service descriptors.</param>
        /// <returns>The <see cref="IServiceProvider"/>.</returns>
        public static IServiceProvider BuldIntercepedServiceProvider(this IServiceCollection services)
        {
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            Registrations registrations = serviceProvider.GetService(typeof(Registrations)) as Registrations;
            if (registrations == null || registrations.Count == 0)
            {
                return serviceProvider;
            }

            MassiveServiceProvider massiveServiceProvider = new MassiveServiceProvider(serviceProvider,
                MassiveDynamicProxyGeneratorDiSettings.ProxyGeneratorProvider.GetProxyGenerator(serviceProvider),
                registrations);

            return massiveServiceProvider;
        }

        /// <summary>
        ///  Creates an <see cref="System.IServiceProvider"/> containing services from the provided <see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection"/> with interception.
        /// </summary>
        /// <param name="services">The <see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection"/> containing service descriptors.</param>
        /// <param name="validateScopes"><c>true</c> to perform check verifying that scoped services never gets resolved from root provider; otherwise <c>false</c>.</param>
        /// <returns>The <see cref="IServiceProvider"/>.</returns>
        public static IServiceProvider BuldIntercepedServiceProvider(this IServiceCollection services, bool validateScopes)
        {
            IServiceProvider serviceProvider = services.BuildServiceProvider(validateScopes);
            Registrations registrations = serviceProvider.GetService(typeof(Registrations)) as Registrations;
            if (registrations == null || registrations.Count == 0)
            {
                return serviceProvider;
            }

            MassiveServiceProvider massiveServiceProvider = new MassiveServiceProvider(serviceProvider,
                MassiveDynamicProxyGeneratorDiSettings.ProxyGeneratorProvider.GetProxyGenerator(serviceProvider),
                registrations);

            return massiveServiceProvider;
        }

        /// <summary>
        /// Creates an <see cref="System.IServiceProvider" /> containing services from the provided <see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection" /> with interception.
        /// </summary>
        /// <param name="services">The <see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection" /> containing service descriptors.</param>
        /// <param name="options">Configures various service provider behaviors.</param>
        /// <returns>
        /// The <see cref="IServiceProvider" />.
        /// </returns>
        public static IServiceProvider BuldIntercepedServiceProvider(this IServiceCollection services, ServiceProviderOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            IServiceProvider serviceProvider = services.BuildServiceProvider(options);
            Registrations registrations = serviceProvider.GetService(typeof(Registrations)) as Registrations;
            if (registrations == null || registrations.Count == 0)
            {
                return serviceProvider;
            }

            MassiveServiceProvider massiveServiceProvider = new MassiveServiceProvider(serviceProvider,
                MassiveDynamicProxyGeneratorDiSettings.ProxyGeneratorProvider.GetProxyGenerator(serviceProvider),
                registrations);

            return massiveServiceProvider;
        }
    }
}
