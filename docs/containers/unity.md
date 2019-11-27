## Customizing the underlying container

There are times that you may want to customize some behavior of the container. For these times you may want to access one of the overload Create methods available.

!!! note "Note"
    This is for ADVANCED USERS ONLY!!! If you do not know what you're doing with the D.I. container do not use these methods!

```c#
var container = new UnityContainer();
PrismContainerExtension.Create(container);
```