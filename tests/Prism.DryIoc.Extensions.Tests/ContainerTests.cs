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
            var newInstance = PrismContainerExtension.Create();
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
            var newInstance1 = PrismContainerExtension.Create();
            var newInstance2 = PrismContainerExtension.Create();

            Assert.Single(listener.Messages);
            Assert.NotSame(newInstance1, PrismContainerExtension.Current);
            Assert.Same(newInstance2, PrismContainerExtension.Current);
        }

        [Fact]
        public void IServiceProviderIsRegistered()
        {
            PrismContainerExtension.Reset();
            Assert.True(PrismContainerExtension.Current.IsRegistered<IServiceProvider>());
        }

        [Fact]
        public void IContainerProviderIsRegistered()
        {
            PrismContainerExtension.Reset();
            Assert.True(PrismContainerExtension.Current.IsRegistered<IContainerProvider>());
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
        public void RegisterTransientService()
        {
            PrismContainerExtension.Reset();
            var c = PrismContainerExtension.Current;
            c.Register<IFoo, Foo>();
            IFoo foo = null;
            var ex = Record.Exception(() => foo = c.Resolve<IFoo>());

            Assert.Null(ex);
            Assert.NotNull(foo);
            Assert.IsType<Foo>(foo);
        }

        [Fact]
        public void RegisterTransientNamedService()
        {
            PrismContainerExtension.Reset();
            var c = PrismContainerExtension.Current;
            c.Register<IFoo, Foo>("fooBar");
            IFoo foo = null;
            var ex = Record.Exception(() => foo = c.Resolve<IFoo>());

            Assert.NotNull(ex);

            ex = null;
            ex = Record.Exception(() => foo = c.Resolve<IFoo>("fooBar"));

            Assert.Null(ex);
            Assert.NotNull(foo);
            Assert.IsType<Foo>(foo);
        }

        [Fact]
        public void RegisterSingletonService()
        {
            PrismContainerExtension.Reset();
            var c = PrismContainerExtension.Current;
            c.RegisterSingleton<IFoo, Foo>();
            IFoo foo = null;
            var ex = Record.Exception(() => foo = c.Resolve<IFoo>());

            Assert.Null(ex);
            Assert.NotNull(foo);
            Assert.IsType<Foo>(foo);

            Assert.Same(foo, c.Resolve<IFoo>());
        }

        [Fact]
        public void RegisterInstanceResolveSameInstance()
        {
            PrismContainerExtension.Reset();
            var c = PrismContainerExtension.Current;
            var foo = new Foo();

            c.RegisterInstance<IFoo>(foo);

            Assert.True(c.IsRegistered<IFoo>());
            Assert.Same(foo, c.Resolve<IFoo>());
        }

        [Fact]
        public void RegisterInstanceResolveSameNamedInstance()
        {
            PrismContainerExtension.Reset();
            var c = PrismContainerExtension.Current;
            var foo = new Foo();

            c.RegisterInstance<IFoo>(foo, "test");

            Assert.True(c.IsRegistered<IFoo>("test"));
            Assert.Same(foo, c.Resolve<IFoo>("test"));
        }

        [Fact]
        public void RegisterSingletonNamedService()
        {
            PrismContainerExtension.Reset();
            var c = PrismContainerExtension.Current;
            c.RegisterSingleton<IFoo, Foo>("fooBar");
            IFoo foo = null;
            var ex = Record.Exception(() => foo = c.Resolve<IFoo>());

            Assert.NotNull(ex);

            ex = null;
            ex = Record.Exception(() => foo = c.Resolve<IFoo>("fooBar"));

            Assert.Null(ex);
            Assert.NotNull(foo);
            Assert.IsType<Foo>(foo);

            Assert.Same(foo, c.Resolve<IFoo>("fooBar"));
        }

        [Fact]
        public void FactoryCreatesTransientTypeWithoutContainerProvider()
        {
            PrismContainerExtension.Reset();
            var c = PrismContainerExtension.Current;
            var message = "expected";
            c.RegisterDelegate<IFoo>(FooFactory);

            IFoo foo = null;
            var ex = Record.Exception(() => foo = c.Resolve<IFoo>());

            Assert.Null(ex);
            Assert.Equal(message, foo.Message);

            Assert.NotSame(foo, c.Resolve<IFoo>());
        }

        [Fact]
        public void FactoryCreatesSingletonTypeWithoutContainerProvider()
        {
            PrismContainerExtension.Reset();
            var c = PrismContainerExtension.Current;
            var message = "expected";
            c.RegisterSingletonFromDelegate<IFoo>(FooFactory);

            IFoo foo = null;
            var ex = Record.Exception(() => foo = c.Resolve<IFoo>());

            Assert.Null(ex);
            Assert.Equal(message, foo.Message);

            Assert.Same(foo, c.Resolve<IFoo>());
        }

        [Fact]
        public void FactoryCreatesTransientTypeWithServiceProvider()
        {
            PrismContainerExtension.Reset();
            var c = PrismContainerExtension.Current;
            var expectedMessage = "constructed with IServiceProvider";
            c.RegisterDelegate(typeof(IBar), BarFactoryWithIServiceProvider);
            c.RegisterInstance<IFoo>(new Foo { Message = expectedMessage });

            IBar bar = null;
            var ex = Record.Exception(() => bar = c.Resolve<IBar>());

            Assert.Null(ex);
            Assert.False(string.IsNullOrWhiteSpace(bar.Foo.Message));
            Assert.Equal(expectedMessage, bar.Foo.Message);

            Assert.NotSame(bar, c.Resolve<IBar>());
        }

        [Fact]
        public void FactoryCreatesTransientTypeWithServiceProviderFromGeneric()
        {
            PrismContainerExtension.Reset();
            var c = PrismContainerExtension.Current;
            var expectedMessage = "constructed with IServiceProvider";
            c.RegisterDelegate<IBar>(BarFactoryWithIServiceProvider);
            c.RegisterInstance<IFoo>(new Foo { Message = expectedMessage });

            IBar bar = null;
            var ex = Record.Exception(() => bar = c.Resolve<IBar>());

            Assert.Null(ex);
            Assert.False(string.IsNullOrWhiteSpace(bar.Foo.Message));
            Assert.Equal(expectedMessage, bar.Foo.Message);

            Assert.NotSame(bar, c.Resolve<IBar>());
        }

        [Fact]
        public void FactoryCreatesSingletonTypeWithServiceProvider()
        {
            PrismContainerExtension.Reset();
            var c = PrismContainerExtension.Current;
            var expectedMessage = "constructed with IServiceProvider";
            c.RegisterSingletonFromDelegate(typeof(IBar), BarFactoryWithIServiceProvider);
            c.RegisterInstance<IFoo>(new Foo { Message = expectedMessage });

            IBar bar = null;
            var ex = Record.Exception(() => bar = c.Resolve<IBar>());

            Assert.Null(ex);
            Assert.False(string.IsNullOrWhiteSpace(bar.Foo.Message));
            Assert.Equal(expectedMessage, bar.Foo.Message);

            Assert.Same(bar, c.Resolve<IBar>());
        }

        [Fact]
        public void FactoryCreatesSingletonTypeWithServiceProviderFromGeneric()
        {
            PrismContainerExtension.Reset();
            var c = PrismContainerExtension.Current;
            var expectedMessage = "constructed with IServiceProvider";
            c.RegisterSingletonFromDelegate<IBar>(BarFactoryWithIServiceProvider);
            c.RegisterInstance<IFoo>(new Foo { Message = expectedMessage });

            IBar bar = null;
            var ex = Record.Exception(() => bar = c.Resolve<IBar>());

            Assert.Null(ex);
            Assert.False(string.IsNullOrWhiteSpace(bar.Foo.Message));
            Assert.Equal(expectedMessage, bar.Foo.Message);

            Assert.Same(bar, c.Resolve<IBar>());
        }

        [Fact]
        public void FactoryCreatesTransientTypeWithContainerProvider()
        {
            PrismContainerExtension.Reset();
            var c = PrismContainerExtension.Current;

            var expectedMessage = "constructed with IContainerProvider";
            c.RegisterDelegate(typeof(IBar), BarFactoryWithIContainerProvider);
            c.RegisterSingleton<IFoo, Foo>();

            IBar bar = null;
            var ex = Record.Exception(() => bar = c.Resolve<IBar>());

            Assert.Null(ex);
            Assert.IsType<Bar>(bar);
            Assert.Equal(expectedMessage, ((Bar)bar).Message);

            Assert.Same(c.Resolve<IFoo>(), bar.Foo);
            Assert.NotSame(c.Resolve<IBar>(), bar);
        }

        [Fact]
        public void FactoryCreatesTransientTypeWithContainerProviderWithGeneric()
        {
            PrismContainerExtension.Reset();
            var c = PrismContainerExtension.Current;

            var expectedMessage = "constructed with IContainerProvider";
            c.RegisterDelegate<IBar>(BarFactoryWithIContainerProvider);
            c.RegisterSingleton<IFoo, Foo>();

            IBar bar = null;
            var ex = Record.Exception(() => bar = c.Resolve<IBar>());

            Assert.Null(ex);
            Assert.IsType<Bar>(bar);
            Assert.Equal(expectedMessage, ((Bar)bar).Message);

            Assert.Same(c.Resolve<IFoo>(), bar.Foo);
            Assert.NotSame(c.Resolve<IBar>(), bar);
        }

        [Fact]
        public void FactoryCreatesSingletonTypeWithContainerProvider()
        {
            PrismContainerExtension.Reset();
            var c = PrismContainerExtension.Current;

            var expectedMessage = "constructed with IContainerProvider";
            c.RegisterSingletonFromDelegate(typeof(IBar), BarFactoryWithIContainerProvider);
            c.RegisterSingleton<IFoo, Foo>();

            IBar bar = null;
            var ex = Record.Exception(() => bar = c.Resolve<IBar>());

            Assert.Null(ex);
            Assert.IsType<Bar>(bar);
            Assert.Equal(expectedMessage, ((Bar)bar).Message);

            Assert.Same(c.Resolve<IFoo>(), bar.Foo);
            Assert.Same(c.Resolve<IBar>(), bar);
        }

        [Fact]
        public void FactoryCreatesSingletonTypeWithContainerProviderWithGeneric()
        {
            PrismContainerExtension.Reset();
            var c = PrismContainerExtension.Current;

            var expectedMessage = "constructed with IContainerProvider";
            c.RegisterSingletonFromDelegate<IBar>(BarFactoryWithIContainerProvider);
            c.RegisterSingleton<IFoo, Foo>();

            IBar bar = null;
            var ex = Record.Exception(() => bar = c.Resolve<IBar>());

            Assert.Null(ex);
            Assert.IsType<Bar>(bar);
            Assert.Equal(expectedMessage, ((Bar)bar).Message);

            Assert.Same(c.Resolve<IFoo>(), bar.Foo);
            Assert.Same(c.Resolve<IBar>(), bar);
        }

        [Fact]
        public void ResolveWithSpecifiedTypeOverridesRegistration()
        {
            PrismContainerExtension.Reset();
            var c = PrismContainerExtension.Current;
            c.Register<IBar, Bar>();
            var foo = new Foo { Message = "This shouldn't be resolved" };
            c.RegisterInstance<IFoo>(foo);

            var overrideFoo = new Foo { Message = "We expect this one" };

            Assert.Same(foo, c.Resolve<IFoo>());

            var bar = c.Resolve<IBar>((typeof(IFoo), overrideFoo));
            Assert.Same(overrideFoo, bar.Foo);
        }

        public static IFoo FooFactory() => new Foo { Message = "expected" };

        public static IBar BarFactoryWithIContainerProvider(IContainerProvider containerProvider) =>
            new Bar(containerProvider.Resolve<IFoo>())
            {
                Message = "constructed with IContainerProvider"
            };

        public static IBar BarFactoryWithIServiceProvider(IServiceProvider serviceProvider) =>
            new Bar((IFoo)serviceProvider.GetService(typeof(IFoo)));
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
