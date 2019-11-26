using System;
using Prism.DryIoc;
using Prism.Ioc;
using Shiny.Beacons;
using Shiny.BluetoothLE;
using Shiny.IO;
using Shiny.Jobs;
using Shiny.Locations;
using Shiny.Net;
using Shiny.Power;
using Shiny.Prism.Mocks;
using Shiny.Prism.Mocks.Delegates;
using Shiny.Prism.Mocks.Modularity;
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
        public void PrismStartupLocatesContainerExtension()
        {
            var ex = Record.Exception(() => ShinyPrismTestHost.Init(new MockStartup(_testOutputHelper, false)));

            Assert.Null(ex);
            Assert.NotNull(PrismContainerExtension.Current.Resolve<IConnectivity>());
        }
    }
}
