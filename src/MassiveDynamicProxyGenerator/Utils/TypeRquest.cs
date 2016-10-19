using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.Utils
{
    /// <summary>
    /// Type request for single interface.
    /// </summary>
    /// <seealso cref="IEquatable{TypeRquest}" />
    /// <seealso cref="ITypeRquest" />
    internal class TypeRquest : IEquatable<TypeRquest>, ITypeRquest
    {
        private readonly TypedDecoratorType decoratorType;
        private readonly Type interfaceType;

        /// <summary>
        /// Gets the type of the decorator.
        /// </summary>
        /// <value>
        /// The type of the decorator.
        /// </value>
        public virtual TypedDecoratorType DecoratorType
        {
            get
            {
                return this.decoratorType;
            }
        }

        /// <summary>
        /// Gets the interface types.
        /// </summary>
        /// <value>
        /// The interface types.
        /// </value>
        public virtual Type[] InterfaceTypes
        {
            get
            {
                return new Type[] { this.interfaceType };
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeRquest"/> class.
        /// </summary>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="decoratorType">Type of the decorator.</param>
        /// <exception cref="ArgumentNullException">interfaceType</exception>
        public TypeRquest(Type interfaceType, TypedDecoratorType decoratorType)
        {
            if (interfaceType == null)
            {
                throw new ArgumentNullException(nameof(interfaceType));
            }

            this.interfaceType = interfaceType;
            this.decoratorType = decoratorType;
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }

            return this.Equals((TypeRquest)obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            int hash = this.interfaceType.GetHashCode();
            hash += (int)this.decoratorType;

            return hash;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(TypeRquest other)
        {
            if (other == null)
            {
                return false;
            }

            return this.decoratorType == other.decoratorType && this.interfaceType == other.interfaceType;
        }
    }
}
