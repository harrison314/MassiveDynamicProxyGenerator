# MassiveDynamicProxyGenerator

[![NuGet Status](http://img.shields.io/nuget/v/MassiveDynamicProxyGenerator.svg?style=flat)](https://www.nuget.org/packages/MassiveDynamicProxyGenerator/)

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
* .Net Framework 4.8, NetStandard 2.0, .Net 6.0 and .Net 8.0

## Getting started

In package manager console:

`PM> Install-Package MassiveDynamicProxyGenerator`

Or
`dotnet add package MassiveDynamicProxyGenerator`

## A Quick Example

Examples of use MassiveDynamicProxyGenerator.

### Use for generate proxy stub for JsonRpc client

In samples folder in project __WcfForHipsters.WebServer__ is implement service endpoint using api controller
(for example ExampleServiceController) and transform JsonRpc call to method invocation using generic class __EndpointAdapter__.

Client is in project __WcfForHipsters.Client__. Generic class __HipsterClientBase__ , like ClientBase class from Wcf,
using MassiveDynamicProxyGenerator for generate proxy stub for JsonRpc client.

Client and server using interface __IExampleService__ as service contract.

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

ICalculator calculator = generator.GenerateProxy<ICalculator>(jsonRpcInterceptor);

int resultModulo = calculator.Modulo(15, 4);

Console.WriteLine("Modulo 15 and 4 is {0}", resultModulo);

int resultProduct = calculator.Product(8, 486);

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

# MassiveDynamicProxyGenerator.SimpleInjector

[![NuGet Status](http://img.shields.io/nuget/v/MassiveDynamicProxyGenerator.SimpleInjector.svg?style=flat)](https://www.nuget.org/packages/MassiveDynamicProxyGenerator.SimpleInjector/)

_Library for integration MassiveDynamicProxyGenerator to [SimpleInjector](http://simpleinjector.readthedocs.io/en/latest/)._

_MassiveDynamicProxyGenerator.SimpleInjector_ is library of extensions method for IoC container - Simple Injector.
Adding methods for register mock implementations, proxys, instance proxies for resolving circular depndencies or lazy initialization and interpetion for decoration instances.

## Getting started

In package manager console:

`PM> Install-Package MassiveDynamicProxyGenerator.SimpleInjector`

Or

`dotnet add package MassiveDynamicProxyGenerator.SimpleInjector`

Or download [MassiveDynamicProxyGenerator.SimpleInjector](https://www.nuget.org/packages/MassiveDynamicProxyGenerator.SimpleInjector/).

### Register mock

```cs
using MassiveDynamicProxyGenerator.SimpleInjector;

Container container = new Container();
container.RegisterMock<IMessageService>();
```

### Register proxy instances

```cs
using MassiveDynamicProxyGenerator.SimpleInjector;

Container container = new Container();
InterceptorAdapter adapter = new InterceptorAdapter(invodcation =>
{
     if (invodcation.MethodName == nameof(IMessageService.GetCountOfMessagesInFront))
     {
          invodcation.ReturnValue = 42;
     }
});

container.RegisterProxy(typeof(IMessageService), adapter);
```
### Register decorators using interception

```cs
using MassiveDynamicProxyGenerator.SimpleInjector;

Container container = new Container();
CallableInterceptorAdapter adapter = new CallableInterceptorAdapter((invodcation) =>
{
    invodcation.Process();
    if (invodcation.MethodName == nameof(IMessageService.GetCountOfMessagesInFront))
    {
       invodcation.ReturnValue = 42;
    }
});

container.RegisterInterceptedDecorator(adapter, t => !t.Name.StartsWith("IType"));
```

### Register instance provider

```cs
using MassiveDynamicProxyGenerator.SimpleInjector;

Container container = new Container();
container.RegisterInstanceProxy<IMessageService>(() => new MessageService());
```

### Dangerous extensions for mock all unregistred types

```cs
using MassiveDynamicProxyGenerator.SimpleInjector;
using MassiveDynamicProxyGenerator.SimpleInjector.Dangerous;

Container container = new Container();
container.RegisterAllUnregistredAsMock();
```

### Dangerous extension for instance provider

This extensions using only with cope lifestyle.

```cs
using MassiveDynamicProxyGenerator.SimpleInjector;
using MassiveDynamicProxyGenerator.SimpleInjector.Dangerous;

Container container = new Container();

internal class WeekInstanceProvider : IInstanceProvicer
{
   private WeakReference<ITypeA> reference;
   private object syncRoot;

   public WeekInstanceProvider()
   {
      this.reference = new WeakReference<ITypeA>(null);
      this.syncRoot = new object();
   }
  
   public object GetInstance()
   {
       IMessageService instance = null;
       lock (this.syncRoot)
       {
          reference.TryGetTarget(out instance);
          if (instance == null)
          {
             instance= new MessageService();
             reference.SetTarget(instance);
          }
       }

        return instance;
    }
        
    public void Dispose()
    {
    }
}

container.RegisterInstanceProxy<IMessageService>(new WeekInstanceProvider());
```

# MassiveDynamicProxyGenerator.Microsoft.DependencyInjection
**This library is obsolute.**

[![NuGet Status](http://img.shields.io/nuget/v/MassiveDynamicProxyGenerator.Microsoft.DependencyInjection.svg?style=flat)](https://www.nuget.org/packages/MassiveDynamicProxyGenerator.Microsoft.DependencyInjection/)

_Library for integration MassiveDynamicProxyGenerator to Asp.Net Core standard [IoC](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection/)._

_MassiveDynamicProxyGenerator.Microsoft.DependencyInjection_ is library of extensions method for _Microsoft.Extensions.DependencyInjection_ IoC container. 
Adding methods for decorators, proxys, instance proxies for resolving circular depndencies or lazy initialization and interpetion for decoration instances.

_Microsoft.Extensions.DependencyInjection_ is simple IoC container, so  _MassiveDynamicProxyGenerator.Microsoft.DependencyInjection_ does just a simple extenion for fetures from _MassiveDynamicProxyGenerator_ library, because any fetures don't support open generics.

## Getting started

In package manager console:

`PM> Install-Package MassiveDynamicProxyGenerator.Microsoft.DependencyInjection`

Or

`dotnet add package MassiveDynamicProxyGenerator.Microsoft.DependencyInjection`

Or download [MassiveDynamicProxyGenerator.Microsoft.DependencyInjection](https://www.nuget.org/packages/MassiveDynamicProxyGenerator.Microsoft.DependencyInjection/).

### Add decorators
This feature unrelated with _MassiveDynamicProxyGenerator_, it is universal adding interface generators.

```cs
using MassiveDynamicProxyGenerator.Microsoft.DependencyInjection;

public class MessageServiceDecorator : IMessageService
{
    public MessageServiceDecorator(IMessageService messageService, ILogger<MessageServiceDecorator> logger)
    {
        // ...
    }

    // ...
}

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        this.Configuration = configuration;
    }

    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<IMessageService, RealMessageService>();
        services.AddDecorator<IMessageService, MessageServiceDecorator>();
        services.AddMvc();

        return services.BuildIntercepedServiceProvider();
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        //...
    }
}
    
```

### Add Proxy
```cs
using MassiveDynamicProxyGenerator.Microsoft.DependencyInjection;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        this.Configuration = configuration;
    }

    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
        services.AddProxy<IMessageService>(new MyInterceptor());
        // or
        services.AddTransient<MyInterceptor>();
        services.AddProxy<IMessageService, MyInterceptor>();
        // or
        services.AddProxy<IMessageService>(typeof(MyInterceptor));
        // or
        services.AddTransient<IMessageService>(invocation => {
            //... any work with invocation
        });
        
        services.AddMvc();

        return services.BuildIntercepedServiceProvider();
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        //...
    }
}
    
```

### Add Intercepred Decorator
```cs
using MassiveDynamicProxyGenerator.Microsoft.DependencyInjection;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        this.Configuration = configuration;
    }

    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<IMessageService, RealMessageService>();

        services.AddInterceptedDecorator<IMessageService>(new MessageCallableInterceptor());
        // or
        services.AddInterceptedDecorator<IMessageService, MessageCallableInterceptor>();
        // or
        services.AddInterceptedDecorator(type => type.Name.EndsWith("Service"), 
            serviceProvider => new LoggingInterceptor(serviceProvider.GetRequiredService<ILogger>()));
        // or
        services.AddInterceptedDecorator<IMessageService>(new CallableInterceptorAdapter(invocation =>
        {
            // operations before real methd call
            invocation.Process();
            // operations after real methd call
        }));

        services.AddMvc();

        return services.BuildIntercepedServiceProvider();
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        //...
    }
}
    
```

### Add Instance Proxy
```cs
using MassiveDynamicProxyGenerator.Microsoft.DependencyInjection;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        this.Configuration = configuration;
    }

    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
        services.AddInstanceProxy<IMessageService>(new MessageServiceInstanceProvider());
        //or 
        services.AddInstanceProxy<IMessageService>(() => new RealMessageService());

        services.AddMvc();

        return services.BuildIntercepedServiceProvider();
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        //...
    }
}
    
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
