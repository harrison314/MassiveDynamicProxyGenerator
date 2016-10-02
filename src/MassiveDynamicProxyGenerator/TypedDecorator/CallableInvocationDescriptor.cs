using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.TypedDecorator
{
    internal class CallableInvocationDescriptor
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
