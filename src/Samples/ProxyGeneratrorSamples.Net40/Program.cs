using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MassiveDynamicProxyGenerator;
using MassiveDynamicProxyGenerator.TypedInstanceProxy;

namespace ProxyGeneratrorSamples.Net40
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ExampleLoggInterceptor();
            DynamicProxyAsJsonRpc2();
            InstanceProxyExample();

            Console.ReadLine();
        }

        private static void ExampleLoggInterceptor()
        {
            Console.WriteLine(".... GenerateDecorator for logging ...\n");

            ProxygGenerator generator = new ProxygGenerator();

            Calculator realInstance = new Calculator();
            ICallableInterceptor interceptor = new CallableInterceptorAdapter((invocation) =>
            {
                Console.WriteLine(" Log: Call method {0}", invocation.MethodName);
                try
                {
                    invocation.Process();
                    Console.WriteLine(" Log: return: {0}", invocation.ReturnValue ?? "null");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(" Log: Exception message: {0}", ex.Message);
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
            Console.WriteLine();
            Console.WriteLine();
        }

        private static void DynamicProxyAsJsonRpc2()
        {
            Console.WriteLine(".... GenerateProxy for remote JSON RPC2 ...\n");

            IInterceptor jsonRpcInterceptor = new InterceptorAdapter((invocation, isDynamic) =>
            {
                Guid requestId = Guid.NewGuid();
                var rcpBody = new
                {
                    jsonrpc = "2.0",
                    method = invocation.MethodName,
                    @params = invocation.Arguments,
                    id = requestId.ToString()
                };

                string serialized = Newtonsoft.Json.JsonConvert.SerializeObject(rcpBody);

                //simulate sending and recrive

                Console.WriteLine(" Sending Rpc: {0}", serialized);
                string recText = $"{{\"jsonrpc\": \"2.0\", \"result\": 19, \"id\": \"{requestId.ToString()}\"}}";
                Console.WriteLine(" Rec Rpc: {0}", recText);

                Newtonsoft.Json.Linq.JObject response = Newtonsoft.Json.JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(recText);
                invocation.ReturnValue = response["result"].ToObject(invocation.ReturnType);
            });

            ProxygGenerator generator = new ProxygGenerator();

            ICalculator calcilator = generator.GenerateProxy<ICalculator>(jsonRpcInterceptor);

            int resultModulo = calcilator.Modulo(15, 4);

            Console.WriteLine("Modulo 15 and 4 is {0}", resultModulo);

            int resultProduct = calcilator.Product(8, 486);

            Console.WriteLine("Product 8 and 486 is {0}", resultProduct);
            Console.WriteLine();
            Console.WriteLine();
        }

        private static void InstanceProxyExample()
        {
            Console.WriteLine(".... GenerateDecorator lazy initialize object ...\n");

            IInstanceProvicer instanceProvider = new LazyInstanceProvider<ICalculator>(
                () =>
                {
                    Console.WriteLine(" Log: Create Calculator.");
                    return new Calculator();
                }, false);

            ProxygGenerator generator = new ProxygGenerator();
            ICalculator  calculator = generator.GenerateInstanceProxy<ICalculator>(instanceProvider);

            Console.WriteLine("Before call Add with 2013 and 6");
            int result = calculator.Add(2013, 6);
            Console.WriteLine("Result is {0}", result);

            Console.WriteLine("Before call Product with 2013 and 6");
            int result2 = calculator.Product(2013, 6);
            Console.WriteLine("Result is {0}", result2);
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
