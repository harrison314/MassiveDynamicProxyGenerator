using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator
{
    /// <summary>
    /// Interface representes creator for name of type.
    /// </summary>
    public interface ITypeNameCreator
    {
        /// <summary>
        /// Creates the name of the type.
        /// </summary>
        /// <returns>A new name of the type.</returns>
        string CreateTypeName();

        /// <summary>
        /// Creates the name of the type.
        /// </summary>
        /// <param name="prefix">The name prefix.</param>
        /// <param name="lenght">The lenght of name.</param>
        /// <returns>A new name of the type.</returns>
        string CreateTypeName(string prefix, int lenght);

        /// <summary>
        /// Creates the name of the method.
        /// </summary>
        /// <returns>A new name of the method.</returns>
        string CreateMethodName();

        /// <summary>
        /// Creates the name of the method.
        /// </summary>
        /// <param name="prefix">The name prefix.</param>
        /// <param name="lenght">The lenght of name.</param>
        /// <returns>A new name of the method.</returns>
        string CreateMethodName(string prefix, int lenght);
    }
}
