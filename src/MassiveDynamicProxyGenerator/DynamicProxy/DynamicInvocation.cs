using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.DynamicProxy
{
    public class DynamicInvocation : IInvocation
    {
        private object[] arguments;
        private Type[] argumentTypes;
        private string methodName;
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
                if (this.argumentTypes == null)
                {
                    // TODO: lock
                    this.argumentTypes = new Type[this.arguments.Length];
                    for (int i = 0; i < this.arguments.Length; i++)
                    {
                        if (this.arguments[i] == null)
                        {
                            this.argumentTypes[i] = typeof(object);
                        }
                        else
                        {
                            this.argumentTypes[i] = this.arguments[i].GetType();
                        }
                    }
                }

                return this.argumentTypes;
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
                return typeof(DynamicObject);
            }
        }

        public object ReturnValue
        {
            get
            {
                return this.returnValue;
            }

            set
            {
                this.returnValue = value;
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

        public DynamicInvocation()
        {
            this.returnValue = null;
            this.argumentTypes = null;
        }

        public MethodBase GetConcreteMethod()
        {
            return null;
        }
    }
}
