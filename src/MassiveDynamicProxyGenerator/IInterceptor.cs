using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator
{
    /// <summary>
    /// Rozhranie pre interceptor používaný Simple injectorom.
    /// </summary>
    /// <remarks>
    /// Inšpirované http://simpleinjector.readthedocs.org/en/latest/advanced.html#interception
    /// </remarks>
    public interface IInterceptor
    {
        /// <summary>
        /// Interceptuje- každé volanie metódy.
        /// </summary>
        /// <param name="invocation">Informácie o volaní.</param>
        /// <param name="isDynamicInterception">if set to <c>true</c> [is dynamic interception].</param>
        void Intercept(IInvocation invocation, bool isDynamicInterception);
    }
}
