using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator
{
    /// <summary>
    /// Interface represented callable invocation informations.
    /// </summary>
    /// <seealso cref="ICommonInvocation" />
    public interface ICallableInvocation : ICommonInvocation
    {
        /// <summary>
        /// Processes intercept method.
        /// </summary>
        void Process();
    }
}
