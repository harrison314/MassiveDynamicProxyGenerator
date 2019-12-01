using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformaceExamples
{
    public class RemoteCall : IRemoteCall
    {
        public RemoteCall()
        {

        }

        public object Call(string methodName, Type returnType, object[] parameters)
        {
            return methodName;
        }
    }
}
