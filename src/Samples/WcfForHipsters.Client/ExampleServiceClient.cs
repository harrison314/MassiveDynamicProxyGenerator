using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WcfForHipsters.Client.WcfForHipsters;
using WcfForHipsters.WebServer.Contract;

namespace WcfForHipsters.Client
{
    public class ExampleServiceClient : HipsterClientBase<IExampleService>, IExampleService
    {
        public ExampleServiceClient(string endpointUrl)
            : base(endpointUrl)
        {
        }

        public ExampleServiceClient(Uri endpoint)
            : base(endpoint)
        {
        }

        public int CalCulateAdd(int a, int b, int c)
        {
            return this.Channal.CalCulateAdd(a, b, c);
        }

        public CreatBookResponse CreateBook(CreateBookRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            return this.Channal.CreateBook(request);
        }

        public void SendEvent(Guid id, string content, RequestMetadata metadata)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));
            if (metadata == null) throw new ArgumentNullException(nameof(metadata));

            this.Channal.SendEvent(id, content, metadata);
        }
    }
}
