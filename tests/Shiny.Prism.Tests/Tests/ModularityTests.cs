using System;
using System.Collections.Generic;
using System.Text;
using Prism.DryIoc;
using Prism.Ioc;
using Shiny.Prism.Mocks;
using Shiny.Prism.Mocks.Modularity;
using Shiny.Prism.Mocks.Modularity.Services;
using Shiny.Prism.Modularity;
using Xunit;
using Xunit.Abstractions;

namespace Shiny.Prism.Tests
{
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

            Assert.True(PrismContainerExtension.Current.IsRegistered<IMockModuleServiceA>());
        }

        [Fact]
        public void StartupModuleServicesAreNotRegisteredFromContainerRegistry()
        {
            ShinyPrismTestHost.Init(new MockModuleStartup(_testOutputHelper));

            Assert.False(PrismContainerExtension.Current.IsRegistered<IMockModuleServiceB>());
        }

        [Fact]
        public void GetNavigationSegmentNameInvokesInternalMethod()
        {
            var currentType = GetType();
            var segmentName = IContainerRegistryAutoLoadExtensions.GetNavigationSegmentName(currentType);

            Assert.Equal(currentType.Name, segmentName);
        }
    }
}
