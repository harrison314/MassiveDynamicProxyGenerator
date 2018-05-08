using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MassiveDynamicProxyGenerator.Microsoft.DependencyInjection.ServiceProvider
{
    public class MassiveServiceProvider : IServiceProvider, IDisposable
    {
        private readonly IServiceProvider aspServiceProvider;
        private readonly IProxyGenerator proxygGenerator;
        private readonly IServiceWraperer serviceWraperer;

        protected internal MassiveServiceProvider(IServiceProvider aspServiceProvider, IProxyGenerator proxygGenerator, IServiceWraperer serviceWraperer)
        {
            if (aspServiceProvider == null) throw new ArgumentNullException(nameof(aspServiceProvider));
            if (proxygGenerator == null) throw new ArgumentNullException(nameof(proxygGenerator));
            if (serviceWraperer == null) throw new ArgumentNullException(nameof(serviceWraperer));

            this.aspServiceProvider = aspServiceProvider;
            this.proxygGenerator = proxygGenerator;
            this.serviceWraperer = serviceWraperer;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));

            if (serviceType == typeof(IServiceProvider))
            {
                return this;
            }

            if (serviceType == typeof(IEnumerable<IServiceProvider>))
            {
                return new IServiceProvider[] { this };
            }

            if (serviceType == typeof(IServiceScopeFactory))
            {
                IServiceScopeFactory scopeFactory = this.aspServiceProvider.GetService<IServiceScopeFactory>();
                if (scopeFactory is MassiveScopedServiceFactory)
                {
                    return scopeFactory;
                }
                else
                {
                    return new MassiveScopedServiceFactory(scopeFactory, this.proxygGenerator, this.serviceWraperer);
                }
            }

            object realInstance = this.aspServiceProvider.GetService(serviceType);
            return this.serviceWraperer.ProvideInstance(serviceType, realInstance, this.aspServiceProvider, this.proxygGenerator);
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.aspServiceProvider is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }
    }
}
