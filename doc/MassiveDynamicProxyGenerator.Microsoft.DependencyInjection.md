# MassiveDynamicProxyGenerator.Microsoft.DependencyInjection

_MassiveDynamicProxyGenerator.Microsoft.DependencyInjection_ is library of extensions method for _Microsoft.Extensions.DependencyInjection_ IoC container. 
Adding methods for decorators, proxys, instance proxies for resolving circular depndencies or lazy initialization and interpetion for decoration instances.

_Microsoft.Extensions.DependencyInjection_ is simple IoC container, so  _MassiveDynamicProxyGenerator.Microsoft.DependencyInjection_ does just a simple extenion for fetures from _MassiveDynamicProxyGenerator_ library, becose any fetures don't support open generics.

### Add decorators
This feture unrelated with _MassiveDynamicProxyGenerator_, it is universal adding interface generators.

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

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<IMessageService, RealMessageService>();
        services.AddDecorator<IMessageService, MessageServiceDecorator>();
        services.AddMvc();
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

    public void ConfigureServices(IServiceCollection services)
    {
        services.Addproxy<IMessageService>(new MyInterceptor());
        // or
        services.AddTransient<MyInterceptor>();
        services.Addproxy<IMessageService, MyInterceptor>();
        // or
        services.Addproxy<IMessageService>(typeof(MyInterceptor));
        // or
        services.AddTransient<IMessageService>(invocation => {
            //... any work with invocation
        });
        
        services.AddMvc();
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

    public void ConfigureServices(IServiceCollection services)
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

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddInstanceProxy<IMessageService>(new MessageServiceInstanceProvider());
        //or 
        services.AddInstanceProxy<IMessageService>(() => new RealMessageService());

        services.AddMvc();
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        //...
    }
}
    
```