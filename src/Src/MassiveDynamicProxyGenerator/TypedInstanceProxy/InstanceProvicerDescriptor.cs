using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.TypedInstanceProxy
{
    /// <summary>
    /// Instance provider descriptor.
    /// </summary>
    /// <seealso cref="IInstanceProvicer"/>
    internal class InstanceProvicerDescriptor
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public Type Type
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the dispose.
        /// </summary>
        /// <value>
        /// The dispose.
        /// </value>
        public MethodInfo Dispose
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the get instance.
        /// </summary>
        /// <value>
        /// The get instance.
        /// </value>
        public MethodInfo GetInstance
        {
            get;
            protected set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceProvicerDescriptor"/> class.
        /// </summary>
        public InstanceProvicerDescriptor()
        {
            this.Type = typeof(IInstanceProvicer);
            this.Dispose = this.Type.GetTypeInfo().GetMethod(nameof(IInstanceProvicer.Dispose), Type.EmptyTypes);
            this.GetInstance = this.Type.GetTypeInfo().GetMethod(nameof(IInstanceProvicer.GetInstance), Type.EmptyTypes);
        }
    }
}
