using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.Utils
{
    internal class GeneratedTypeList
    {
        private readonly Dictionary<ITypeRquest, Type> cache;
        private readonly object syncRoot;

        public GeneratedTypeList()
        {
            this.cache = new Dictionary<ITypeRquest, Type>();
            this.syncRoot = new object();
        }

        public Type EnshureType(Type interfaceType, TypedDecoratorType decoraorType, Func<Type, Type> typeFactory)
        {
            if (interfaceType == null)
            {
                throw new ArgumentNullException(nameof(interfaceType));
            }

            if (typeFactory == null)
            {
                throw new ArgumentNullException(nameof(typeFactory));
            }

            TypeRquest request = new TypeRquest(interfaceType, decoraorType);
            Type returnValue;

            lock (this.syncRoot)
            {
                if (this.cache.TryGetValue(request, out returnValue))
                {
                    return returnValue;
                }
                else
                {
                    returnValue = typeFactory.Invoke(interfaceType);
                    this.cache.Add(request, returnValue);

                    return returnValue;
                }
            }
        }

        public Type EnshureType(Type[] interfaceTypes, TypedDecoratorType decoraorType, Func<Type[], Type> typeFactory)
        {
            if (interfaceTypes == null)
            {
                throw new ArgumentNullException(nameof(interfaceTypes));
            }

            if (typeFactory == null)
            {
                throw new ArgumentNullException(nameof(typeFactory));
            }

            MultyTypeRquest request = new MultyTypeRquest(interfaceTypes, decoraorType);
            Type returnValue;

            lock (this.syncRoot)
            {
                if (this.cache.TryGetValue(request, out returnValue))
                {
                    return returnValue;
                }
                else
                {
                    returnValue = typeFactory.Invoke(interfaceTypes);
                    this.cache.Add(request, returnValue);

                    return returnValue;
                }
            }
        }
    }
}
