using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.TypedDecorator
{
    /// <summary>
    /// Internal interceptor descriptor.
    /// </summary>
    internal class CallableInterceptorDescriptor
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public Type Type
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the intercept.
        /// </summary>
        /// <value>
        /// The intercept.
        /// </value>
        public MethodInfo Intercept
        {
            get;
            protected set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CallableInterceptorDescriptor"/> class.
        /// </summary>
        public CallableInterceptorDescriptor()
        {
            this.Type = typeof(ICallableInterceptor);
            this.Intercept = this.Type.GetTypeInfo().GetMethod(nameof(ICallableInterceptor.Intercept), new Type[] { typeof(ICallableInvocation) });
        }
    }
}
