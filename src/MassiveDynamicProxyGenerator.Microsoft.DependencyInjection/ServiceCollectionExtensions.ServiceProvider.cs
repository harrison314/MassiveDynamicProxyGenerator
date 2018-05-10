using MassiveDynamicProxyGenerator.Microsoft.DependencyInjection.ServiceProvider;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
        public static IServiceProvider BuildIntercepedServiceProvider(this IServiceCollection services)
        {
            services.TryAddTransient(typeof(IOriginalService<>), typeof(OriginalService<>));
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
        public static IServiceProvider BuildIntercepedServiceProvider(this IServiceCollection services, bool validateScopes)
        {
            services.TryAddTransient(typeof(IOriginalService<>), typeof(OriginalService<>));
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
        public static IServiceProvider BuildIntercepedServiceProvider(this IServiceCollection services, ServiceProviderOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            services.TryAddTransient(typeof(IOriginalService<>), typeof(OriginalService<>));
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

        /// <summary>
        /// Creates an <see cref="System.IServiceProvider" /> containing services from the provided <see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection" /> with custom service wraperer.
        /// </summary>
        /// <param name="services">The <see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection" /> containing service descriptors.</param>
        /// <param name="serviceWraperer">The service wraperer.</param>
        /// <returns>
        /// The <see cref="IServiceProvider" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">serviceWraperer</exception>
        public static IServiceProvider BuildServiceProviderWithWraperer(this IServiceCollection services, IServiceWraperer serviceWraperer)
        {
            if (serviceWraperer == null) throw new ArgumentNullException(nameof(serviceWraperer));

            services.TryAddTransient(typeof(IOriginalService<>), typeof(OriginalService<>));
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            MassiveServiceProvider massiveServiceProvider = new MassiveServiceProvider(serviceProvider,
                MassiveDynamicProxyGeneratorDiSettings.ProxyGeneratorProvider.GetProxyGenerator(serviceProvider),
                serviceWraperer);

            return massiveServiceProvider;
        }
    }
}
