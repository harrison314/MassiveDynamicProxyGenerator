using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.SimpleInjector.Tests.Services
{
    public class StringGenericService : IGenericService<string>
    {
        public StringGenericService()
        {

        }

        public string GetLast()
        {
            throw new NotImplementedException();
        }

        public string Transform(string item)
        {
            throw new NotImplementedException();
        }
    }
}
