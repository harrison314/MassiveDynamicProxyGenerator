# MassiveDynamicProxyGenerator
_.Net library for generate proxy class instances_

_Massive Dynamic Proxy Generator_ is  a library for generating lightweight
.NET proxies on the fly at runtime. Proxy objects allow calls to members
of an object to be intercepted without modifying the code of the class.
Both classes and interfaces can be proxied, however only virtual members
can be intercepted.

Fertures:
* Proxy instance generator, for intercept.
* Decorator generator.
* Proxy instance generator, for control instancing real implementation (e.g. lazy loading).

The following platforms are supported:
* .Net 4.0, 4.5, 4.6, 4.6.1, NetStandard 1.6, NetStandard 2.0 and UWP applications (NetStandard 1.4)


### Generate decorator 

```cs
 ProxyGenerator generator = new ProxyGenerator();
 Calculator realInstance = new Calculator();
 ICallableInterceptor interceptor = new CallableInterceptorAdapter((invocation) =>
 {
     Console.WriteLine(" Log: Call method {0}", invocation.MethodName);
     try
     {
         invocation.Process();
         Console.WriteLine(" Log: return: {0}", invocation.ReturnValue ?? "null");
     }
     catch(Exception ex)
     {
         Console.WriteLine(" Log: Exception message: {0}", ex.Message);
         throw;
     }
 });


 ICalculator decorator = generator.GenerateDecorator<ICalculator>(interceptor, realInstance);

 Console.WriteLine("Before call Add with 2013 and 6");
 int result = decorator.Add(2013, 6);
 Console.WriteLine("Result is {0}", result);
```

### Generate proxy

```cs
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

    Newtonsoft.Json.Linq.JObject response = Newtonsoft.Json.JsonConvert.DeserializeObject< Newtonsoft.Json.Linq.JObject>(recText);
    invocation.ReturnValue = response["result"].ToObject(invocation.ReturnType);
});

ProxyGenerator generator = new ProxyGenerator();

ICalculator calcilator = generator.GenerateProxy<ICalculator>(jsonRpcInterceptor);

int resultModulo = calcilator.Modulo(15, 4);

Console.WriteLine("Modulo 15 and 4 is {0}", resultModulo);

int resultProduct = calcilator.Product(8, 486);

Console.WriteLine("Product 8 and 486 is {0}", resultProduct);
```
### Generate instance proxy

```cs
IInstanceProvicer instanceProvider = new LazyInstanceProvider<ICalculator>(
    () =>
    {
        Console.WriteLine(" Log: Create Calculator.");
        return new Calculator();
    }, false);

ProxyGenerator generator = new ProxyGenerator();
ICalculator  calculator = generator.GenerateInstanceProxy<ICalculator>(instanceProvider);

Console.WriteLine("Before call Add with 2013 and 6");
int result = calculator.Add(2013, 6);
Console.WriteLine("Result is {0}", result);

Console.WriteLine("Before call Product with 2013 and 6");
int result2 = calculator.Product(2013, 6);
Console.WriteLine("Result is {0}", result2);
```