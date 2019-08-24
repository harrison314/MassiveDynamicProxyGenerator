using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.TypedProxy
{
    internal class TypedProxyContext
    {
        public MethodBuilder ParentMethodCall
        {
            get;
            protected set;
        }

        public TypedProxyContext(MethodBuilder builder)
        {
#if DEBUG
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
#endif

            this.ParentMethodCall = builder;
        }
    }
}
