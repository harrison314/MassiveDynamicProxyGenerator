using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.TypedProxy
{
    public class TypedProxyInvocation : IInvocation
    {
        private bool isReturnValueInitialized;
        private object[] arguments;
        private Type[] argumentTypes;
        private string methodName;
        private Type originalType;
        private Type returnType;
        private object returnValue;

        public object[] Arguments
        {
            get
            {
                return this.arguments;
            }

            set
            {
                this.arguments = value;
            }
        }

        public Type[] ArgumentTypes
        {
            get
            {
                return this.argumentTypes;
            }

            set
            {
                this.argumentTypes = value;
            }
        }

        public string MethodName
        {
            get
            {
                return this.methodName;
            }

            set
            {
                this.methodName = value;
            }
        }

        public Type OriginalType
        {
            get
            {
                return this.originalType;
            }

            set
            {
                this.originalType = value;
            }
        }

        public object ReturnValue
        {
            get
            {
                if (!this.isReturnValueInitialized)
                {
                    if (this.returnType != typeof(void))
                    {
                        Expression<Func<object>> e = Expression.Lambda<Func<object>>(Expression.Convert(Expression.Default(this.returnType), typeof(object)));
                        this.returnValue = e.Compile().Invoke();
                    }

                    this.isReturnValueInitialized = true;
                }

                return this.returnValue;
            }

            set
            {
                this.returnValue = value;
                this.isReturnValueInitialized = true;
            }
        }

        public Type ReturnType
        {
            get
            {
                return this.returnType;
            }

            set
            {
                this.returnType = value;
            }
        }

        public TypedProxyInvocation()
        {
            this.returnValue = null;
            this.isReturnValueInitialized = false;
        }

        public MethodBase GetConcreteMethod()
        {
            MethodInfo info = this.originalType.GetMethod(this.methodName, this.argumentTypes);
            return info;
        }
    }
}
