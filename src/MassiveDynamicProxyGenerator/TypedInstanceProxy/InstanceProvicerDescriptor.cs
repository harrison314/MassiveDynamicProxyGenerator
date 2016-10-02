using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.TypedInstanceProxy
{
    internal class InstanceProvicerDescriptor
    {
        public Type Type
        {
            get;
            protected set;
        }

        public MethodInfo Dispose
        {
            get;
            protected set;
        }

        public MethodInfo GetInstance
        {
            get;
            protected set;
        }

        public InstanceProvicerDescriptor()
        {
            this.Type = typeof(IInstanceProvicer);
            this.Dispose = this.Type.GetMethod(nameof(IInstanceProvicer.Dispose), new Type[0]);
            this.GetInstance = this.Type.GetMethod(nameof(IInstanceProvicer.GetInstance), new Type[0]);
        }
    }
}
