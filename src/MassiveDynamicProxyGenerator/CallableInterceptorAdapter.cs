using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator
{
    /// <summary>
    /// Adapter converts <see cref="Action{ICallableInvocation}"/> to <see cref="ICallableInterceptor"/>.
    /// </summary>
    /// <seealso cref="MassiveDynamicProxyGenerator.ICallableInterceptor" />
    public class CallableInterceptorAdapter : ICallableInterceptor
    {
        private readonly Action<ICallableInvocation> intercept;

        /// <summary>
        /// Initializes a new instance of the <see cref="CallableInterceptorAdapter"/> class.
        /// </summary>
        /// <param name="intercept">The intercept.</param>
        /// <exception cref="ArgumentNullException">intercept</exception>
        public CallableInterceptorAdapter(Action<ICallableInvocation> intercept)
        {
            if (intercept == null)
            {
                throw new ArgumentNullException(nameof(intercept));
            }

            this.intercept = intercept;
        }

        /// <summary>
        /// Intercepts the specified invocation.
        /// </summary>
        /// <param name="invocation">The invocation informations.</param>
        /// <exception cref="ArgumentNullException">invocation</exception>
        public void Intercept(ICallableInvocation invocation)
        {
            if (invocation == null)
            {
                throw new ArgumentNullException(nameof(invocation));
            }

            this.intercept.Invoke(invocation);
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }

            return this.intercept.Equals(((CallableInterceptorAdapter)obj).intercept);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return this.intercept.GetHashCode() + 1;
            }
        }
    }
}
