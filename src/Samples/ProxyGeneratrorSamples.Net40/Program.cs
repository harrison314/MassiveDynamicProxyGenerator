using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MassiveDynamicProxyGenerator;

namespace ProxyGeneratrorSamples.Net40
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ExampleLoggInterceptor();

            Console.ReadLine();
        }

        private static void ExampleLoggInterceptor()
        {
            ProxygGenerator generator = new ProxygGenerator();

            Calculator realInstance = new Calculator();
            ICallableInterceptor interceptor = new CallableInterceptorAdapter((invocation) =>
            {
                Console.WriteLine("Log: Call method {0}", invocation.MethodName);
                try
                {
                    invocation.Process();
                    Console.WriteLine("Log: return: {0}", invocation.ReturnValue ?? "null");
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Log: Exception message: {0}", ex.Message);
                    throw;
                }
            });


            ICalculator decorator = generator.GenerateDecorator<ICalculator>(interceptor, realInstance);

            Console.WriteLine("Before call Add with 2013 and 6");
            int result = decorator.Add(2013, 6);
            Console.WriteLine("Result is {0}", result);

            Console.WriteLine();

            try
            {
                Console.WriteLine("Before call Modulo with 45 and -2");
                int modulo = decorator.Modulo(45, -2);
                Console.WriteLine("Result is {0}", modulo);
            }
            catch (ArgumentOutOfRangeException)
            {

            }
        }
    }
}
