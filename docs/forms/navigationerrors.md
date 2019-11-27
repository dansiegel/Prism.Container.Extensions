Errors happen when navigating. Using the PrismApplication in Prism.Forms.Extended allows you to automatically and globally handle these errors.

## Default Behavior

- Writes to the Console that there was a Navigation Error
- Resolves the ILogger, and Reports the Error with the following additional properties
  - Serializes the NavigationParameters to a JSON string
  - Adds the Navigation Uri

## Overriding the Default Behavior

If the output on the NavigationError isn't quite what you're hoping for you will need to override the `OnNavigationError` in `PrismApplication`. As an example below we will maintain the same logging behavior but ensure that while debugging we always hit a breakpoint so we can explore the values of the Navigation Error anytime this is encountered.

```c#
protected override void OnNavigationError(INavigationError navigationError)
{
#if DEBUG
    // Ensure we always break here while debugging!
    System.Diagnostics.Debugger.Break();
#endif

    base.OnNavigationError(navigationError);
}
```
