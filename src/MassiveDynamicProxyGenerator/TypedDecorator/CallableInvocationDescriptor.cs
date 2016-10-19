using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.TypedDecorator
{
    /// <summary>
    /// Descriptor for <see cref="ICallableInvocation"/>.
    /// </summary>
    internal class CallableInvocationDescriptor
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
        /// Gets or sets the return value.
        /// </summary>
        /// <value>
        /// The return value.
        /// </value>
        public PropertyInfo ReturnValue
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the type of the return.
        /// </summary>
        /// <value>
        /// The type of the return.
        /// </value>
        public PropertyInfo ReturnType
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the arguments.
        /// </summary>
        /// <value>
        /// The arguments.
        /// </value>
        public PropertyInfo Arguments
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the argument types.
        /// </summary>
        /// <value>
        /// The argument types.
        /// </value>
        public PropertyInfo ArgumentTypes
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the name of the method.
        /// </summary>
        /// <value>
        /// The name of the method.
        /// </value>
        public PropertyInfo MethodName
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the type of the original.
        /// </summary>
        /// <value>
        /// The type of the original.
        /// </value>
        public PropertyInfo OriginalType
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the constructor.
        /// </summary>
        /// <value>
        /// The constructor.
        /// </value>
        public ConstructorInfo Constructor
        {
            get;
            protected set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CallableInvocationDescriptor"/> class.
        /// </summary>
        public CallableInvocationDescriptor()
        {
            Type type = typeof(CallableInvocation);
            this.Type = type;
            this.ReturnValue = type.GetProperty(nameof(ICallableInvocation.ReturnValue));
            this.Arguments = type.GetProperty(nameof(ICallableInvocation.Arguments));
            this.ArgumentTypes = type.GetProperty(nameof(ICallableInvocation.ArgumentTypes));
            this.MethodName = type.GetProperty(nameof(ICallableInvocation.MethodName));
            this.OriginalType = type.GetProperty(nameof(ICallableInvocation.OriginalType));
            this.ReturnType = type.GetProperty(nameof(ICallableInvocation.ReturnType));
            this.Constructor = type.GetConstructor(new Type[] { typeof(Action<ICallableInvocation>) });
        }
    }
}
