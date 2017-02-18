using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WcfForHipsters.WebServer.WcfForHipsters
{
    public class FaultException : Exception
    {
        public FaultBody FaultBody
        {
            get;
            private set;
        }

        public FaultException(string message, string id, Exception innerException) 
            : base(message, innerException)
        {
            this.FaultBody = new FaultBody()
            {
                error = innerException.ToString(),
                id = id,
                jsonrpc = "2.0"
            };
        }
    }
}
