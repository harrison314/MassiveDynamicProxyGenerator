using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator
{
    /// <summary>
    /// Interceptor for mocking types instances.
    /// </summary>
    /// <seealso cref="IInterceptor"/>
    public class MockInterceptor : IInterceptor
    {
        private readonly Func<Type, object> returnValueFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="MockInterceptor"/> class.
        /// </summary>
        /// <param name="returnValues">
        /// The dictionary for mapping return type to value.
        /// If dictionary does not contain return type, then interceptor returns default value of type.
        /// </param>
        /// <exception cref="ArgumentNullException">returnValues</exception>
        public MockInterceptor(Dictionary<Type, object> returnValues)
        {
            if (returnValues == null)
            {
                throw new ArgumentNullException(nameof(returnValues));
            }

            this.returnValueFactory = type => returnValues.TryGetValue(type, out object value) ? value : null;
        }

#if !NET40
        /// <summary>
        /// Initializes a new instance of the <see cref="MockInterceptor"/> class.
        /// </summary>
        /// /// <param name="returnValues">
        /// The dictionary for mapping return type to value.
        /// If dictionary does not contain return type, then interceptor returns default value of type.
        /// </param>
        /// <exception cref="ArgumentNullException">returnValues</exception>
        public MockInterceptor(IReadOnlyDictionary<Type, object> returnValues)
        {
            if (returnValues is null)
            {
                throw new ArgumentNullException(nameof(returnValues));
            }

            this.returnValueFactory = type => returnValues.TryGetValue(type, out object value) ? value : null;
        }
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="MockInterceptor"/> class.
        /// </summary>
        /// <param name="returnValueFactory">
        /// The function return value by type.
        /// If function return <c>null</c> interceptor return default value of type.
        /// </param>
        /// <exception cref="ArgumentNullException">returnValueFactory</exception>
        public MockInterceptor(Func<Type, object> returnValueFactory)
        {
            this.returnValueFactory = returnValueFactory ?? throw new ArgumentNullException(nameof(returnValueFactory));
        }

        /// <summary>
        /// Intercept call of method.
        /// </summary>
        /// <param name="invocation">Invocation informations.</param>
        public void Intercept(IInvocation invocation)
        {
            object returnValue = this.returnValueFactory.Invoke(invocation.ReturnType);
            if (returnValue != null)
            {
                invocation.ReturnValue = returnValue;
            }
        }
    }
}
