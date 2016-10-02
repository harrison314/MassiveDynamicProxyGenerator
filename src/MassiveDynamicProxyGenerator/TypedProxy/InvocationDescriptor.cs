using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace MassiveDynamicProxyGenerator.TypedProxy
{
    internal class InvocationDescriptor
    {
        public Type Type
        {
            get;
            protected set;
        }

        public PropertyInfo ReturnValue
        {
            get;
            protected set;
        }

        public PropertyInfo ReturnType
        {
            get;
            protected set;
        }

        public PropertyInfo Arguments
        {
            get;
            protected set;
        }

        public PropertyInfo ArgumentTypes
        {
            get;
            protected set;
        }

        public PropertyInfo MethodName
        {
            get;
            protected set;
        }

        public PropertyInfo OriginalType
        {
            get;
            protected set;
        }

        public ConstructorInfo Constructor
        {
            get;
            protected set;
        }

        private InvocationDescriptor(Type type)
        {
            this.Type = type;
            this.ReturnValue = type.GetProperty(nameof(IInvocation.ReturnValue));
            this.Arguments = type.GetProperty(nameof(IInvocation.Arguments));
            this.ArgumentTypes = type.GetProperty(nameof(IInvocation.ArgumentTypes));
            this.MethodName = type.GetProperty(nameof(IInvocation.MethodName));
            this.OriginalType = type.GetProperty(nameof(IInvocation.OriginalType));
            this.ReturnType = type.GetProperty(nameof(IInvocation.ReturnType));
            this.Constructor = type.GetConstructor(new Type[0]);
        }

        public static InvocationDescriptor Create<T>()
            where T : IInvocation, new()
        {
            InvocationDescriptor descriptor = new InvocationDescriptor(typeof(T));
            return descriptor;
        }

        public static InvocationDescriptor Create(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (!typeof(IInvocation).IsAssignableFrom(type))
            {
                string message = string.Format("Type {0} is not {1}", type.FullName, nameof(IInvocation));
                throw new ArgumentNullException(message);
            }

            if (type.GetConstructor(new Type[0]) == null)
            {
                string message = string.Format("Type {0} is must have nonparametric public constructor.", type.FullName);
                throw new ArgumentNullException(message);
            }

            InvocationDescriptor descriptor = new InvocationDescriptor(type);
            return descriptor;
        }
    }
}
