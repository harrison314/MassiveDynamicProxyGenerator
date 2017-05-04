using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.SimpleInjector.Tests.Services
{
    public class MessageService : IMessageService
    {
        public MessageService()
        {

        }

        public int GetCountOfMessagesInFront()
        {
            return 12;
        }

        public void Send(string email, string body)
        {
            
        }

        public void TestMethod()
        {
            
        }
    }
}
