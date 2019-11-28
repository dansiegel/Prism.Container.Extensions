# Register Services

For those coming from an ASP.NET Core background, you're already very familiar with the RegisterServices method in which we can add services to the `IServiceCollection`. In truth this doesn't make as much sense with Prism Applications to fully adopt this pattern. That said there are services which may already ship with Registration helpers for `IServiceCollection`. In these cases rather than making it more difficult on you, the PrismContainerExtensions aim to make your life a little easier by providing a simple extension method that will allow you to register certain services with the `IServiceCollection` which can then update the underlying container.

!!! note "Note"
    For those using the Prism.Microsoft.DependencyInjection.Extensions package, the services are registered directly with the underlying `IServiceCollection`.

```c#
PrismContainerExtension.Current.RegisterServices(s => {
    s.AddHttpClient();
    s.AddDbContext<AwesomeAppDbContext>(o => o.UseSqlite("my connection string"));
})
```