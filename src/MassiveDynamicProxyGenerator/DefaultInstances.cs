using MassiveDynamicProxyGenerator.Utils;

namespace MassiveDynamicProxyGenerator
{
    internal static class DefaultInstances
    {
        private static object syncRoot = new object();

        private static ITypeNameCreator typeNameCreator = null;
        private static GeneratedTypeList typedList = null;

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
