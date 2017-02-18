using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WcfForHipsters.WebServer.WcfForHipsters
{
    public class RequestBody
    {
        public string jsonrpc
        {
            get;
            set;
        }

        public string method
        {
            get;
            set;
        }

        public object[] @params
        {
            get;
            set;
        }

        public string id
        {
            get;
            set;
        }

        public RequestBody()
        {

        }
    }
}
