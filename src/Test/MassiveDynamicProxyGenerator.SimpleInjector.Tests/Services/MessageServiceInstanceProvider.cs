using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.SimpleInjector.Tests.Services
{
    public class MessageServiceInstanceProvider : IInstanceProvicer
    {
        public MessageServiceInstanceProvider()
        {

        }

        public void Dispose()
        {
        }

        public object GetInstance()
        {
            return new MessageService();
        }
    }
}
