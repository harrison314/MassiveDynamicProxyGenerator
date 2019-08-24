using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator
{
    /// <summary>
    /// Common members for all type of invocations.
    /// </summary>
    public interface ICommonInvocation
    {
        /// <summary>
        /// Gets or sets the return value of intercept method.
        /// </summary>
        /// <value>
        /// The return value of intercept method.
        /// </value>
        object ReturnValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the arguments of intercept method.
        /// </summary>
        /// <value>
        /// The arguments of intercept method.
        /// </value>
        object[] Arguments
        {
            get;
        }

        /// <summary>
        /// Gets the type of the origin interface.
        /// </summary>
        /// <value>
        /// The type of the origin interface.
        /// </value>
        Type OriginalType
        {
            get;
        }

        /// <summary>
        /// Gets the name of the intercept method.
        /// </summary>
        /// <value>
        /// The name of the intercept method.
        /// </value>
        string MethodName
        {
            get;
        }

        /// <summary>
        /// Gets the types of arguments.
        /// </summary>
        /// <value>
        /// The argument types of arguments.
        /// </value>
        Type[] ArgumentTypes
        {
            get;
        }

        /// <summary>
        /// Gets the type of the return.
        /// </summary>
        /// <value>
        /// The type of the return.
        /// </value>
        Type ReturnType
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="MethodBase"/> as representation of intercept method.
        /// </summary>
        /// <returns>The <see cref="MethodBase"/> of intercept method.</returns>
        MethodBase GetConcreteMethod();
    }
}
