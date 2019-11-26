#### Navigation from Shiny Services

While this is generally not a great idea, there could potentially be times in which you need to navigate from a background service. For these times you will need to use the INavigationServiceDelegate.

```c#
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