using Microsoft.Extensions.DependencyInjection;
using Prism.Container.Extensions.Tests.Mocks;
using Prism.Ioc;
using System;
using Xunit;

namespace Prism.Unity.Extensions.Tests
{
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
                var services = new ServiceCollection(); services.AddSingleton<IFoo>(foo);
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
