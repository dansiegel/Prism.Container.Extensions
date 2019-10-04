using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Prism.DryIoc;
using Prism.Ioc;
using Shiny.Beacons;
using Shiny.BluetoothLE;
using Shiny.Locations;
using Shiny.Prism.Mocks.Delegates;
using Shiny.Logging;
using Xunit.Abstractions;

namespace Shiny.Prism.Mocks
{
    public class MockStartup : PrismStartup, ILogger
    {
        private ITestOutputHelper _testOutputHelper { get; }

        public MockStartup(ITestOutputHelper testOutputHelper)
            : base(RegenerateContainer())
        {
            _testOutputHelper = testOutputHelper;
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            Log.Loggers.Clear();
            Log.AddLogger(this);

            services.UseBeacons<MockBeaconDelegate>();
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

        public void Write(Exception exception, params (string Key, string Value)[] parameters)
        {
            _testOutputHelper.WriteLine(exception.ToString());
            WriteParams(parameters);
        }

        public void Write(string eventName, string description, params (string Key, string Value)[] parameters)
        {
            _testOutputHelper.WriteLine($"Event: {eventName}");
            _testOutputHelper.WriteLine(description);
            WriteParams(parameters);
        }

        private void WriteParams((string Key, string Value)[] parameters)
        {
            foreach((string Key, string Value) in parameters)
            {
                _testOutputHelper.WriteLine($"{Key}: {Value}");
            }
        }
    }
}
