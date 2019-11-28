The Prism.Container.Extensions project currently has support for 3 third party Dependency Injection Containers:

- <a href="https://github.com/dadhi/DryIoc" target="_blank">DryIoc</a>
- <a href="https://github.com/aspnet/Extensions" target="_blank">Microsoft.Extensions.DependencyInjection</a>
- <a href="https://github.com/unitycontainer/unity" target="_blank">Unity Container</a> - (not to be confused with Unity Game Development...)

## Using the Containers

All of the implementations follow some common practices for accessing the containers.

!!! important "Important"
    There is absolutely ZERO that ties these container implementations to any platform. You can safely use them for WPF, Xamarin.Forms, or other platforms not officially supported by Prism. All examples shown here will be referencing the use for Xamarin applications.

To access the container at any point you can access the current instance of the container from the `Current` property. Note that if no instance has been created yet, it will automatically initialize the container with our default configuration for the container.

```c#
var container = PrismContainerExtension.Current;

// Creates a new instance with the default configuration
var container = PrismContainerExtension.Create();
```

!!! warning "Warning"
    Create should only ever be called a single time. For this reason if an instance has already been created, subsquent calls to Create will result in a `NotSupportedException` being thrown.

## Modifying PrismApplication

When using the extended container extension you simply need to add the following to your PrismApplication to ensure that it uses the same instance that may have been created prior to the initialization of PrismApplication.

```c#
protected override IContainerExtension CreateContainerExtension() => PrismContainerExtension.Current;
```

**NOTE:** This section *ONLY* applies to applications that are based on the Official packages from Prism. If you're using an Extended PrismApplication from this repo you should not modify the PrismApplication.
