using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Prism.Container.Extensions.Shared.Mocks;
using Xunit;
using Prism.Ioc;
using System.Net.Http;
#if DRYIOC
using Prism.DryIoc;
#elif UNITY
using Prism.Unity;
#elif MICROSOFT_DI
using Prism.Microsoft.DependencyInjection;
#endif

namespace Prism.Container.Extensions.Shared.Tests
{
    [Collection(nameof(SharedTests))]
    public class CommonAspNetServiceTests
    {
        public CommonAspNetServiceTests()
        {
            PrismContainerExtension.Reset();
        }

        [Fact]
        public void CreatingServicesDoesNotThrowException()
        {
            var ex = Record.Exception(() => ConfigureServices());
            Assert.Null(ex);
        }

        [Theory]
        [InlineData(typeof(MockDbContext))]
        [InlineData(typeof(IHttpClientFactory))]
        public void ServiceIsRegistered(Type type)
        {
            ConfigureServices();
            Assert.True(PrismContainerExtension.Current.IsRegistered(type));
        }

        [Fact]
        public void IHttpClientFactoryIsResolvable()
        {
            ConfigureServices();
            IHttpClientFactory service = null;
            PrismContainerExtension.Current.CreateScope();
            var ex = Record.Exception(() => service = PrismContainerExtension.Current.Resolve<IHttpClientFactory>());

            Assert.Null(ex);
            Assert.NotNull(service);
            Assert.IsAssignableFrom<IHttpClientFactory>(service);
        }

        [Fact]
        public void DbContextResolveableWithoutScope()
        {
            ConfigureServices();
            MockDbContext context = null;
            var ex = Record.Exception(() => context = PrismContainerExtension.Current.Resolve<MockDbContext>());

            Assert.Null(ex);
            Assert.NotNull(context);
        }

        [Fact]
        public void DbContextResolveableWithScope()
        {
            ConfigureServices();
            MockDbContext context = null;
            PrismContainerExtension.Current.CreateScope();
            var ex = Record.Exception(() => context = PrismContainerExtension.Current.Resolve<MockDbContext>());

            Assert.Null(ex);
            Assert.NotNull(context);
        }

        [Fact]
        public void ScopingProvidesNewInstance()
        {
            ConfigureServices();
            var context = PrismContainerExtension.Current.Resolve<MockDbContext>();
            PrismContainerExtension.Current.CreateScope();
            var context1 = PrismContainerExtension.Current.Resolve<MockDbContext>();
            Assert.NotSame(context, context1);

            var context2 = PrismContainerExtension.Current.Resolve<MockDbContext>();
            Assert.Same(context1, context2);

            PrismContainerExtension.Current.CreateScope();
            var context3 = PrismContainerExtension.Current.Resolve<MockDbContext>();
            Assert.NotSame(context2, context3);
        }

        [Fact]
        public void RegisterServicesExtensionsAddsServicesToContainer()
        {
            PrismContainerExtension.Current.RegisterServices(s =>
            {
                s.AddHttpClient();
            });

            Assert.True(((IContainerProvider)PrismContainerExtension.Current).IsRegistered<IHttpClientFactory>());
        }

        [Fact]
        public void IServiceScopeFactoryIsRegistered()
        {
            PrismContainerExtension.Current.RegisterServices(s =>
            {
                s.AddHttpClient();
            });

            Assert.True(((IContainerProvider)PrismContainerExtension.Current).IsRegistered<IServiceScopeFactory>());
        }

        [Fact]
        public void ResolvesHttpClientWithHttpClientFactory()
        {
            PrismContainerExtension.Current.RegisterServices(s =>
            {
                s.AddHttpClient();
            });

            HttpClient client = null;
            var ex = Record.Exception(() => client = PrismContainerExtension.Current.Resolve<HttpClient>());

            Assert.Null(ex);
            Assert.NotNull(client);
        }

        private void ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddDbContext<MockDbContext>(o => o.UseInMemoryDatabase("test"));
            services.AddHttpClient();
            PrismContainerExtension.Current.CreateServiceProvider(services);
        }
    }
}
