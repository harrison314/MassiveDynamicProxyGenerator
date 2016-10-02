using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.TypedDecorator
{
    internal class GenerateUnion
    {
        public MethodInfo ProcessMethod
        {
            get;
            private set;
        }

        public GenerateUnion(MethodInfo methodInfo)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }

            this.ProcessMethod = methodInfo;
        }
    }
}
