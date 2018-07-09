using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.Tests.Examples
{
    class InvocationExample : IInvocation
    {
        public object[] Arguments
        {
            get;
            set;
        }

        public Type[] ArgumentTypes
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string MethodName
        {
            get;
            set;
        }

        public Type OriginalType
        {
            get;
            set;
        }

        public Type ReturnType
        {
            get;
            set;
        }

        public object ReturnValue
        {
            get;
            set;
        }

        public MethodBase GetConcreteMethod()
        {
            throw new NotImplementedException();
        }
    }
}
