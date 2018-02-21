using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.DependencyInjection.Test.Services
{
    public class TypeC : ITypeC
    {
        public void FooForC()
        {
            throw new NotImplementedException();
        }
    }
}
