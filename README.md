# Prism.Container.Extensions

The Prism Container Extensions provide various additional extensions making the Prism Container easier to use with Splat, IServiceCollection/IServiceProvider and in scenarios where you may require a Singleton container that may need to be initialized from Platform specific code prior to PrismApplication being created. Note that both the Prism.Container.Extensions and Prism.DryIoc.Extensions are platform agnostic meaning you can use them on WPF or Xamarin Forms.

| Stage | Status |
|:-----:|--------|
| Build Extensions | [![Build Status](https://dev.azure.com/dansiegel/Prism.Plugins/_apis/build/status/dansiegel.Prism.Container.Extensions?branchName=master&stageName=Build%20NuGet%20Packages&jobName=Build%20Container%20Extensions)](https://dev.azure.com/dansiegel/Prism.Plugins/_build/latest?definitionId=41&branchName=master) |
| MyGet Deploy | [![Build Status](https://dev.azure.com/dansiegel/Prism.Plugins/_apis/build/status/dansiegel.Prism.Container.Extensions?branchName=master&stageName=Deploy%20NuGets&jobName=MyGet.org)](https://dev.azure.com/dansiegel/Prism.Plugins/_build/latest?definitionId=41&branchName=master) |
| NuGet Deploy | [![Build Status](https://dev.azure.com/dansiegel/Prism.Plugins/_apis/build/status/dansiegel.Prism.Container.Extensions?branchName=master&stageName=Deploy%20NuGets&jobName=NuGet.org)](https://dev.azure.com/dansiegel/Prism.Plugins/_build/latest?definitionId=41&branchName=master) |

## NuGet

You can add the MyGet CI feed to nuget by adding it as a source in Visual Studio:

`https://www.myget.org/F/prism-plugins/api/v3/index.json`

| Package | NuGet | MyGet |
|-------|:-----:|:------:|
| Prism.Container.Extensions | [![ContainerExtensionsShield]][ContainerExtensionsNuGet] | [![ContainerExtensionsMyGetShield]][ContainerExtensionsMyGet] |
| Prism.Forms.Extended | [![PrismFormsExtendedShield]][PrismFormsExtendedNuGet] | [![PrismFormsExtendedMyGetShield]][PrismFormsExtendedMyGet] |
| Prism.DryIoc.Extensions | [![DryIocExtensionsShield]][DryIocExtensionsNuGet] | [![DryIocExtensionsMyGetShield]][DryIocExtensionsMyGet] |
| Prism.DryIoc.Forms.Extended | [![DryIocFormsExtendedShield]][DryIocFormsExtendedNuGet] | [![DryIocFormsExtendedMyGetShield]][DryIocFormsExtendedMyGet] |
| Prism.Unity.Extensions | [![UnityExtensionsShield]][UnityExtensionsNuGet] | [![UnityExtensionsMyGetShield]][UnityExtensionsMyGet] |
| Prism.Unity.Forms.Extended | [![UnityFormsExtendedShield]][UnityFormsExtendedNuGet] | [![UnityFormsExtendedMyGetShield]][UnityFormsExtendedMyGet] |
| Shiny.Prism | [![ShinyPrismShield]][ShinyPrismNuGet] | [![ShinyPrismMyGetShield]][ShinyPrismMyGet] |

## Initialization

The PrismContainerExtension can be initialized automatically and accessed by simply calling `PrismContainerExtension.Current`. You can also create a new container with any of the following methods:

```cs
// Use Default Prism configuration
PrismContainerExtension.Create();

// Use custom Container with custom rules
PrismContainerExtension.Create(new Container());
```

**NOTE** That by default the container extension will ensure that the underlying container is properly configured to work with Prism Applications.

## Modifying PrismApplication

When using the extended container extension you simply need to add the following to your PrismApplication to ensure that it uses the same instance that may have been created prior to the initialization of PrismApplication.

```cs
protected override IContainerExtension CreateContainerExtension() => PrismContainerExtension.Current;
```

**NOTE:** This section *ONLY* applies to applications that are based on the Official packages from Prism. If you're using an Extended PrismApplication from this repo you do not need to modify the PrismApplication.

## Using the Prism.DryIoc.Forms.Extended

The Prism.DryIoc.Forms.Extended package is designed to make it even easier for you to integrate these fantastic packages. As you'll see using it is identical in every way to creating a typical Prism Application. The only difference is that you are installing the Prism.DryIoc.Forms.Extended package instead of Prism.DryIoc.Forms.

```xml
<prism:PrismApplication xmlns="http://xamarin.com/schemas/2014/forms"
                        xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
                        xmlns:prism="http://prismlibrary.com"
                        x:Class="Contoso.Awesome.App">
</prism:PrismApplication>
```

```cs
public partial class App
{
    protected override void OnInitialized()
    {
        InitializeComponent();
        NavigationService.NavigateAsync("MainPage");
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        // Register your services like normal
        containerRegistry.RegisterForNavigation<MainPage>();
    }
}
```

### Why use Prism.DryIoc.Forms.Extended

The extended PrismApplication is cross compiled for Xamarin.iOS and Xamarin.Android and provides several out of the box improvements over the normal PrismApplication.

- It includes the `ILogger` from [Prism.Plugin.Logging](https://github.com/dansiegel/Prism.Plugin.Logging)
- It has pre-wired support for logging XAML errors and other issues directly from Xamarin.Forms
- Becase it is cross compiled, it has global Exception Handling built in for:
  - AndroidEnvironment
  - ObjectiveC.Runtime
  - AppDomain
  - TaskScheduler
- It provides you all of the container extensions found here that help make advanced registration scenarios much easier.

```xml
<prism:PrismApplication xmlns="http://xamarin.com/schemas/2014/forms"
                        xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
                        xmlns:prism="http://prismlibrary.com"
                        x:Class="Contoso.Awesome.App">
  <prism:PrismApplication.ModuleCatalog>
    <prism:ModuleCatalog>
      <prism:ModuleInfo />
    </prism.ModuleCatalog>
  </prism:PrismApplication.ModuleCatalog>
</prism:PrismApplication>
```

## Working With Shiny

[Shiny](https://github.com/shinyorg/shiny) uses the Microsoft.Extensions.DependencyInjection pattern found in ASP.NET Core applications with a Startup class. This in particular is a use case in which you will need to initialize a container prior to Forms.Init being called on the native platform. To work with Shiny you simply need to do something like the following:

```cs
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

```cs
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

```cs
// Android
public class App : Android.App.Application
{
    public override void OnCreate()
    {
        AndroidShinyHost.Init(this, new MyStartup());
    }
}

// iOS
[Register("AppDelegate")]
public partial class AppDelegate : FormsApplicationDelegate
{
    public override bool FinishedLaunching(UIApplication app, NSDictionary options)
    {
        // this needs to be loaded before EVERYTHING
        iOSShinyHost.Init(new MyStartup());

        Forms.Init();
        this.LoadApplication(new App());
        return base.FinishedLaunching(app, options);
    }

    // if you are using jobs, you need this
    public override void PerformFetch(UIApplication application, Action<UIBackgroundFetchResult> completionHandler)
        => JobManager.OnBackgroundFetch(completionHandler);
}
```

#### Navigation from Shiny Services

While this is generally not a great idea, there could potentially be times in which you need to navigate from a background service. For these times you will need to use the INavigationServiceDelegate.

```cs
public class MyStartup : PrismStartup
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.UseNavigationDelegate();
    }
}

public class MyJob : IJob
{
    private INavigationServiceDelegate NavigationService { get; }

    public MyJob(INavigationServiceDelegate navigationServiceDelegate)
    {
        NavigationService = navigationServiceDelegate;
    }

    public async Task<bool> Run(JobInfo jobInfo, CancellationToken cancelToken)
    {
        // Your logic
        await NavigationService.NavigateAsync("SomePage?useModal=true");
    }
}
```

[ContainerExtensionsNuGet]: https://www.nuget.org/packages/Prism.Container.Extensions
[ContainerExtensionsShield]: https://img.shields.io/nuget/vpre/Prism.Container.Extensions.svg
[ContainerExtensionsMyGet]: https://www.myget.org/feed/prism-plugins/package/nuget/Prism.Container.Extensions
[ContainerExtensionsMyGetShield]: https://img.shields.io/myget/prism-plugins/vpre/Prism.Container.Extensions.svg

[DryIocExtensionsNuGet]: https://www.nuget.org/packages/Prism.DryIoc.Extensions
[DryIocExtensionsShield]: https://img.shields.io/nuget/vpre/Prism.DryIoc.Extensions.svg
[DryIocExtensionsMyGet]: https://www.myget.org/feed/prism-plugins/package/nuget/Prism.DryIoc.Extensions
[DryIocExtensionsMyGetShield]: https://img.shields.io/myget/prism-plugins/vpre/Prism.DryIoc.Extensions.svg

[DryIocFormsExtendedNuGet]: https://www.nuget.org/packages/Prism.DryIoc.Forms.Extended
[DryIocFormsExtendedShield]: https://img.shields.io/nuget/vpre/Prism.DryIoc.Forms.Extended.svg
[DryIocFormsExtendedMyGet]: https://www.myget.org/feed/prism-plugins/package/nuget/Prism.DryIoc.Forms.Extended
[DryIocFormsExtendedMyGetShield]: https://img.shields.io/myget/prism-plugins/vpre/Prism.DryIoc.Forms.Extended.svg

[ShinyPrismNuGet]: https://www.nuget.org/packages/Shiny.Prism
[ShinyPrismShield]: https://img.shields.io/nuget/vpre/Shiny.Prism.svg
[ShinyPrismMyGet]: https://www.myget.org/feed/prism-plugins/package/nuget/Shiny.Prism
[ShinyPrismMyGetShield]: https://img.shields.io/myget/prism-plugins/vpre/Shiny.Prism.svg

[PrismFormsExtendedNuGet]: https://www.nuget.org/packages/Prism.Forms.Extended
[PrismFormsExtendedShield]: https://img.shields.io/nuget/vpre/Prism.Forms.Extended.svg
[PrismFormsExtendedMyGet]: https://www.myget.org/feed/prism-plugins/package/nuget/Prism.Forms.Extended
[PrismFormsExtendedMyGetShield]: https://img.shields.io/myget/prism-plugins/vpre/Prism.Forms.Extended.svg

[UnityExtensionsNuGet]: https://www.nuget.org/packages/Prism.Unity.Extensions
[UnityExtensionsShield]: https://img.shields.io/nuget/vpre/Prism.Unity.Extensions.svg
[UnityExtensionsMyGet]: https://www.myget.org/feed/prism-plugins/package/nuget/Prism.Unity.Extensions
[UnityExtensionsMyGetShield]: https://img.shields.io/myget/prism-plugins/vpre/Prism.Unity.Extensions.svg

[UnityFormsExtendedNuGet]: https://www.nuget.org/packages/Prism.Unity.Forms.Extended
[UnityFormsExtendedShield]: https://img.shields.io/nuget/vpre/Prism.Unity.Forms.Extended.svg
[UnityFormsExtendedMyGet]: https://www.myget.org/feed/prism-plugins/package/nuget/Prism.Unity.Forms.Extended
[UnityFormsExtendedMyGetShield]: https://img.shields.io/myget/prism-plugins/vpre/Prism.Unity.Forms.Extended.svg
