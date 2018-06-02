using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WcfForHipsters.WebServer.Contract
{
    public interface IExampleService
    {
        int CalCulateAdd(int a, int b, int c);

        CreatBookResponse CreateBook(CreateBookRequest request);

        void SendEvent(Guid id, string content, RequestMetadata metadata);
    }
}
