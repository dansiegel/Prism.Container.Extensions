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