using Prism.DryIoc;
using Prism.Ioc;
using Shiny.Prism.Mocks;
using Shiny.Prism.Mocks.Modularity;
using Shiny.Prism.Mocks.Modularity.Services;
using Xunit;
using Xunit.Abstractions;

namespace Shiny.Prism.Tests
{
    [Collection(nameof(ShinyTests))]
    public class ModularityTests
    {
        private ITestOutputHelper _testOutputHelper { get; }

        public ModularityTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void StartupModuleServicesAreRegisteredFromServiceCollection()
        {
            ShinyPrismTestHost.Init(new MockModuleStartup(_testOutputHelper));

            Assert.True(((IContainerProvider)PrismContainerExtension.Current).IsRegistered<IMockModuleServiceA>());
        }

        [Fact]
        public void StartupModuleServicesAreNotRegisteredFromContainerRegistry()
        {
            ShinyPrismTestHost.Init(new MockModuleStartup(_testOutputHelper));

            Assert.False(((IContainerProvider)PrismContainerExtension.Current).IsRegistered<IMockModuleServiceB>());
        }
    }
}
