using System;
using System.Reactive.Subjects;
using Microsoft.Extensions.DependencyInjection;
using Shiny.Infrastructure;
using Shiny.Jobs;
using Shiny.Net;
using Shiny.Power;
using Shiny.Settings;
using Shiny.Testing;
using Shiny.Testing.Jobs;
using Shiny.Testing.Net;
using Shiny.Testing.Power;
using Shiny.Testing.Settings;
using Xunit.Abstractions;

namespace Shiny.Prism.Mocks
{
    class ShinyPrismTestHost : TestPlatform
    {
        private readonly Action<IServiceCollection> _platformBuild;

        private ShinyPrismTestHost(Action<IServiceCollection> platformBuild)
        {
            _platformBuild = platformBuild;
        }

        public static void Init(ITestOutputHelper testOutputHelper) => Init(new MockStartup(testOutputHelper));

        public static void Init(IShinyStartup startup = null, Action<IServiceCollection> platformBuild = null)
        {
            ShinyHost.Init(new ShinyPrismTestHost(platformBuild), startup);
        }

        public override void Register(IServiceCollection services)
        {
            base.Register(services);
            services.AddSingleton<IJobManager, TestJobManager>();
            services.AddSingleton<IConnectivity, TestConnectivity>();
            services.AddSingleton<IPowerManager, TestPowerManager>();
            services.AddSingleton<ISettings, TestSettings>();
            services.AddSingleton<ISerializer, ShinySerializer>();
            _platformBuild?.Invoke(services);
        }
    }
}
