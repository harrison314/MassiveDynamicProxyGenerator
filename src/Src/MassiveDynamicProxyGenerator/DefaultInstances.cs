using MassiveDynamicProxyGenerator.Utils;

namespace MassiveDynamicProxyGenerator
{
    /// <summary>
    /// Container for default instances.
    /// </summary>
    internal static class DefaultInstances
    {
        private static object syncRoot = new object();

        private static ITypeNameCreator typeNameCreator = null;
        private static GeneratedTypeList typedList = null;

        /// <summary>
        /// Gets the type name creator.
        /// </summary>
        /// <value>
        /// The type name creator.
        /// </value>
        public static ITypeNameCreator TypeNameCreator
        {
            get
            {
                if (typeNameCreator == null)
                {
                    lock (syncRoot)
                    {
                        if (typeNameCreator == null)
                        {
                            typeNameCreator = new KoreanTypeNameCreator();
                        }
                    }
                }

                return typeNameCreator;
            }
        }

        /// <summary>
        /// Gets the typed list - cache.
        /// </summary>
        /// <value>
        /// The typed list.
        /// </value>
        public static GeneratedTypeList TypedList
        {
            get
            {
                if (typedList == null)
                {
                    lock (syncRoot)
                    {
                        if (typedList == null)
                        {
                            typedList = new GeneratedTypeList();
                        }
                    }
                }

                return typedList;
            }
        }
    }
}
