using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.Utils
{
    /// <summary>
    /// Type request for multiple interface instance.
    /// </summary>
    /// <seealso cref="MassiveDynamicProxyGenerator.Utils.ITypeRquest" />
    internal class MultyTypeRquest : ITypeRquest
    {
        private readonly Type[] interfaceTypes;
        private readonly TypedDecoratorType decoraorType;
        private readonly int hashCode;

        /// <summary>
        /// Gets the type of the decorator.
        /// </summary>
        /// <value>
        /// The type of the decorator.
        /// </value>
        public TypedDecoratorType DecoratorType
        {
            get
            {
                return this.decoraorType;
            }
        }

        /// <summary>
        /// Gets the interface types.
        /// </summary>
        /// <value>
        /// The interface types.
        /// </value>
        public Type[] InterfaceTypes
        {
            get
            {
                return this.interfaceTypes;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultyTypeRquest"/> class.
        /// </summary>
        /// <param name="interfaceTypes">The interface types.</param>
        /// <param name="decoraorType">Type of the decoraor.</param>
        /// <exception cref="ArgumentNullException">interfaceTypes</exception>
        public MultyTypeRquest(Type[] interfaceTypes, TypedDecoratorType decoraorType)
        {
            if (interfaceTypes == null)
            {
                throw new ArgumentNullException(nameof(interfaceTypes));
            }

            Type[] types = (Type[])interfaceTypes.Clone();
            Array.Sort<Type>(types, (a, b) => a.AssemblyQualifiedName.CompareTo(b.AssemblyQualifiedName));

            this.interfaceTypes = types;
            this.decoraorType = decoraorType;
            this.hashCode = this.CalculateHashCode(types, decoraorType);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return this.hashCode;
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

            MultyTypeRquest other = (MultyTypeRquest)obj;
            if (this.hashCode != other.hashCode)
            {
                return false;
            }

            if (this.decoraorType != other.decoraorType)
            {
                return false;
            }

            if (this.interfaceTypes.Length != other.interfaceTypes.Length)
            {
                return false;
            }

            for (int i = 0; i < this.interfaceTypes.Length; i++)
            {
                if (!this.interfaceTypes[i].Equals(other.interfaceTypes[i]))
                {
                    return false;
                }
            }

            return true;
        }

        private int CalculateHashCode(Type[] interfaceTypes, TypedDecoratorType decoraorType)
        {
            unchecked
            {
                const int p = 16777619;
                int hashCode = (int)2166136261;
                hashCode = (hashCode ^ decoraorType.GetHashCode()) * p;

                for (int i = 0; i < interfaceTypes.Length; i++)
                {
                    hashCode = (hashCode ^ interfaceTypes[i].GetHashCode()) * p;
                }

                hashCode += hashCode << 13;
                hashCode ^= hashCode >> 7;
                hashCode += hashCode << 3;
                hashCode ^= hashCode >> 17;
                hashCode += hashCode << 5;
                return hashCode;
            }
        }
    }
}
