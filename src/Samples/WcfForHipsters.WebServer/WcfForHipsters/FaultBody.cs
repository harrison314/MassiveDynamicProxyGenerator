using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WcfForHipsters.WebServer.WcfForHipsters
{
    public class FaultBody
    {
        public string jsonrpc
        {
            get;
            set;
        }

        public string id
        {
            get;
            set;
        }

        public string error
        {
            get;
            set;
        }

        public FaultBody()
        {

        }
    }
}
