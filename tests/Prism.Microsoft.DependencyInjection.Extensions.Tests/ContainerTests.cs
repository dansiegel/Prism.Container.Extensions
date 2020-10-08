using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Prism.Container.Extensions.Shared.Mocks;
using Prism.Container.Extensions.Shared.Tests;
using Prism.Container.Extensions.Tests.Mocks;
using Prism.Ioc;
using Xunit;
using Xunit.Abstractions;

namespace Prism.Microsoft.DependencyInjection.Extensions.Tests
{
    [Collection(nameof(SharedTests))]
    public class ContainerTests
    {
        private readonly object testLock = new object();
        private ITestOutputHelper _testOutputHelper;

        public ContainerTests(ITestOutputHelper testOutputHelper) =>
            _testOutputHelper = testOutputHelper;

        [Fact]
        public void StaticInstanceSameAsNewInstance()
        {
            lock (testLock)
            {
                PrismContainerExtension.Reset();
                GC.Collect();
                var newInstance = PrismContainerExtension.Init();
                Assert.Same(newInstance, PrismContainerExtension.Current);
            }
        }

        [Fact]
        public void StaticInstanceSameAsCreateInstance()
        {
            lock (testLock)
            {
                PrismContainerExtension.Reset();
                GC.Collect();
                var created = PrismContainerExtension.Init(new ServiceCollection());
                Assert.Same(created, PrismContainerExtension.Current);
            }
        }

        [Fact]
        public void CreateCanOnlyBeCalledOnce()
        {
            lock (testLock)
            {
                var newInstance1 = CreateContainer();
                Assert.Same(newInstance1, PrismContainerExtension.Current);

                var ex = Record.Exception(() => PrismContainerExtension.Init());
                Assert.NotNull(ex);
                Assert.IsType<NotSupportedException>(ex);
            }
        }

        [Fact]
        public void IServiceProviderIsRegistered()
        {
            lock (testLock)
            {
                PrismContainerExtension.Reset();
                GC.Collect();
                Assert.True(((IContainerProvider)PrismContainerExtension.Current).IsRegistered<IServiceProvider>());
            }
        }

        [Fact]
        public void IContainerProviderIsRegistered()
        {
            lock (testLock)
            {
                PrismContainerExtension.Reset();
                GC.Collect();
                Assert.True(((IContainerProvider)PrismContainerExtension.Current).IsRegistered<IContainerProvider>());
            }
        }

        [Fact]
        public void RegisterManyHasSameTypeAcrossServices()
        {
            lock (testLock)
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
        }

        [Fact]
        public void RegisterManyHasSameInstanceAcrossServices()
        {
            lock (testLock)
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
        }

        [Fact]
        public void RegisterTransientService()
        {
            lock (testLock)
            {
                var c = CreateContainer();
                c.Register<IFoo, Foo>();
                IFoo foo = null;
                var ex = Record.Exception(() => foo = c.Resolve<IFoo>());

                Assert.Null(ex);
                Assert.NotNull(foo);
                Assert.IsType<Foo>(foo);
            }
        }

        [Fact]
        public void RegisterTransientNamedService()
        {
            lock (testLock)
            {
                var c = CreateContainer();
                c.Register<IFoo, Foo>("fooBar");
                IFoo foo = null;
                var ex = Record.Exception(() => foo = c.Resolve<IFoo>());

                Assert.Null(foo);

                ex = null;
                ex = Record.Exception(() => foo = c.Resolve<IFoo>("fooBar"));

                Assert.Null(ex);
                Assert.NotNull(foo);
                Assert.IsType<Foo>(foo);
            }
        }

        [Fact]
        public void RegisterSingletonService()
        {
            lock (testLock)
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
        }

        [Fact]
        public void RegisterInstanceResolveSameInstance()
        {
            lock (testLock)
            {
                var c = CreateContainer();
                var foo = new Foo();

                c.RegisterInstance<IFoo>(foo);

                Assert.True(((IContainerProvider)c).IsRegistered<IFoo>());
                Assert.Same(foo, c.Resolve<IFoo>());
            }
        }

        [Fact]
        public void RegisterInstanceResolveSameNamedInstance()
        {
            lock (testLock)
            {
                var c = CreateContainer();
                var foo = new Foo();

                c.RegisterInstance<IFoo>(foo, "test");

                Assert.True(((IContainerProvider)c).IsRegistered<IFoo>("test"));
                Assert.Same(foo, c.Resolve<IFoo>("test"));
            }
        }

        [Fact]
        public void RegisterSingletonNamedService()
        {
            lock (testLock)
            {
                var c = CreateContainer();
                c.RegisterSingleton<IFoo, Foo>("fooBar");
                IFoo foo = null;
                var ex = Record.Exception(() => foo = c.Resolve<IFoo>());

                Assert.Null(foo);

                ex = null;
                ex = Record.Exception(() => foo = c.Resolve<IFoo>("fooBar"));

                Assert.Null(ex);
                Assert.NotNull(foo);
                Assert.IsType<Foo>(foo);

                Assert.Same(foo, c.Resolve<IFoo>("fooBar"));
            }
        }

