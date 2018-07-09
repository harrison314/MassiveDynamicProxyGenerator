using System;
using System.Collections.Generic;
using System.Text;

namespace MassiveDynamicProxyGenerator.DependencyInjection.Test.Services
{
    public class MessageService : IMessageService
    {
        public MessageService()
        {

        }

        public int GetCountOfMessagesInFront()
        {
            return 14;
        }

        public void Send(string email, string body)
        {

        }

        public void TestMethod()
        {

        }
    }
}
