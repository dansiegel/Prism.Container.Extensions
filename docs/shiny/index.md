# Working With Shiny

[Shiny](https://github.com/shinyorg/shiny) uses the Microsoft.Extensions.DependencyInjection pattern of service registration found in ASP.NET Core applications with a Startup class. This in particular is a use case in which you will need to initialize a container prior to Forms.Init being called on the native platform. To work with Shiny you simply need to do something like the following:


```c#
// Android
[Application]
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

