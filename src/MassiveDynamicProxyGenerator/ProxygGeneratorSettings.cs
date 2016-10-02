using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator
{
    public class ProxygGeneratorSettings
    {
        public bool UseLocalCache
        {
            get;
            set;
        }

        public ITypeNameCreator TypeNameCreator
        {
            get;
            set;
        }

        public string AssemblyName
        {
            get;
            set;
        }

        public ProxygGeneratorSettings()
        {
            this.UseLocalCache = false;
            this.TypeNameCreator = null;
            this.AssemblyName = null;
        }
    }
}
