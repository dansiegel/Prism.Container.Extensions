using System;
using Prism.DryIoc;
using Prism.Ioc;
using Shiny.Jobs;
using Shiny.Net;
using Shiny.Power;
using Shiny.Prism.Mocks;
using Shiny.Testing.Jobs;
using Shiny.Testing.Net;
using Shiny.Testing.Power;
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
        //[InlineData(typeof(IBleAdapterDelegate), typeof(MockBleAdapterDelegate))]
        public void ExpectedTypesAreRegisteredAndResolve(Type serviceType, Type implementingType)
        {
            ShinyPrismTestHost.Init(_testOutputHelper);
            Assert.True(PrismContainerExtension.Current.IsRegistered(serviceType));

            var fromShiny = ShinyHost.ServiceProvider.GetService(serviceType);
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
