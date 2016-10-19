using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator
{
    /// <summary>
    /// Optimal settings for <see cref="ProxygGenerator"/>.
    /// </summary>
    /// <seealso cref="ProxygGenerator"/>
    public class ProxygGeneratorSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether <see cref="ProxygGenerator"/> use local or global cache.
        /// </summary>
        /// <value>
        ///   <c>true</c> if use local cache; otherwise, <c>false</c> use global cache.
        /// </value>
        public bool UseLocalCache
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type name creator.
        /// </summary>
        /// <value>
        /// The type name creator.
        /// </value>
        public ITypeNameCreator TypeNameCreator
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the created assembly.
        /// </summary>
        /// <value>
        /// The name of the cerated assembly.
        /// </value>
        public string AssemblyName
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxygGeneratorSettings"/> class.
        /// </summary>
        public ProxygGeneratorSettings()
        {
            this.UseLocalCache = false;
            this.TypeNameCreator = null;
            this.AssemblyName = null;
        }
    }
}
