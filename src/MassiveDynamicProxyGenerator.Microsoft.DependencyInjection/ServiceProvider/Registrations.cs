using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.Microsoft.DependencyInjection.ServiceProvider
{
    internal class Registrations : IServiceWraperer
    {
        internal class Registration
        {
            public Predicate<Type> Filter;
            public Func<IServiceProvider, ICallableInterceptor> InterceptorFactory;
        }

        private readonly Dictionary<Type, Type> closedTypes;
        private readonly Dictionary<Type, Type> openGeneriksTypes;
        private readonly List<Registration> predicatRegistrations;

        public int Count
        {
            get => this.closedTypes.Count + this.openGeneriksTypes.Count + this.predicatRegistrations.Count;
        }

        public Registrations()
        {
            this.closedTypes = new Dictionary<Type, Type>();
            this.openGeneriksTypes = new Dictionary<Type, Type>();
            this.predicatRegistrations = new List<Registration>();
        }

        public void Add(Type interfaceType, Type interceptorType)
        {
            if (TypeHelper.IsOpenGeneric(interfaceType))
            {
                this.openGeneriksTypes.Add(interfaceType, interceptorType);
            }
            else
            {
                this.closedTypes.Add(interfaceType, interceptorType);
            }
        }

        public void Add(Predicate<Type> predicate, Func<IServiceProvider, ICallableInterceptor> factory)
        {
            this.predicatRegistrations.Add(new Registration()
            {
                Filter = predicate,
                InterceptorFactory = factory
            });
        }

        public object ProvideInstance(Type serviceType, object realInstance, IServiceProvider aspServiceProvider, IProxyGenerator proxyGenerator)
        {
            if (realInstance == null)
            {
                return null;
            }

            if (serviceType.IsInterface)
            {
                object instance = realInstance;
                if (this.closedTypes.TryGetValue(serviceType, out Type interceptorType))
                {
                    ICallableInterceptor interceptor = (ICallableInterceptor)ActivatorUtilities.CreateInstance(aspServiceProvider, interceptorType);

                    instance = proxyGenerator.GenerateDecorator(serviceType, interceptor, instance);
                }

                if (serviceType.GetTypeInfo().IsGenericType)
                {
                    if (this.openGeneriksTypes.TryGetValue(serviceType.GetGenericTypeDefinition(), out Type genericInterceptorType))
                    {
                        ICallableInterceptor interceptor = (ICallableInterceptor)ActivatorUtilities.CreateInstance(aspServiceProvider, genericInterceptorType);

                        instance = proxyGenerator.GenerateDecorator(serviceType, interceptor, instance);
                    }
                }

                foreach (Registration registration in this.predicatRegistrations)
                {
                    ICallableInterceptor interceptor = registration.InterceptorFactory(aspServiceProvider);
                    instance = proxyGenerator.GenerateDecorator(serviceType, interceptor, instance);
                }

                return instance;
            }

            return realInstance;
        }
    }
}
