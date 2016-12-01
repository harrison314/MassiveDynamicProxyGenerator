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
* .Net 4.0, 4.5, 4.6, 4.6.1 and .Net Core 1.0 (NetStandard 1.6)

## Getting started

In package manager console:

`Install-Package MassiveDynamicProxyGenerator`

or add package to _project.json_ in .Net Core projects.

## A Quick Example

_In progress._

### Generate decorator 

```cs
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

ProxygGenerator generator = new ProxygGenerator();

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

ProxygGenerator generator = new ProxygGenerator();
ICalculator  calculator = generator.GenerateInstanceProxy<ICalculator>(instanceProvider);

Console.WriteLine("Before call Add with 2013 and 6");
int result = calculator.Add(2013, 6);
Console.WriteLine("Result is {0}", result);

Console.WriteLine("Before call Product with 2013 and 6");
int result2 = calculator.Product(2013, 6);
Console.WriteLine("Result is {0}", result2);
```

## Inscribtion

Inscribe to _Lucia L._ extraordinary woman, a wonderful person and an intelligent medical doctor.


## License

**The MIT License (MIT)**

Copyright (c) 2016 harrison314

Permission is hereby granted, free of charge, to any person
obtaining a copy of this software and associated documentation
files (the "Software"), to deal in the Software without
restriction, including without limitation the rights to use,
copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the
Software is furnished to do so, subject to the following
conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.

Project started 14.8.2016.