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
            PrismContainerExtension.Reset();
            var newInstance = new PrismContainerExtension();
            Assert.Same(newInstance, PrismContainerExtension.Current);
        }

        [Fact]
        public void StaticInstanceSameAsCreateInstance()
        {
            PrismContainerExtension.Reset();
            var created = PrismContainerExtension.Create(new Container());
            Assert.Same(created, PrismContainerExtension.Current);
        }

        [Fact]
        public void WarningGeneratedFromMultipleInstances()
        {
            PrismContainerExtension.Reset();
            var listener = new MockListener();
            Trace.Listeners.Add(listener);
            var newInstance1 = new PrismContainerExtension();
            var newInstance2 = new PrismContainerExtension();

            Assert.Single(listener.Messages);
            Assert.NotSame(newInstance1, PrismContainerExtension.Current);
            Assert.Same(newInstance2, PrismContainerExtension.Current);
        }

        [Fact]
        public void RegisterManyHasSameTypeAcrossServices()
        {
            PrismContainerExtension.Reset();
            var c = PrismContainerExtension.Current;
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
            PrismContainerExtension.Reset();
            var c = PrismContainerExtension.Current;
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
            PrismContainerExtension.Reset();
            var c = PrismContainerExtension.Current;
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
            PrismContainerExtension.Reset();
            var c = PrismContainerExtension.Current;
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
            PrismContainerExtension.Reset();
            var c = PrismContainerExtension.Current;
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
            PrismContainerExtension.Reset();
            var c = PrismContainerExtension.Current;
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
