using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PerformaceExamples
{
    public class DecoratorDispatchProxy<T> : DispatchProxy where T : class
    {
        private T parent;
        private IRemoteCall remoteCall;

        public DecoratorDispatchProxy()
        {

        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            this.remoteCall.Call(targetMethod.Name, targetMethod.ReturnType, args);
            return targetMethod.Invoke(this.parent, args);
        }

        public static T Create(T parent, IRemoteCall remoteCall)
        {
            T decorator = DecoratorDispatchProxy<T>.Create<T, DecoratorDispatchProxy<T>>();
            (decorator as DecoratorDispatchProxy<T>).parent = parent;
            (decorator as DecoratorDispatchProxy<T>).remoteCall = remoteCall;

            return decorator;
        }
    }
}
