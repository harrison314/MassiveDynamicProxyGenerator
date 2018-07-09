using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator
{
#if NET40
    /// <summary>
    /// Null interceptor, returns default values.
    /// </summary>
    /// <seealso cref="MassiveDynamicProxyGenerator.IInterceptor" />
#else
    /// <summary>
    /// Null interceptor, returns default values.
    /// This interceptor is preferred in the .Net 4.0, in more framework vesions use <see cref="NullAsyncInterceptor"/>.
    /// </summary>
    /// <seealso cref="MassiveDynamicProxyGenerator.IInterceptor" />
#endif
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
        public void Intercept(IInvocation invocation)
        {
        }
    }
}
