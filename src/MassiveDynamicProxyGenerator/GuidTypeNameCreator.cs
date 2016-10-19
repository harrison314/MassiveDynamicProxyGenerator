using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator
{
    /// <summary>
    /// Type name creator, creates names using guid.
    /// </summary>
    /// <seealso cref="MassiveDynamicProxyGenerator.ITypeNameCreator" />
    public class GuidTypeNameCreator : ITypeNameCreator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GuidTypeNameCreator"/> class.
        /// </summary>
        public GuidTypeNameCreator()
        {
        }

        /// <summary>
        /// Creates the name of the method.
        /// </summary>
        /// <returns>
        /// A new name of the method.
        /// </returns>
        public string CreateMethodName()
        {
            string name = string.Concat("M", Guid.NewGuid().ToString("D").Replace("-", string.Empty));

            return name;
        }

        /// <summary>
        /// Creates the name of the method.
        /// </summary>
        /// <param name="prefix">The name prefix.</param>
        /// <param name="lenght">The lenght of name.</param>
        /// <returns>
        /// A new name of the method.
        /// </returns>
        /// <exception cref="ArgumentNullException">prefix</exception>
        /// <exception cref="ArgumentOutOfRangeException">lenght - lenght</exception>
        public string CreateMethodName(string prefix, int lenght)
        {
            if (prefix == null)
            {
                throw new ArgumentNullException(nameof(prefix));
            }

            if (lenght <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(lenght), $"Parameter {nameof(lenght)} must by more than zero.");
            }

            string name = string.Concat(prefix, Guid.NewGuid().ToString("D").Replace("-", string.Empty));

            return name;
        }

        /// <summary>
        /// Creates the name of the type.
        /// </summary>
        /// <returns>
        /// A new name of the type.
        /// </returns>
        public string CreateTypeName()
        {
            string name = string.Concat("T", Guid.NewGuid().ToString("D").Replace("-", string.Empty));

            return name;
        }

        /// <summary>
        /// Creates the name of the type.
        /// </summary>
        /// <param name="prefix">The name prefix.</param>
        /// <param name="lenght">The lenght of name.</param>
        /// <returns>
        /// A new name of the type.
        /// </returns>
        /// <exception cref="ArgumentNullException">prefix</exception>
        /// <exception cref="ArgumentOutOfRangeException">lenght - lenght</exception>
        public string CreateTypeName(string prefix, int lenght)
        {
            if (prefix == null)
            {
                throw new ArgumentNullException(nameof(prefix));
            }

            if (lenght <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(lenght), $"Parameter {nameof(lenght)} must by more than zero.");
            }

            string name = string.Concat(prefix, Guid.NewGuid().ToString("D").Replace("-", string.Empty));

            return name;
        }
    }
}
