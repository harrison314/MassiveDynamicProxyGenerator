using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace MassiveDynamicProxyGenerator.TypedProxy
{
    /// <summary>
    /// Descriptor for <see cref="IInvocation"/>.
    /// </summary>
    internal class InvocationDescriptor
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

        private InvocationDescriptor(Type type)
        {
            this.Type = type;
            var typeInfo = type.GetTypeInfo();
            this.ReturnValue = typeInfo.GetProperty(nameof(IInvocation.ReturnValue));
            this.Arguments = typeInfo.GetProperty(nameof(IInvocation.Arguments));
            this.ArgumentTypes = typeInfo.GetProperty(nameof(IInvocation.ArgumentTypes));
            this.MethodName = typeInfo.GetProperty(nameof(IInvocation.MethodName));
            this.OriginalType = typeInfo.GetProperty(nameof(IInvocation.OriginalType));
            this.ReturnType = typeInfo.GetProperty(nameof(IInvocation.ReturnType));
            this.Constructor = typeInfo.GetConstructors().Single();
        }

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <typeparam name="T">Type of invocation descriptor.</typeparam>
        /// <returns>Instance of invocation descriptor.</returns>
        public static InvocationDescriptor Create<T>()
            where T : IInvocation
        {
            InvocationDescriptor descriptor = new InvocationDescriptor(typeof(T));
            return descriptor;
        }

        /// <summary>
        /// Creates the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Invocation descriptor.</returns>
        /// <exception cref="ArgumentNullException">
        /// type
        /// </exception>
        public static InvocationDescriptor Create(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (!typeof(IInvocation).GetTypeInfo().IsAssignableFrom(type))
            {
                throw new ArgumentNullException($"Type {type.FullName} is not {nameof(IInvocation)}.");
            }

            if (type.GetTypeInfo().GetConstructor(Type.EmptyTypes) == null)
            {
                throw new ArgumentNullException($"Type {type.FullName} is must have nonparametric public constructor.");
            }

            InvocationDescriptor descriptor = new InvocationDescriptor(type);
            return descriptor;
        }
    }
}
