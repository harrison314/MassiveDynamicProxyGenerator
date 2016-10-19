using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.TypedDecorator
{
    /// <summary>
    /// Internal generate union.
    /// </summary>
    internal class GenerateUnion
    {
        /// <summary>
        /// Gets the process method.
        /// </summary>
        /// <value>
        /// The process method.
        /// </value>
        public MethodInfo ProcessMethod
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateUnion"/> class.
        /// </summary>
        /// <param name="methodInfo">The method information.</param>
        /// <exception cref="ArgumentNullException">methodInfo</exception>
        public GenerateUnion(MethodInfo methodInfo)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }

            this.ProcessMethod = methodInfo;
        }
    }
}
