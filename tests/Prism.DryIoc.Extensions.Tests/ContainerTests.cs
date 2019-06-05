using System;
using System.Collections.Generic;
using System.Diagnostics;
using DryIoc;
using Prism.DryIoc.Extensions.Tests.Mocks;
using Prism.Ioc;
using Xunit;
using Xunit.Abstractions;

namespace Prism.DryIoc.Extensions.Tests
{
    public class ContainerTests
    {
        private ITestOutputHelper _testOutputHelper;

        public ContainerTests(ITestOutputHelper testOutputHelper) =>
            _testOutputHelper = testOutputHelper;

        [Fact]
        public void StaticInstanceSameAsNewInstance()
        {
            PrismContainerExtension.Reset();
            GC.Collect();
            var newInstance = PrismContainerExtension.Create();
            Assert.Same(newInstance, PrismContainerExtension.Current);
        }

        [Fact]
        public void StaticInstanceSameAsCreateInstance()
        {
            PrismContainerExtension.Reset();
            GC.Collect();
            var created = PrismContainerExtension.Create(new Container());
            Assert.Same(created, PrismContainerExtension.Current);
        }

        [Fact]
        public void CreateCanOnlyBeCalledOnce()
        {
            var newInstance1 = CreateContainer();
            Assert.Same(newInstance1, PrismContainerExtension.Current);

            var ex = Record.Exception(() => PrismContainerExtension.Create());
            Assert.NotNull(ex);
            Assert.IsType<NotSupportedException>(ex);
        }

        [Fact]
        public void IServiceProviderIsRegistered()
        {
            PrismContainerExtension.Reset();
            GC.Collect();
            Assert.True(PrismContainerExtension.Current.IsRegistered<IServiceProvider>());
        }

        [Fact]
        public void IContainerProviderIsRegistered()
        {
            PrismContainerExtension.Reset();
            GC.Collect();
            Assert.True(PrismContainerExtension.Current.IsRegistered<IContainerProvider>());
        }

        [Fact]
        public void RegisterManyHasSameTypeAcrossServices()
        {
            var c = CreateContainer();
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
            var c = CreateContainer();
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
            var c = CreateContainer();
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
            var c = CreateContainer();
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
            var c = CreateContainer();
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
            var c = CreateContainer();
            var foo = new Foo();

            c.RegisterInstance<IFoo>(foo);

            Assert.True(c.IsRegistered<IFoo>());
            Assert.Same(foo, c.Resolve<IFoo>());
        }

        [Fact]
        public void RegisterInstanceResolveSameNamedInstance()
        {
            var c = CreateContainer();
            var foo = new Foo();

            c.RegisterInstance<IFoo>(foo, "test");

            Assert.True(c.IsRegistered<IFoo>("test"));
            Assert.Same(foo, c.Resolve<IFoo>("test"));
        }

        [Fact]
        public void RegisterSingletonNamedService()
        {
            var c = CreateContainer();
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
            var c = CreateContainer();
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
            var c = CreateContainer();
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
            var c = CreateContainer();
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
            var c = CreateContainer();
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
            var c = CreateContainer();
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
            try
            {
                var c = CreateContainer();
                Assert.NotNull(c);
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
            catch (Exception ex)
            {
                _testOutputHelper.WriteLine(ex.ToString());
                throw;
            }
            
        }

        [Fact]
        public void FactoryCreatesTransientTypeWithContainerProvider()
        {
            var c = CreateContainer();

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
            var c = CreateContainer();

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
            var c = CreateContainer();

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
            var c = CreateContainer();

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
            var c = CreateContainer();
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

        private static IContainerExtension CreateContainer()
        {
            PrismContainerExtension.Reset();
            GC.Collect();
            return PrismContainerExtension.Current;
        }
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
