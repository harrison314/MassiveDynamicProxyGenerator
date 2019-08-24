using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.DynamicProxy
{
    /// <summary>
    /// proxy object for dynamic interception.
    /// </summary>
    /// <seealso cref="System.Dynamic.DynamicObject" />
    internal class DynamicProxyObject : DynamicObject
    {
        private static readonly FieldInfo CallSiteBinderCache = typeof(CallSiteBinder).GetTypeInfo().GetField("Cache", BindingFlags.NonPublic | BindingFlags.Instance);

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
            base.TryInvokeMember(binder, args, out result);
            invocation.ReturnType = binder.ReturnType;
            invocation.ReturnType = BindingType(binder);

            // TODO: refactor to another interception this.interceptor.Intercept(invocation);

            result = invocation.ReturnValue;

            return true;
        }

        private static Type BindingType(CallSiteBinder binder)
        {
            IDictionary<Type, object> cache = (IDictionary<Type, object>)CallSiteBinderCache.GetValue(binder);
            Type ftype = cache.Select(t => t.Key).FirstOrDefault(t =>
            t != null
            && t.GetTypeInfo().IsGenericType
            && t.GetTypeInfo().GetGenericTypeDefinition() == typeof(Func<,,>));
            if (ftype == null)
            {
                return null;
            }

            Type[] genargs = ftype.GetTypeInfo().GetGenericArguments();
            return genargs[2];
        }
    }
}
