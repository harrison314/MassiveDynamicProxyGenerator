using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.Utils
{
    internal sealed class TypeRquest : IEquatable<TypeRquest>
    {
        private readonly TypedDecoratorType decoratorType;
        private readonly Type interfaceType;

        public TypedDecoratorType DecoratorType
        {
            get
            {
                return this.decoratorType;
            }
        }

        public Type InterfaceType
        {
            get
            {
                return this.interfaceType;
            }
        }

        public TypeRquest(Type interfaceType, TypedDecoratorType decoratorType)
        {
            if (interfaceType == null)
            {
                throw new ArgumentNullException(nameof(interfaceType));
            }

            this.interfaceType = interfaceType;
            this.decoratorType = decoratorType;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }

            return this.Equals((TypeRquest)obj);
        }

        public override int GetHashCode()
        {
            int hash = this.interfaceType.GetHashCode();
            hash += (int)this.decoratorType;

            return hash;
        }

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
