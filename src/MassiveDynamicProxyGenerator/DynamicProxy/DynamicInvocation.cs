using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.DynamicProxy
{
    /// <summary>
    /// Class representes dynamic invocation.
    /// </summary>
    /// <seealso cref="MassiveDynamicProxyGenerator.IInvocation" />
    internal class DynamicInvocation : IInvocation
    {
        private object[] arguments;
        private Type[] argumentTypes;
        private string methodName;
        private Type returnType;
        private object returnValue;

        /// <summary>
        /// Gets or sets the arguments of intercept method.
        /// </summary>
        /// <value>
        /// The arguments of intercept method.
        /// </value>
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

        /// <summary>
        /// Gets the types of arguments.
        /// </summary>
        /// <value>
        /// The argument types of arguments.
        /// </value>
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

        /// <summary>
        /// Gets or sets the name of the intercept method.
        /// </summary>
        /// <value>
        /// The name of the tntercept method.
        /// </value>
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

        /// <summary>
        /// Gets the type of the origin interface.
        /// </summary>
        /// <value>
        /// The type of the origin interface.
        /// </value>
        public Type OriginalType
        {
            get
            {
                return typeof(DynamicObject);
            }
        }

        /// <summary>
        /// Gets or sets the return value of intercept method.
        /// </summary>
        /// <value>
        /// The return value of intercept method.
        /// </value>
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

        /// <summary>
        /// Gets or sets the type of the return.
        /// </summary>
        /// <value>
        /// The type of the return.
        /// </value>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicInvocation"/> class.
        /// </summary>
        public DynamicInvocation()
        {
            this.returnValue = null;
            this.argumentTypes = null;
        }

        /// <summary>
        /// Gets the <see cref="MethodBase" /> as representation of intecept method. Retusrns <c>null</c>.
        /// </summary>
        /// <returns>
        /// Allwais <c>null</c>. It's dynamic invocation.
        /// </returns>
        public MethodBase GetConcreteMethod()
        {
            return null;
        }
    }
}
