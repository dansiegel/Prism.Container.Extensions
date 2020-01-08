Prism's IPageBehaviorFactory is a great way to apply some custom behaviors on to pages either globally or with a little business logic. The Extended Prism.Forms package uses a custom PageBehaviorFactory. In addition to the normal behaviors that Prism applies to your Pages behind the scenes, the Extended version provides support for the following:

- Globally adding Platform Specifics on:
  - SetToolbarPlacement on Android TabbedPage
  - Use Safe Area on iOS
  - PreferLargeTitles on iOS
- A custom behavior that changes the Title of a TabbedPage to always match the actively selected Tab

To control these features you simply need to register an implementation of `IPageBehaviorFactoryOptions`.

!!! note "Note"
    A default instance is provided automatically that enables all of these features.

```c#
internal class MyPageBehaviorFactoryOptions : IPageBehaviorFactoryOptions
{
    public bool UseBottomTabs => true;

    public bool UseSafeArea => true;

    public bool UseChildTitle => true;

    public bool PreferLargeTitles => true;
}
```

## Using Explicit Values

While it's great to generalize certain Platform Specifics like `UseBottomTabs` or `UseSafeArea`, there may be times which you prefer to opt-out of these platform specifics from the PageBehaviorFactory and either use the default value or a custom value. For these times you can update your XAML as follows:

```xml
<ContentPage xmlns:prism="http://prismlibrary.com"
             prism:PlatformSpecifics.UseExplicit="true">
```

Or in Code:

```csharp
public class ViewA : ContentPage
{
    public ViewA()
    {
        PlatformSpecifics.SetUseExplicit(this, true);
    }
}
```