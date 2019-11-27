#### Navigation from Shiny Services

While this is generally not a great idea, there could potentially be times in which you need to navigate from a background service. For these times you will need to use the INavigationServiceDelegate. It is important to understand that the NavigationServiceDelegate differs from the NavigationService that you are used to using as it will attempt to determine what the currently displayed page is and Navigate from there. This will do exactly what you want if you are resetting the NavigationStack but may not work as expected if you are doing some sort of relative navigation.

```c#
public class MyStartup : PrismStartup
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.UseNavigationDelegate();
    }
}

public class NotificationDelegate : INotificationDelegate
{
    private INavigationServiceDelegate NavigationService { get; }

    public NotificationDelegate(INavigationServiceDelegate navigationServiceDelegate)
    {
        NavigationService = navigationServiceDelegate;
    }

    public async Task OnEntry(NotificationResponse response)
    {
        if(!string.IsNullOrEmpty(notification.Payload))
        {
            await NavigationService.NavigateAsync(notification.Payload);
        }
    }

    public Task OnReceived(Notification notification)
    {
        if(!string.IsNullOrEmpty(notification.Payload))
        {
            await NavigationService.NavigateAsync(notification.Payload);
        }
    }
}
```