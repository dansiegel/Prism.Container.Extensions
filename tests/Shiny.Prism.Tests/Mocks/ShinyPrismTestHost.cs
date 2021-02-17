using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Subjects;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
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
    class ShinyPrismTestHost : IPlatform
    {
        private readonly Subject<PlatformState> _stateChanged = new Subject<PlatformState>();
        private readonly Action<IServiceCollection> _platformBuild;

        private ShinyPrismTestHost(Action<IServiceCollection> platformBuild)
        {
            _platformBuild = platformBuild;
        }

        public DirectoryInfo AppData { get; }
        public DirectoryInfo Cache { get; }
        public DirectoryInfo Public { get; }
        public string AppIdentifier { get; }
        public string AppVersion { get; }
        public string AppBuild { get; }
        public string MachineName { get; }
        public string OperatingSystem { get; }
        public string OperatingSystemVersion { get; }
        public string Manufacturer { get; }
        public string Model { get; }

        public static void Init(ITestOutputHelper testOutputHelper) => Init(new MockStartup(testOutputHelper));

        public static void Init(IShinyStartup startup = null, Action<IServiceCollection> platformBuild = null)
        {
            ShinyHost.Init(new ShinyPrismTestHost(platformBuild), startup);
        }

        public void Register(IServiceCollection services)
        {
            services.AddSingleton<IJobManager, TestJobManager>();
            services.AddSingleton<IConnectivity, TestConnectivity>();
            services.AddSingleton<IPowerManager, TestPowerManager>();
            //services.AddSingleton<IFileSystem, FileSystemImpl>();
            services.AddSingleton<ISettings, TestSettings>();
            //services.AddSingleton<IEnvironment, TestEnvironment>();
            _platformBuild?.Invoke(services);
        }

        public IObservable<PlatformState> WhenStateChanged()
        {
            return _stateChanged;
        }
    }
}
