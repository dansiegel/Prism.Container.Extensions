The use of fluent API's and declartive syntax is gaining popularity. There is a lot to be said for this as it becomes very clear about what we might expect.

## Using the Fluent API

```c#
navService.Navigate("ViewA")
          .WithParameter("message", "Hello World")
          .Catch(e =>
          {
              // Equivalent to catch(Exception e)
          })
          .Catch<ArgumentNullException>(e =>
          {
              // Equivalent to catch(ArgumentNullException e)
          })
          .ExecuteAsync();
```

!!! note "Note"
    When using the Fluent API, ExecuteAsync returns a `Task<bool>` and not a `Task<INavigationResult>`. It is important that if you want to do something with an exception returned by the INavigationResult in the underlying API, you must provide a Catch to handle it in the Fluent API.