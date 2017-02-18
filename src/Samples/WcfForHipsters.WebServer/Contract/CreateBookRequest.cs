using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WcfForHipsters.WebServer.Contract
{
    public class CreateBookRequest
    {
        public string Title
        {
            get;
            set;
        }

        public string MarkdawnText
        {
            get;
            set;
        }

        public RequestMetadata Metadata
        {
            get;
            set;
        }

        public CreateBookRequest()
        {

        }
    }
}
