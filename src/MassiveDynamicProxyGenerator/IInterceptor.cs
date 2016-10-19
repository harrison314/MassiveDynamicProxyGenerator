using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator
{
    /// <summary>
    /// Interface for interceptor.
    /// </summary>
    /// <remarks>
    /// See http://simpleinjector.readthedocs.org/en/latest/advanced.html#interception
    /// </remarks>
    public interface IInterceptor
    {
        /// <summary>
        /// Intercept call of method.
        /// </summary>
        /// <param name="invocation">Invocation informations.</param>
        /// <param name="isDynamicInterception">if set to <c>true</c> is invocation in dynamic object.</param>
        void Intercept(IInvocation invocation, bool isDynamicInterception);
    }
}
