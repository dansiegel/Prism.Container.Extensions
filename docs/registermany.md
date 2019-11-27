# RegisterMany

One of the very powerful new methods provided by the Container Extensions is the `RegisterMany` and `RegisterManySingleton` method. This really can help you reduce how much boilerplate code you need to write and provide some advanced scenarios. So what is it?

```c#
public interface IFoo
{
    void DoFoo();
}

public interface IBar
{
    void DoBar();
}
```

To start let's assume that you have 2 interfaces like the ones above `IFoo` and `IBar`. Now let's assume that you have a single implementing type like:

```c#
public class FooBar : IFoo, IBar
{

    public void DoFoo()
    {
        Console.WriteLine("Doing foo");
    }

    public void DoBar()
    {
        Console.WriteLine("Doing Bar");
    }
}
```

Without the Container Extensions you might have a transient registration like:

```c#
containerRegistry.Register<IFoo, FooBar>();
containerRegistry.Register<IBar, FooBar>();
```

While this may not be such a big deal, it suddenly starts making more sense when we expect the use of a singleton. The issue here is that if we were to do something similar to this to register a Singleton traditionally like:

```c#
containerRegistry.RegisterSingleton<IFoo, FooBar>();
containerRegistry.RegisterSingleton<IBar, FooBar>();
```

We are under the impression that we have a singleton here. The issue of course is that if you check for equality like:

```c#
if(Container.Resolve<IFoo>() == Container.Resolve<IBar>())
{
    Console.WriteLine("Foo and Bar are the same instance");
}
else
{
    Console.WriteLine("Foo and Bar are difference instances");
}
```

We might expect that the first case would evaluate to true that Foo and Bar are the same instance, but in reality they are two different instances. The issue isn't that we somehow didn't register them as a singleton because if you resolve IFoo twice and do the same equality check it will actually evaluate to true because they would be the same instance. However, Foo and Bar are different instances because they were registered separately. This is where `RegisterManySingleton` really shines. If we were to update our registration like:

```c#
// Implicitly registers any implemented interfaces
containerRegistry.RegisterManySingleton<FooBar>();

// Explicitly registers implemented interfaces
containerRegistry.RegisterManySingleton<FooBar>(typeof(IFoo), typeof(IBar))
```

We can now perform the same equality check above only this time `IFoo` and `IBar` would equal one another because they would both have been resolved from the same instance of the `FooBar` implementation.
