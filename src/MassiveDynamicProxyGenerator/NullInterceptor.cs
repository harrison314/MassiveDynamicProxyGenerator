using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator
{
    /// <summary>
    /// Null interceptor, returns default values;
    /// </summary>
    /// <seealso cref="MassiveDynamicProxyGenerator.IInterceptor" />
    public class NullInterceptor : IInterceptor
    {
        private static NullInterceptor instance;

        /// <summary>
        /// Gets the singlton instance.
        /// </summary>
        /// <value>
        /// The singlton instance.
        /// </value>
        public static NullInterceptor Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// Initializes static members of the <see cref="NullInterceptor"/> class.
        /// </summary>
        static NullInterceptor()
        {
            instance = new NullInterceptor();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NullInterceptor"/> class.
        /// </summary>
        public NullInterceptor()
        {
        }

        /// <summary>
        /// Intercept call of method. Returns default value.
        /// </summary>
        /// <param name="invocation">Invocation informations.</param>
        /// <param name="isDynamicInterception">if set to <c>true</c> is invocation in dynamic object.</param>
        public void Intercept(IInvocation invocation, bool isDynamicInterception)
        {
            // TODO: implement async calls
        }
    }
}
