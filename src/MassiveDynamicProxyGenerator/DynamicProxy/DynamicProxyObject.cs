using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.DynamicProxy
{
    /// <summary>
    /// proxy object for dynmaic interception.
    /// </summary>
    /// <seealso cref="System.Dynamic.DynamicObject" />
    public class DynamicProxyObject : DynamicObject
    {
        private readonly IInterceptor interceptor;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicProxyObject"/> class.
        /// </summary>
        /// <param name="interceptor">The interceptor.</param>
        /// <exception cref="ArgumentNullException">interceptor</exception>
        public DynamicProxyObject(IInterceptor interceptor)
        {
            if (interceptor == null)
            {
                throw new ArgumentNullException(nameof(interceptor));
            }

            this.interceptor = interceptor;
        }

        /// <summary>
        /// Provides the implementation for operations that invoke a member. Classes derived from the <see cref="T:System.Dynamic.DynamicObject" /> class can override this method to specify dynamic behavior for operations such as calling a method.
        /// </summary>
        /// <param name="binder">Provides information about the dynamic operation. The binder.Name property provides the name of the member on which the dynamic operation is performed. For example, for the statement sampleObject.SampleMethod(100), where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject" /> class, binder.Name returns "SampleMethod". The binder.IgnoreCase property specifies whether the member name is case-sensitive.</param>
        /// <param name="args">The arguments that are passed to the object member during the invoke operation. For example, for the statement sampleObject.SampleMethod(100), where sampleObject is derived from the <see cref="T:System.Dynamic.DynamicObject" /> class, <paramref name="args" /> is equal to 100.</param>
        /// <param name="result">The result of the member invocation.</param>
        /// <returns>
        /// true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a language-specific run-time exception is thrown.)
        /// </returns>
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            DynamicInvocation invocation = new DynamicInvocation();
            invocation.Arguments = args;
            invocation.MethodName = binder.Name;
            invocation.ReturnType = binder.ReturnType;

            this.interceptor.Intercept(invocation, true);

            result = invocation.ReturnValue;

            return true;
        }
    }
}
