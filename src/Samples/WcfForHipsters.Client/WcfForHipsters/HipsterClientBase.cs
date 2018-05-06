using MassiveDynamicProxyGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WcfForHipsters.Client.WcfForHipsters
{
    public abstract class HipsterClientBase<T> where T : class
    {
        protected T Channal
        {
            get;
            private set;
        }

        public Uri Endpoint
        {
            get;
            private set;
        }

        public HipsterClientBase(Uri endpoint)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException(nameof(endpoint));
            }

            //TODO: check type of T - must by interface with unique method names.

            this.Endpoint = endpoint;

            ProxyGenerator proxygenerator = new ProxyGenerator();
            this.Channal = proxygenerator.GenerateProxy<T>(new JsonRpcInterceptor(endpoint));
        }

        public HipsterClientBase(string endpointUrl)
            : this(new Uri(endpointUrl))
        {
            if (endpointUrl == null)
            {
                throw new ArgumentNullException(nameof(endpointUrl));
            }
        }
    }
}
