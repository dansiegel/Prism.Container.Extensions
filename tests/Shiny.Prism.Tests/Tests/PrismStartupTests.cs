using System;
using System.Linq;
using Prism.DryIoc;
using Prism.Ioc;
using Shiny.Jobs;
using Shiny.Net;
using Shiny.Power;
using Shiny.Prism.Mocks;
using Shiny.Settings;
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
        [InlineData(typeof(IPlatform), typeof(ShinyPrismTestHost))]
        [InlineData(typeof(IJobManager), typeof(TestJobManager))]
        [InlineData(typeof(IConnectivity), typeof(TestConnectivity))]
        [InlineData(typeof(IPowerManager), typeof(TestPowerManager))]
        [InlineData(typeof(ISettings), typeof(TestSettings))]
        public void ExpectedTypesAreRegisteredAndResolve(Type serviceType, Type implementingType)
        {
            ShinyPrismTestHost.Init(_testOutputHelper);
            var types = ContainerLocator.Container
                .GetContainer()
                .GetServiceRegistrations()
                .Where(x => x.ServiceType.Assembly.GetName().Name == "Shiny.Core");
            foreach (var type in types)
                _testOutputHelper.WriteLine($"Found: {type.ServiceType.FullName} - {type.ImplementationType?.FullName}");

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
            Assert.NotNull(PrismContainerExtension.Current.Resolve<IJobManager>());
        }
    }
}
