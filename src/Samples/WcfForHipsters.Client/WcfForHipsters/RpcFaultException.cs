using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WcfForHipsters.Client.WcfForHipsters
{
    public class RpcFaultException : Exception
    {
        public RpcFaultException()
        {
        }

        public RpcFaultException(string message) 
            : base(message)
        {
        }

        public RpcFaultException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
