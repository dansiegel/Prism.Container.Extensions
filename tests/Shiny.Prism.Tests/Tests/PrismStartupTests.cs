using System;
using Prism.DryIoc;
using Prism.Ioc;
using Shiny.Beacons;
using Shiny.BluetoothLE;
using Shiny.Locations;
using Shiny.Prism.Mocks;
using Shiny.Prism.Mocks.Delegates;
using Shiny.Prism.Mocks.Modularity;
using Shiny.Testing;
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
        //[InlineData(typeof(IGpsDelegate), typeof(MockGpsDelegate))]
        [InlineData(typeof(IGeofenceDelegate), typeof(MockGeofenceDelegate))]
        //[InlineData(typeof(IBeaconDelegate), typeof(MockBeaconDelegate))]
        [InlineData(typeof(IBleAdapterDelegate), typeof(MockBleAdapterDelegate))]
        public void ExpectedTypesAreRegisteredAndResolve(Type serviceType, Type implementingType)
        {
            ShinyPrismTestHost.Init(_testOutputHelper);
            Assert.True(PrismContainerExtension.Current.IsRegistered(serviceType));

            var fromShiny = ShinyHost.Container.GetService(serviceType);
            Assert.IsType(implementingType, fromShiny);
            Assert.Same(fromShiny, PrismContainerExtension.Current.Resolve(serviceType));
        }

        //[Fact]
        //public void GpsManagerIsRegistered()
        //{
        //    ShinyPrismTestHost.Init(_testOutputHelper);

        //    Assert.True(PrismContainerExtension.Current.IsRegistered<IGpsManager>());

        //    IGpsManager gpsManager = null;
        //    var ex = Record.Exception(() => gpsManager = PrismContainerExtension.Current.Resolve<IGpsManager>());

        //    Assert.Null(ex);
        //    Assert.NotNull(gpsManager);
        //}

        
    }
}
