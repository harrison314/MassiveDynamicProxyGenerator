using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WcfForHipsters.WebServer.Contract
{
    public class RequestMetadata
    {
        public string Nonce
        {
            get;
            set;
        }

        public int CorelationId
        {
            get;
            set;
        }

        public ReuquestFormat Format
        {
            get;
            set;
        }

        public RequestMetadata()
        {

        }
    }
}
