using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator
{
    /// <summary>
    /// Adapter converts <see cref="Action{IInvocation, Boolean}"/> to <see cref="IInterceptor"/>.
    /// </summary>
    /// <seealso cref="IInterceptor" />
    public class InterceptorAdapter : IInterceptor
    {
        private readonly Action<IInvocation, bool> intercept;

        /// <summary>
        /// Initializes a new instance of the <see cref="InterceptorAdapter"/> class.
        /// </summary>
        /// <param name="intercept">The intercept action.</param>
        /// <exception cref="ArgumentNullException">intercept</exception>
        public InterceptorAdapter(Action<IInvocation, bool> intercept)
        {
            if (intercept == null)
            {
                throw new ArgumentNullException(nameof(intercept));
            }

            this.intercept = intercept;
        }

        /// <summary>
        /// Intercept call of method.
        /// </summary>
        /// <param name="invocation">Invocation informations.</param>
        /// <param name="isDynamicInterception">if set to <c>true</c> is invocation in dynamic object.</param>
        /// <exception cref="ArgumentNullException">invocation</exception>
        public void Intercept(IInvocation invocation, bool isDynamicInterception)
        {
            if (invocation == null)
            {
                throw new ArgumentNullException(nameof(invocation));
            }

            this.intercept.Invoke(invocation, isDynamicInterception);
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

            return this.intercept.Equals(((InterceptorAdapter)obj).intercept);
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
