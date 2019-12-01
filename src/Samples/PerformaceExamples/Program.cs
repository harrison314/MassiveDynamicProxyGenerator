using BenchmarkDotNet.Running;
using System;

namespace PerformaceExamples
{
    public class Program
    {
        public static void Main(string[] args)
        {
            _ = BenchmarkRunner.Run<DynamicProxyBenchmark>();
        }
    }
}
