using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator
{
    /// <summary>
    /// Interface representes callable interceptor.
    /// </summary>
    public interface ICallableInterceptor
    {
        /// <summary>
        /// Intercepts the specified invocation.
        /// </summary>
        /// <param name="invocation">The invocation informations.</param>
        void Intercept(ICallableInvocation invocation);
    }
}
