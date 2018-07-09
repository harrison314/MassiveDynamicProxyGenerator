using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.SimpleInjector.Tests.Services
{
    public class IntGenericService : IGenericService<int>
    {
        public IntGenericService()
        {

        }

        public int GetLast()
        {
            throw new NotImplementedException();
        }

        public int Transform(int item)
        {
            throw new NotImplementedException();
        }
    }
}
