using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator
{
    /// <summary>
    /// Interface represented invocation information`s.
    /// </summary>
    /// <seealso cref="ICommonInvocation"/>
    public interface IInvocation : ICommonInvocation
    {
        /// <summary>
        /// Processes intercept method on <paramref name="instance"/>.
        /// </summary>
        /// <param name="instance">Instance of object when by call method.</param>
        void Process(object instance);
    }
}
