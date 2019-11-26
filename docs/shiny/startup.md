```c#
public class PrismStartup : ShinyStartup
{
    public override void ConfigureServices(IServiceCollection services)
    {
        // Register services with Shiny like:
        services.UseGpsBackground<MyDelegate>();
    }

    public override IServiceProvider CreateServiceProvider(IServiceCollection services)
    {
        return PrismContainerExtension.Current.CreateServiceProvider(services);
    }
}
```

### Shiny.Prism

With your App using the PrismApplication from Prism.DryIoc.Forms.Extended you now only need to reference the PrismStartup as the base class for your Startup class like:

```c#
public class MyStartup : PrismStartup
{
    public override void ConfigureServices(IServiceCollection services)
    {
        // Register services with Shiny like:
        services.UseGps<MyDelegate>();
    }
}
```

You can now pass your startup to the ShinyHost at your application's startup and use full Dependency Injection of Shiny's services in your app, full DI of services from Prism's container within services that are resolved by Shiny.

**NOTE:** Shiny uses `IServiceProvider` which does not support the use of named services.