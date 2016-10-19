using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.Utils
{
    /// <summary>
    /// Generated type list is like cache.
    /// </summary>
    internal class GeneratedTypeList
    {
        private readonly Dictionary<ITypeRquest, Type> cache;
        private readonly object syncRoot;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneratedTypeList"/> class.
        /// </summary>
        public GeneratedTypeList()
        {
            this.cache = new Dictionary<ITypeRquest, Type>();
            this.syncRoot = new object();
        }

        /// <summary>
        /// Enshures the cretion of type.
        /// </summary>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="decoraorType">Type of the decoraor.</param>
        /// <param name="typeFactory">The type factory for type.</param>
        /// <returns>Created type.</returns>
        /// <exception cref="ArgumentNullException">
        /// interfaceType
        /// or
        /// typeFactory
        /// </exception>
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

        /// <summary>
        /// Enshures the creation of type.
        /// </summary>
        /// <param name="interfaceTypes">The interface types.</param>
        /// <param name="decoraorType">Type of the decoraor.</param>
        /// <param name="typeFactory">The type factory.</param>
        /// <returns>Created type.</returns>
        /// <exception cref="ArgumentNullException">
        /// interfaceTypes
        /// or
        /// typeFactory
        /// </exception>
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
