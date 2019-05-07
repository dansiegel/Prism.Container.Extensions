using System;
using System.Collections.Generic;
using System.Diagnostics;
using DryIoc;
using Prism.DryIoc.Extensions.Tests.Mocks;
using Prism.Ioc;
using Xunit;

namespace Prism.DryIoc.Extensions.Tests
{
    public class ContainerTests
    {
        [Fact]
        public void StaticInstanceSameAsNewInstance()
        {
            ExtendedPrismDryIocContainer.Reset();
            var newInstance = new ExtendedPrismDryIocContainer();
            Assert.Same(newInstance, ExtendedPrismDryIocContainer.Current);
        }

        [Fact]
        public void StaticInstanceSameAsCreateInstance()
        {
            ExtendedPrismDryIocContainer.Reset();
            var created = ExtendedPrismDryIocContainer.Create(new Container());
            Assert.Same(created, ExtendedPrismDryIocContainer.Current);
        }

        [Fact]
        public void WarningGeneratedFromMultipleInstances()
        {
            ExtendedPrismDryIocContainer.Reset();
            var listener = new MockListener();
            Trace.Listeners.Add(listener);
            var newInstance1 = new ExtendedPrismDryIocContainer();
            var newInstance2 = new ExtendedPrismDryIocContainer();

            Assert.Single(listener.Messages);
            Assert.NotSame(newInstance1, ExtendedPrismDryIocContainer.Current);
            Assert.Same(newInstance2, ExtendedPrismDryIocContainer.Current);
        }

        [Fact]
        public void RegisterManyHasSameTypeAcrossServices()
        {
            ExtendedPrismDryIocContainer.Reset();
            var c = ExtendedPrismDryIocContainer.Current;
            c.RegisterMany<FooBarImpl>();

            IFoo foo = null;
            IBar bar = null;
            var ex = Record.Exception(() => foo = c.Resolve<IFoo>());

            Assert.Null(ex);
            Assert.NotNull(foo);
            Assert.IsType<FooBarImpl>(foo);

            ex = Record.Exception(() => bar = c.Resolve<IBar>());

            Assert.Null(ex);
            Assert.NotNull(bar);
            Assert.IsType<FooBarImpl>(bar);

            Assert.NotSame(foo, bar);
        }

        [Fact]
        public void RegisterManyHasSameInstanceAcrossServices()
        {
            ExtendedPrismDryIocContainer.Reset();
            var c = ExtendedPrismDryIocContainer.Current;
            c.RegisterManySingleton<FooBarImpl>();

            IFoo foo = null;
            IBar bar = null;
            var ex = Record.Exception(() => foo = c.Resolve<IFoo>());

            Assert.Null(ex);
            Assert.NotNull(foo);
            Assert.IsType<FooBarImpl>(foo);

            ex = Record.Exception(() => bar = c.Resolve<IBar>());

            Assert.Null(ex);
            Assert.NotNull(bar);
            Assert.IsType<FooBarImpl>(bar);

            Assert.Same(foo, bar);
        }

        [Fact]
        public void FactoryCreatesTransientTypeWithoutContainerProvider()
        {
            ExtendedPrismDryIocContainer.Reset();
            var c = ExtendedPrismDryIocContainer.Current;
            var message = "expected";
            c.Register<IFoo>(FooFactory);

            IFoo foo = null;
            var ex = Record.Exception(() => foo = c.Resolve<IFoo>());

            Assert.Null(ex);
            Assert.Equal(message, foo.Message);

            Assert.NotSame(foo, c.Resolve<IFoo>());
        }

        [Fact]
        public void FactoryCreatesTransientTypeWithContainerProvider()
        {
            ExtendedPrismDryIocContainer.Reset();
            var c = ExtendedPrismDryIocContainer.Current;
            var expectedMessage = "constructed with IContainerProvider";
            c.Register<IBar>(BarFactory);
            c.Register<IFoo, Foo>();

            IBar bar = null;
            var ex = Record.Exception(() => bar = c.Resolve<IBar>());

            Assert.Null(ex);
            Assert.False(string.IsNullOrWhiteSpace(bar.Foo.Message));
            Assert.Equal(expectedMessage, bar.Foo.Message);

            Assert.NotSame(bar, c.Resolve<IBar>());
        }

        [Fact]
        public void FactoryCreatesSingletonTypeWithoutContainerProvider()
        {
            ExtendedPrismDryIocContainer.Reset();
            var c = ExtendedPrismDryIocContainer.Current;
            var message = "expected";
            c.RegisterSingleton<IFoo>(FooFactory);

            IFoo foo = null;
            var ex = Record.Exception(() => foo = c.Resolve<IFoo>());

            Assert.Null(ex);
            Assert.Equal(message, foo.Message);

            Assert.Same(foo, c.Resolve<IFoo>());
        }

        [Fact]
        public void FactoryCreatesSingletonTypeWithContainerProvider()
        {
            ExtendedPrismDryIocContainer.Reset();
            var c = ExtendedPrismDryIocContainer.Current;
            var expectedMessage = "constructed with IContainerProvider";
            c.RegisterSingleton<IBar>(BarFactory);
            c.Register<IFoo, Foo>();

            IBar bar = null;
            var ex = Record.Exception(() => bar = c.Resolve<IBar>());

            Assert.Null(ex);
            Assert.False(string.IsNullOrWhiteSpace(bar.Foo.Message));
            Assert.Equal(expectedMessage, bar.Foo.Message);

            Assert.Same(bar, c.Resolve<IBar>());
        }

        static IFoo FooFactory() => new Foo { Message = "expected" };

        static IBar BarFactory(IContainerProvider containerProvider) =>
            containerProvider.Resolve<IBar>((typeof(IFoo), new Foo { Message = "constructed with IContainerProvider" }));
    }

    internal class MockListener : TraceListener
    {
        public readonly List<string> Messages = new List<string>();

        public override void Write(string message)
        {

        }

        public override void WriteLine(string message)
        {
            Messages.Add(message);
        }
    }
}
