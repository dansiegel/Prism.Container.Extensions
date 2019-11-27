DryIoc is a fantastic container to use. It is extremely fast and performant and is the container that Dan recommends the most.

## Customizing the underlying container

There are times that you may want to customize some behavior of the container. For these times you may want to access one of the overload Create methods available.

!!! note "Note"
    This is for ADVANCED USERS ONLY!!! If you do not know what you're doing with the D.I. container do not use these methods!

```c#
var rules = Rules.Default
                 .WithAutoConcreteTypeResolution()
                 .With(Made.Of(FactoryMethod.ConstructorWithResolvableArguments))
                 .WithoutThrowOnRegisteringDisposableTransient()
                 .WithFuncAndLazyWithoutRegistration()
                 .WithDefaultIfAlreadyRegistered(IfAlreadyRegistered.Replace);

// Option 1
PrismContainerExtension.Create(rules);

// Option 2
var container = new Container(rules);
PrismContainerExtension.Create(container);
```