        [Fact]
        public void FactoryCreatesTransientTypeWithoutContainerProvider()
        {
            lock (testLock)
            {
                var c = CreateContainer();
                var message = "expected";
                c.Register<IFoo>(FooFactory);

                IFoo foo = null;
                var ex = Record.Exception(() => foo = c.Resolve<IFoo>());

                Assert.Null(ex);
                Assert.Equal(message, foo.Message);

                Assert.NotSame(foo, c.Resolve<IFoo>());
            }
        }

        [Fact]
        public void FactoryCreatesSingletonTypeWithoutContainerProvider()
        {
            lock (testLock)
            {
                var c = CreateContainer();
                var message = "expected";
                c.RegisterSingleton<IFoo>(FooFactory);

                IFoo foo = null;
                var ex = Record.Exception(() => foo = c.Resolve<IFoo>());

                Assert.Null(ex);
                Assert.Equal(message, foo.Message);

                Assert.Same(foo, c.Resolve<IFoo>());
            }
        }

        //[Fact]
        //public void FactoryCreatesTransientTypeWithServiceProvider()
        //{
        //    lock(testLock)
        //    {
        //        var c = CreateContainer();
        //        var expectedMessage = "constructed with IServiceProvider";
        //        c.Register(typeof(IBar), BarFactoryWithIServiceProvider);
        //        c.RegisterInstance<IFoo>(new Foo { Message = expectedMessage });

        //        IBar bar = null;
        //        var ex = Record.Exception(() => bar = c.Resolve<IBar>());

        //        Assert.Null(ex);
        //        Assert.False(string.IsNullOrWhiteSpace(bar.Foo.Message));
        //        Assert.Equal(expectedMessage, bar.Foo.Message);

        //        Assert.NotSame(bar, c.Resolve<IBar>());
        //    }
        //}

        //[Fact]
        //public void FactoryCreatesTransientTypeWithServiceProviderFromGeneric()
        //{
        //    lock(testLock)
        //    {
        //        var c = CreateContainer();
        //        var expectedMessage = "constructed with IServiceProvider";
        //        c.Register<IBar>(BarFactoryWithIServiceProvider);
        //        c.RegisterInstance<IFoo>(new Foo { Message = expectedMessage });

        //        IBar bar = null;
        //        var ex = Record.Exception(() => bar = c.Resolve<IBar>());

        //        Assert.Null(ex);
        //        Assert.False(string.IsNullOrWhiteSpace(bar.Foo.Message));
        //        Assert.Equal(expectedMessage, bar.Foo.Message);

        //        Assert.NotSame(bar, c.Resolve<IBar>());
        //    }
        //}

        //[Fact]
        //public void FactoryCreatesSingletonTypeWithServiceProvider()
        //{
        //    lock(testLock)
        //    {
        //        var c = CreateContainer();
        //        var expectedMessage = "constructed with IServiceProvider";
        //        c.RegisterSingleton(typeof(IBar), BarFactoryWithIServiceProvider);
        //        c.RegisterInstance<IFoo>(new Foo { Message = expectedMessage });

        //        IBar bar = null;
        //        var ex = Record.Exception(() => bar = c.Resolve<IBar>());

        //        Assert.Null(ex);
        //        Assert.False(string.IsNullOrWhiteSpace(bar.Foo.Message));
        //        Assert.Equal(expectedMessage, bar.Foo.Message);

        //        Assert.Same(bar, c.Resolve<IBar>());
        //    }
        //}

        //[Fact]
        //public void FactoryCreatesSingletonTypeWithServiceProviderFromGeneric()
        //{
        //    lock(testLock)
        //    {
        //        try
        //        {
        //            var c = CreateContainer();
        //            Assert.NotNull(c);
        //            var expectedMessage = "constructed with IServiceProvider";
        //            c.RegisterSingleton<IBar>(BarFactoryWithIServiceProvider);
        //            c.RegisterInstance<IFoo>(new Foo { Message = expectedMessage });

        //            IBar bar = null;
        //            var ex = Record.Exception(() => bar = c.Resolve<IBar>());

        //            Assert.Null(ex);
        //            Assert.False(string.IsNullOrWhiteSpace(bar.Foo.Message));
        //            Assert.Equal(expectedMessage, bar.Foo.Message);

        //            Assert.Same(bar, c.Resolve<IBar>());
        //        }
        //        catch (Exception ex)
        //        {
        //            _testOutputHelper.WriteLine(ex.ToString());
        //            throw;
        //        }
        //    }
        //}

        [Fact]
        public void FactoryCreatesTransientTypeWithContainerProvider()
        {
            lock (testLock)
            {
                var c = CreateContainer();

                var expectedMessage = "constructed with IContainerProvider";
                c.Register(typeof(IBar), BarFactoryWithIContainerProvider);
                c.RegisterSingleton<IFoo, Foo>();

                IBar bar = null;
                var ex = Record.Exception(() => bar = c.Resolve<IBar>());

                Assert.Null(ex);
                Assert.IsType<Bar>(bar);
                Assert.Equal(expectedMessage, ((Bar)bar).Message);

                Assert.Same(c.Resolve<IFoo>(), bar.Foo);
                Assert.NotSame(c.Resolve<IBar>(), bar);
            }
        }

