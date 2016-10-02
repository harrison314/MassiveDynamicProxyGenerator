using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator
{
    public interface ICallableInvocation : IInvocation
    {
        void Process();
    }
}
