using System;
using System.Collections.Generic;
using System.Text;
using Prism.Ioc;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Prism.DryIoc.Extensions.Tests
{
    public class MicrosoftExtensionsTests
    {
        [Fact]
        public void ServiceCollectionTransientServiceIsRegistered()
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
    }
}