        [Fact]
        public void FactoryCreatesTransientTypeWithContainerProviderWithGeneric()
        {
            lock (testLock)
            {
                var c = CreateContainer();

                var expectedMessage = "constructed with IContainerProvider";
                c.Register<IBar>(BarFactoryWithIContainerProvider);
                c.RegisterSingleton<IFoo, Foo>();

                IBar bar = null;
                var ex = Record.Exception(() => bar = c.Resolve<IBar>());

                Assert.Null(ex);
                Assert.IsType<Bar>(bar);
                Assert.Equal(expectedMessage, ((Bar)bar).Message);

                Assert.Same(c.Resolve<IFoo>(), bar.Foo);
                Assert.NotSame(c.Resolve<IBar>(), bar);
            }
        }

        [Fact]
        public void FactoryCreatesSingletonTypeWithContainerProvider()
        {
            lock (testLock)
            {
                var c = CreateContainer();

                var expectedMessage = "constructed with IContainerProvider";
                c.RegisterSingleton(typeof(IBar), BarFactoryWithIContainerProvider);
                c.RegisterSingleton<IFoo, Foo>();

                IBar bar = null;
                var ex = Record.Exception(() => bar = c.Resolve<IBar>());

                Assert.Null(ex);
                Assert.IsType<Bar>(bar);
                Assert.Equal(expectedMessage, ((Bar)bar).Message);

                Assert.Same(c.Resolve<IFoo>(), bar.Foo);
                Assert.Same(c.Resolve<IBar>(), bar);
            }
        }

        [Fact]
        public void FactoryCreatesSingletonTypeWithContainerProviderWithGeneric()
        {
            lock (testLock)
            {
                var c = CreateContainer();

                var expectedMessage = "constructed with IContainerProvider";
                c.RegisterSingleton<IBar>(BarFactoryWithIContainerProvider);
                c.RegisterSingleton<IFoo, Foo>();

                IBar bar = null;
                var ex = Record.Exception(() => bar = c.Resolve<IBar>());

                Assert.Null(ex);
                Assert.IsType<Bar>(bar);
                Assert.Equal(expectedMessage, ((Bar)bar).Message);

                Assert.Same(c.Resolve<IFoo>(), bar.Foo);
                Assert.Same(c.Resolve<IBar>(), bar);
            }
        }

        [Fact]
        public void ResolveWithSpecifiedTypeOverridesRegistration()
        {
            lock (testLock)
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
        }

        [Fact]
        public void ContainerIsMutable()
        {
            lock (testLock)
            {
                var c = CreateContainer();
                c.Register<IFoo, Foo>();

                var foo = c.Resolve<IFoo>();
                Assert.NotNull(foo);
                var ex = Record.Exception(() => c.Resolve<IBar>());
                Assert.NotNull(ex);
                c.Register<IBar, Bar>();
                var bar = c.Resolve<IBar>();
                Assert.NotNull(bar);
            }
        }

        [Fact]
        public void ResolveNamedInstance()
        {
            var genA = new GenericService { Name = "genA" };
            var genB = new GenericService { Name = "genB" };
            lock (testLock)
            {
                var c = CreateContainer();
                c.RegisterInstance<IGenericService>(genA, "genA");
                c.RegisterInstance<IGenericService>(genB, "genB");

                Assert.Same(genA, c.Resolve<IGenericService>("genA"));
                Assert.Same(genB, c.Resolve<IGenericService>("genB"));
            }
        }

        [Fact]
        public void ResolveTakesLastIn()
        {
            var c = CreateContainer();
            c.Register<IGenericService, GenericService>();
            c.Register<IGenericService, AltGenericService>();

            Assert.IsType<AltGenericService>(c.Resolve<IGenericService>());
        }

        [Fact]
        public void ResolveEnumerableResolvesAll()
        {
            var c = CreateContainer();
            c.Register<IGenericService, GenericService>();
            c.Register<IGenericService, AltGenericService>();

            IEnumerable<IGenericService> all = null;
            var ex = Record.Exception(() => all = c.Resolve<IEnumerable<IGenericService>>());

            Assert.Null(ex);
            Assert.NotNull(all);
            Assert.NotEmpty(all);
            Assert.Equal(2, all.Count());
            Assert.Contains(all, x => x is GenericService);
            Assert.Contains(all, x => x is AltGenericService);
        }

        public static IFoo FooFactory() => new Foo { Message = "expected" };

        public static IBar BarFactoryWithIContainerProvider(IContainerProvider containerProvider) =>
            new Bar(containerProvider.Resolve<IFoo>())
            {
                Message = "constructed with IContainerProvider"
            };

        //public static IBar BarFactoryWithIServiceProvider(IServiceProvider serviceProvider) =>
        //    new Bar((IFoo)serviceProvider.GetService(typeof(IFoo)));

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
