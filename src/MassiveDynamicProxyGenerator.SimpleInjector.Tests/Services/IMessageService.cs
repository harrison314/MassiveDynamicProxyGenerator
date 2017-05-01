using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.SimpleInjector.Tests.Services
{
    public interface IMessageService
    {
        void Send(string email, string body);

        void TestMethod();

        int GetCountOfMessagesInFront();
    }
}
