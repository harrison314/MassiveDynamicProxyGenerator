using BenchmarkDotNet.Attributes;
using MassiveDynamicProxyGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformaceExamples
{
    public class DynamicProxyBenchmark
    {
        private IExamapleInterface massiveDynamicProxy;
        private IExamapleInterface disptachProxy;

        [GlobalSetup]
        public void Setup()
        {
            IRemoteCall remoteCall = new RemoteCall();
            ProxyGenerator proxyGenerator = new ProxyGenerator();

            this.massiveDynamicProxy = proxyGenerator.GenerateProxy<IExamapleInterface>(new RemoteCallInterceptor(remoteCall));
            this.disptachProxy = RemoteCallDispatchProxy.Create<IExamapleInterface>(remoteCall);
        }

        [Benchmark]
        public string MassiveDynamic()
        {
            return this.massiveDynamicProxy.Foo(12, "hello", "world");
        }

        [Benchmark]
        public string DispatchProxy()
        {
            return this.disptachProxy.Foo(12, "hello", "world");
        }
    }
}
