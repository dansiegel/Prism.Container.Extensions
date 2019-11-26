## Using the Prism.DryIoc.Forms.Extended

The Prism.DryIoc.Forms.Extended package is designed to make it even easier for you to integrate these fantastic packages. As you'll see using it is identical in every way to creating a typical Prism Application. The only difference is that you are installing the Prism.DryIoc.Forms.Extended package instead of Prism.DryIoc.Forms.

```xml
<prism:PrismApplication xmlns="http://xamarin.com/schemas/2014/forms"
                        xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
                        xmlns:prism="http://prismlibrary.com"
                        x:Class="Contoso.Awesome.App">
</prism:PrismApplication>
```

```c#
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
