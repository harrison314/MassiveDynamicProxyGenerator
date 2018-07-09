using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.SimpleInjector.Tests.Services
{
    public class TestMessager : ITestMessager
    {
        public TestMessager(IGenericService<string> strg, IGenericService<int> igst, IMessageService messages)
        {

        }
        public void Send()
        {
            throw new NotImplementedException();
        }
    }
}
