using System;
using Microsoft.Extensions.DependencyInjection;
using Prism.DryIoc;
using Prism.Ioc;
using Shiny.Prism.Mocks.Delegates;
using Xunit.Abstractions;

namespace Shiny.Prism.Mocks
{
    public class MockStartup : PrismStartup
    {
        private ITestOutputHelper _testOutputHelper { get; }

        public MockStartup(ITestOutputHelper testOutputHelper, bool setContainer = true)
            : base(setContainer ? RegenerateContainer() : null)
        {
            _testOutputHelper = testOutputHelper;

            if(!setContainer)
            {
                RegenerateContainer();
            }
        }

        protected override void ConfigureServices(IServiceCollection services)
        {

            //services.UseBeacons<MockBeaconDelegate>();
            services.UseGps<MockGpsDelegate>();
            services.UseGeofencing<MockGeofenceDelegate>();
            //services.RegisterBleAdapterState<MockBleAdapterDelegate>();
        }

        // For Testing Only...
        static IContainerExtension RegenerateContainer()
        {
            PrismContainerExtension.Reset();
            return PrismContainerExtension.Create();
        }
    }
}
