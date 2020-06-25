using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Prism.DryIoc;
using Prism.Ioc;
using Shiny;
using Shiny.IO;
using Shiny.Jobs;
using Shiny.Net;
using Shiny.Power;
using Shiny.Prism.Mocks;
using Shiny.Prism.Mocks.Delegates;
using Shiny.Push;
using Shiny.Settings;
using Shiny.Testing;
using Shiny.Testing.Jobs;
using Shiny.Testing.Net;
using Shiny.Testing.Power;
using Shiny.Testing.Settings;
using Xunit;
using Xunit.Abstractions;

namespace Shiny.Prism.Tests
{
    [Collection(nameof(ShinyTests))]
    public class PrismStartupTests
    {
        private ITestOutputHelper _testOutputHelper { get; }

        public PrismStartupTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void DoesNotThrowExceptionOnStartup()
        {
            var ex = Record.Exception(() => ShinyPrismTestHost.Init(_testOutputHelper));

            Assert.Null(ex);
        }

        [Theory]
        [InlineData(typeof(IJobManager), typeof(TestJobManager))]
        [InlineData(typeof(IConnectivity), typeof(TestConnectivity))]
        [InlineData(typeof(IPowerManager), typeof(TestPowerManager))]
        [InlineData(typeof(IFileSystem), typeof(FileSystemImpl))]
        [InlineData(typeof(ISettings), typeof(TestSettings))]
        [InlineData(typeof(IEnvironment), typeof(TestEnvironment))]
        //[InlineData(typeof(IBleAdapterDelegate), typeof(MockBleAdapterDelegate))]
        public void ExpectedTypesAreRegisteredAndResolve(Type serviceType, Type implementingType)
        {
            ShinyPrismTestHost.Init(_testOutputHelper);
            Assert.True(PrismContainerExtension.Current.IsRegistered(serviceType));

            var fromShiny = ShinyHost.Container.GetService(serviceType);
            Assert.IsType(implementingType, fromShiny);
            Assert.Same(fromShiny, PrismContainerExtension.Current.Resolve(serviceType));
        }

        [Fact]
        public void CanResolveAll()
        {
            ShinyPrismTestHost.Init(_testOutputHelper, services =>
            {
                services.AddSingleton<IPushDelegate, MockPushDelegate>();
            });

            Assert.True(PrismContainerExtension.Current.IsRegistered(typeof(IPushDelegate)));

            var fromShiny = ShinyHost.Resolve<IPushDelegate>();
            Assert.IsType<MockPushDelegate>(fromShiny);
            Assert.Same(fromShiny, PrismContainerExtension.Current.Resolve(typeof(IPushDelegate)));
            Assert.Same(fromShiny, ShinyHost.Container.Resolve<IPushDelegate>());

            Assert.Equal(new[] { fromShiny }, PrismContainerExtension.Current.Resolve<IEnumerable<IPushDelegate>>());
            Assert.Equal(new[] { fromShiny }, ShinyHost.Container.GetServices<IPushDelegate>());
            Assert.Equal(new[] { fromShiny }, ShinyHost.Container.Resolve<IEnumerable<IPushDelegate>>());
            Assert.Equal(new[] { fromShiny }, ShinyHost.ResolveAll<IPushDelegate>());
        }

        [Fact]
        public void CanResolveAllWithoutRegistration()
        {
            ShinyPrismTestHost.Init(_testOutputHelper);

            Assert.False(PrismContainerExtension.Current.IsRegistered(typeof(IPushDelegate)));

            Assert.Null(ShinyHost.Resolve<IPushDelegate>());

            Assert.Empty(PrismContainerExtension.Current.Resolve<IEnumerable<IPushDelegate>>());
            Assert.Empty(ShinyHost.Container.GetServices<IPushDelegate>());
            Assert.Empty(ShinyHost.Container.Resolve<IEnumerable<IPushDelegate>>());
            Assert.Empty(ShinyHost.ResolveAll<IPushDelegate>());
        }

        [Fact]
        public void PrismStartupLocatesContainerExtension()
        {
            var ex = Record.Exception(() => ShinyPrismTestHost.Init(new MockStartup(_testOutputHelper, false)));

            Assert.Null(ex);
            Assert.NotNull(PrismContainerExtension.Current.Resolve<IConnectivity>());
        }
    }
}
