# MassiveDynamicProxyGenerator.SimpleInjector


_MassiveDynamicProxyGenerator.SimpleInjector_ is library of extensions method for IoC container - Simple Injector.
Adding methods for register mock implementations, proxys, instance proxies for resolving circular depndencies or lazy initialization and interpetion for decoration instances.

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

This extenions using only with cope lifestyle.

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
