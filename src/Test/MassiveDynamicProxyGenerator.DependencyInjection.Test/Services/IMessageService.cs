using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.DependencyInjection.Test.Services
{
    public interface IMessageService
    {
        void Send(string email, string body);

        void TestMethod();

        int GetCountOfMessagesInFront();
    }
}
