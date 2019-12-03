using BenchmarkDotNet.Attributes;
using MassiveDynamicProxyGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformaceExamples
{
    public class DecoratorBenchmark
    {
        private IExamapleInterface massiveDynamicDecorator;
        private IExamapleInterface dispatchProxyDynamicDecorator;

        [GlobalSetup]
        public void Setup()
        {
            IRemoteCall remoteCall = new RemoteCall();
            ProxyGenerator proxyGenerator = new ProxyGenerator();

            this.massiveDynamicDecorator = proxyGenerator.GenerateDecorator<IExamapleInterface>(new LoggerCallableInterceptor(remoteCall), new ExamapleInterface());
            this.dispatchProxyDynamicDecorator = DecoratorDispatchProxy<IExamapleInterface>.Create(new ExamapleInterface(), remoteCall);
        }

        [Benchmark]
        public string MassiveDynamic()
        {
            return this.massiveDynamicDecorator.Foo(12, "hello", "world");
        }

        [Benchmark]
        public string DispatchProxy()
        {
            return this.dispatchProxyDynamicDecorator.Foo(12, "hello", "world");
        }
    }
}
