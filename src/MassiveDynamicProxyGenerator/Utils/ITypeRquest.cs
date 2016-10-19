using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.Utils
{
    /// <summary>
    /// Instance representes type request for cahce.
    /// </summary>
    internal interface ITypeRquest
    {
        /// <summary>
        /// Gets the type of the decorator.
        /// </summary>
        /// <value>
        /// The type of the decorator.
        /// </value>
        TypedDecoratorType DecoratorType
        {
            get;
        }

        /// <summary>
        /// Gets the interface types.
        /// </summary>
        /// <value>
        /// The interface types.
        /// </value>
        Type[] InterfaceTypes
        {
            get;
        }
    }
}
