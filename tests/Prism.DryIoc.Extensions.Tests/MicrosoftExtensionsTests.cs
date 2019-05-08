using Microsoft.Extensions.DependencyInjection;
using Prism.Ioc;
using System;
using Xunit;

namespace Prism.DryIoc.Extensions.Tests
{
    public class MicrosoftExtensionsTests
    {
        [Fact]
        public void TransientServiceIsRegistered()
        {
            var services = new ServiceCollection();
            services.AddTransient<IFoo, Foo>();
            var container = new PrismContainerExtension();
            var serviceProvider = container.CreateServiceProvider(services);

            object service = null;
            var ex = Record.Exception(() => service = serviceProvider.GetService(typeof(IFoo)));

            Assert.Null(ex);
            Assert.NotNull(service);
            Assert.IsAssignableFrom<IFoo>(service);
            Assert.IsType<Foo>(service);

            Assert.NotSame(service, serviceProvider.GetService(typeof(IFoo)));
        }

        [Fact]
        public void SingletonServiceIsRegistered()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IFoo, Foo>();
            var container = new PrismContainerExtension();
            var serviceProvider = container.CreateServiceProvider(services);

            object service = null;
            var ex = Record.Exception(() => service = serviceProvider.GetService(typeof(IFoo)));

            Assert.Null(ex);
            Assert.NotNull(service);
            Assert.IsAssignableFrom<IFoo>(service);
            Assert.IsType<Foo>(service);

            Assert.Same(service, serviceProvider.GetService(typeof(IFoo)));
        }

        [Fact]
        public void SingletonInstanceIsResolved()
        {
            var foo = new Foo();
            var services = new ServiceCollection();services.AddSingleton<IFoo>(foo); var container = new PrismContainerExtension();
            var serviceProvider = container.CreateServiceProvider(services);

            object service = null;
            var ex = Record.Exception(() => service = serviceProvider.GetService(typeof(IFoo)));

            Assert.Null(ex);
            Assert.NotNull(service);
            Assert.IsAssignableFrom<IFoo>(service);
            Assert.IsType<Foo>(service);

            Assert.Same(foo, service);
        }

        [Fact]
        public void ScopedServiceNotSupported()
        {
            var services = new ServiceCollection();
            services.AddScoped<IFoo, Foo>();
            var container = new PrismContainerExtension();
            IServiceProvider serviceProvider = null;

            var ex = Record.Exception(() => serviceProvider = container.CreateServiceProvider(services));

            Assert.NotNull(ex);
            Assert.IsType<NotSupportedException>(ex);
        }
    }
}
