# Delegate Registration

Sometimes you really need a bit more power behind constructing a service. For these times you may find yourself in one of the following scenarios:

- You just need to perform some Action like:

```c#
public static IBackendService CreateBackendService()
{
    return new BackendService
    {
        Uri = Constants.BackendUri
    };
}
```

- You need to resolve something to do a more complex look up and properly construct your type:

```c#
public static IBackendService CreateBackendService(IContainerProvider containerProvider)
{
    var options = containerProvider.Resolve<IOptions>();
    return containerProvider.Resolve<IBackendService>((typeof(Uri), options.BackendUri));
}
```

!!! note "Note"
    This supports both Delegates with `IContainerProvider` and `IServiceProvider`

Regardless of which way you need to resolve service the Delegate Registration extensions really help out for those scenarios where you can't just simply pass a raw implementing type.

```c#
protected override void RegisterTypes(IContainerRegistry containerRegistry)
{
    containerRegistry.RegisterDelegate<IFoo>(FooFactory);
    containerRegistry.RegisterDelegate<IBar>(BarFactory);
}

private static IFoo FooFactory() => new Foo();

private static IBar BarFactory(IContainerProvider container)
{
    var options = container.Resolve<IOptions>();
    return new Bar { HasCode = options.HasCode };
}

private static IBar BarFactory(IServiceProvider serviceProvider)
{
    var options = serviceProvider.GetService<IOptions>();
    return new Bar { HasCode = options.HasCode };
}
```