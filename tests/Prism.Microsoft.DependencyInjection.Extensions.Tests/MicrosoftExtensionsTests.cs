using System;
using Microsoft.Extensions.DependencyInjection;
using Prism.Container.Extensions.Shared.Tests;
using Prism.Container.Extensions.Tests.Mocks;
using Prism.Ioc;
using Xunit;

namespace Prism.Microsoft.DependencyInjection.Extensions.Tests
{
    [Collection(nameof(SharedTests))]
    public class MicrosoftExtensionsTests
    {
        private readonly object testLock = new object();

        [Fact]
        public void TransientServiceIsRegistered()
        {
            lock (testLock)
            {
                PrismContainerExtension.Reset();
                GC.Collect();

                var services = new ServiceCollection();
                services.AddTransient<IFoo, Foo>();
                var container = PrismContainerExtension.Create();
                var serviceProvider = container.CreateServiceProvider(services);

                object service = null;
                var ex = Record.Exception(() => service = serviceProvider.GetService(typeof(IFoo)));

                Assert.Null(ex);
                Assert.NotNull(service);
                Assert.IsAssignableFrom<IFoo>(service);
                Assert.IsType<Foo>(service);

                Assert.NotSame(service, serviceProvider.GetService(typeof(IFoo)));
            }
        }

        [Fact]
        public void SingletonServiceIsRegistered()
        {
            lock(testLock)
            {
                PrismContainerExtension.Reset();
                GC.Collect();

                var services = new ServiceCollection();
                services.AddSingleton<IFoo, Foo>();
                var container = PrismContainerExtension.Create();
                var serviceProvider = container.CreateServiceProvider(services);

                object service = null;
                var ex = Record.Exception(() => service = serviceProvider.GetService(typeof(IFoo)));

                Assert.Null(ex);
                Assert.NotNull(service);
                Assert.IsAssignableFrom<IFoo>(service);
                Assert.IsType<Foo>(service);

                Assert.Same(service, serviceProvider.GetService(typeof(IFoo)));
            }
        }

        [Fact]
        public void SingletonInstanceIsResolved()
        {
            lock(testLock)
            {
                PrismContainerExtension.Reset();
                GC.Collect();

                var foo = new Foo();
                var services = new ServiceCollection();
                services.AddSingleton<IFoo>(foo);
                var container = PrismContainerExtension.Create();
                var serviceProvider = container.CreateServiceProvider(services);

                object service = null;
                var ex = Record.Exception(() => service = serviceProvider.GetService(typeof(IFoo)));

                Assert.Null(ex);
                Assert.NotNull(service);
                Assert.IsAssignableFrom<IFoo>(service);
                Assert.IsType<Foo>(service);

                Assert.Same(foo, service);
            }
        }

        [Fact]
        public void SingletonFactoryIsResolved()
        {
            lock(testLock)
            {
                PrismContainerExtension.Reset();
                GC.Collect();

                int counter = 0;
                var services = new ServiceCollection();
                services.AddSingleton<IFoo>(sp => new Foo { Message = $"Foo has been resolved {++counter} time(s)." });
                PrismContainerExtension.Current.CreateServiceProvider(services);

                IFoo foo = null;
                var ex = Record.Exception(() => foo = PrismContainerExtension.Current.Resolve<IFoo>());

                Assert.Null(ex);
                Assert.NotNull(foo);
                Assert.Equal(1, counter);
                Assert.Equal("Foo has been resolved 1 time(s).", foo.Message);

                var foo2 = PrismContainerExtension.Current.Resolve<IFoo>();

                Assert.Equal(1, counter);
                Assert.Same(foo, foo2);
                Assert.Equal("Foo has been resolved 1 time(s).", foo2.Message);
            }
        }

        [Fact]
        public void TransientFactoryIsResolved()
        {
            lock (testLock)
            {
                PrismContainerExtension.Reset();
                GC.Collect();

                int counter = 0;
                var services = new ServiceCollection();
                services.AddTransient<IFoo>(sp => new Foo { Message = $"Foo has been resolved {++counter} time(s)." });
                PrismContainerExtension.Current.CreateServiceProvider(services);

                IFoo foo = null;
                var ex = Record.Exception(() => foo = PrismContainerExtension.Current.Resolve<IFoo>());

                Assert.Null(ex);
                Assert.NotNull(foo);
                Assert.Equal(1, counter);
                Assert.Equal("Foo has been resolved 1 time(s).", foo.Message);

                var foo2 = PrismContainerExtension.Current.Resolve<IFoo>();

                Assert.Equal(2, counter);
                Assert.NotSame(foo, foo2);
                Assert.Equal("Foo has been resolved 2 time(s).", foo2.Message);
            }
        }

        [Fact]
        public void ScopedServiceIsSupported()
        {
            lock(testLock)
            {
                PrismContainerExtension.Reset();
                GC.Collect();

                var services = new ServiceCollection();
                services.AddScoped<IFoo, Foo>();

                var container = PrismContainerExtension.Current;
                IServiceProvider serviceProvider = null;

                var ex = Record.Exception(() => serviceProvider = container.CreateServiceProvider(services));

                Assert.Null(ex);

                Assert.True(container.IsRegistered<IFoo>());
            }
        }
    }
}
