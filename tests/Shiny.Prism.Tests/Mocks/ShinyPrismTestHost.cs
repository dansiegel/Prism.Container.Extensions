using System;
using Microsoft.Extensions.DependencyInjection;
using Shiny.Testing;
using Xunit.Abstractions;

namespace Shiny.Prism.Mocks
{
    static class ShinyPrismTestHost
    {
        public static void Init(ITestOutputHelper testOutputHelper) => ShinyHost.Init(new TestPlatform(), new MockStartup(testOutputHelper));

        public static void Init(IShinyStartup startup = null, Action<IServiceCollection> platformBuild = null)
        {
            ShinyHost.Init(new TestPlatform(), startup);
            //InitPlatform(startup, services =>
            //{
            //    services.AddSingleton<IJobManager, TestJobManager>();
            //    services.AddSingleton<IConnectivity, TestConnectivity>();
            //    services.AddSingleton<IPowerManager, TestPowerManager>();
            //    platformBuild?.Invoke(services);
            //});
        }
    }
}
