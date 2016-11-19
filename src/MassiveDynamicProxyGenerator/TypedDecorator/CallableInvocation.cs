using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.TypedDecorator
{
    /// <summary>
    /// Class representes callable invocation.
    /// </summary>
    /// <seealso cref="MassiveDynamicProxyGenerator.ICallableInvocation" />
    public class CallableInvocation : ICallableInvocation
    {
        private readonly Action<ICallableInvocation> processAction;
        private bool isReturnValueInitialized;
        private object[] arguments;
        private Type[] argumentTypes;
        private string methodName;
        private Type originalType;
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
        /// Gets or sets the types of arguments.
        /// </summary>
        /// <value>
        /// The argument types of arguments.
        /// </value>
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
        /// Gets or sets the type of the origin interface.
        /// </summary>
        /// <value>
        /// The type of the origin interface.
        /// </value>
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
        /// Initializes a new instance of the <see cref="CallableInvocation"/> class.
        /// </summary>
        /// <param name="processAction">The process action.</param>
        /// <exception cref="ArgumentNullException">processAction</exception>
        public CallableInvocation(Action<ICallableInvocation> processAction)
        {
            if (processAction == null)
            {
                throw new ArgumentNullException(nameof(processAction));
            }

            this.returnValue = null;
            this.isReturnValueInitialized = false;
            this.processAction = processAction;
        }

        /// <summary>
        /// Gets the <see cref="MethodBase" /> as representation of intecept method.
        /// </summary>
        /// <returns>
        /// The <see cref="MethodBase" /> of intercept method.
        /// </returns>
        public MethodBase GetConcreteMethod()
        {
            MethodInfo info = this.originalType.GetTypeInfo().GetMethod(this.methodName, this.argumentTypes);

            return info;
        }

        /// <summary>
        /// Processes intercept method.
        /// </summary>
        public void Process()
        {
            this.processAction.Invoke(this);
        }
    }
}
