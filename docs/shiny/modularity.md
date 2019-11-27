Modularity support for Shiny is a preview feature and can be used by overriding the `ConfigureModuleCatalog` method in the `PrismStartup`, and adding modules just as you would do in the PrismApplication.

## Why on earth would I use this

Ok it's a fair question why should you use this? Let's say that you are trying to build out a Modular application with Prism, but you also need some things from Shiny like you need to register some sort of Background Service. Rather than push the buck to the consuming application to wire everything up this can be taken of in the Module which can now be reused across apps with a more minimalistic configuration.

## How to use Modules with Shiny and Prism

```c#
public class AppStartup : PrismStartup
    {
        public MockModuleStartup(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            // Register your services here...
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<MyShinyModule>();
        }
    }
```

!!! note "Note"
    If adding modules here do not add the same module in your PrismApplication!

### Creating a Shiny Module

Ok as if Modularity wasn't confusing enough... now we had to go and add a Shiny Module... The Shiny Module lets us continue to work with the `ServiceCollection` that you may have started with in the Startup class. As you'll see by inheriting from `ShinyModule` or `StartupModule` we can add services that we may need to here.

```c#
public class MyShinyModule : ShinyModule
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<IMockModuleServiceA, MockModuleServiceA>();
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.Register<IMockModuleServiceB, MockModuleServiceB>();
    }
}
```

!!! critical "Critical Note"
    Depending on what you are registering you may need to use a `StartupModule` which forces the registration to be actually at Startup.