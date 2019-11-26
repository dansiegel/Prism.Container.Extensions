## Initialization

The PrismContainerExtension can be initialized automatically and accessed by simply calling `PrismContainerExtension.Current`. You can also create a new container with any of the following methods:

```c#
// Use Default Prism configuration
PrismContainerExtension.Create();

// Use custom Container with custom rules
PrismContainerExtension.Create(new Container());
```

**NOTE** That by default the container extension will ensure that the underlying container is properly configured to work with Prism Applications.

## Modifying PrismApplication

When using the extended container extension you simply need to add the following to your PrismApplication to ensure that it uses the same instance that may have been created prior to the initialization of PrismApplication.

```c#
protected override IContainerExtension CreateContainerExtension() => PrismContainerExtension.Current;
```

**NOTE:** This section *ONLY* applies to applications that are based on the Official packages from Prism. If you're using an Extended PrismApplication from this repo you do not need to modify the PrismApplication.
