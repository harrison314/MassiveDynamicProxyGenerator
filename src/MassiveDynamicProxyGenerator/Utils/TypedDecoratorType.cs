using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.Utils
{
    /// <summary>
    /// Decorator type.
    /// </summary>
    internal enum TypedDecoratorType : int
    {
        /// <summary>
        /// The typed proxy.
        /// </summary>
        TypedProxy = 1,

        /// <summary>
        /// The typed proxy with parameters.
        /// </summary>
        TypedProxyWithParameters = 2,

        /// <summary>
        /// The typed instanced proxy.
        /// </summary>
        TypedInstancedProxy = 3,

        /// <summary>
        /// The typed decorator.
        /// </summary>
        TypedDecorator = 4
    }
}
