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
        /// Adds the original service.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <returns>
        /// The <see cref="IServiceCollection" /> to add the service to.
        /// </returns>
        public static IServiceCollection AddOriginalService(this IServiceCollection services)
        {
            services.AddTransient(typeof(IOriginalService<>), typeof(OriginalService<>));
            return services;
        }
    }
}
