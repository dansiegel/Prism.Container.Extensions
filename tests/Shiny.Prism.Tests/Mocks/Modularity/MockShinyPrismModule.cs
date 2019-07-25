using Microsoft.Extensions.DependencyInjection;
using Prism.Ioc;
using Shiny.Prism.Mocks.Modularity.Services;
using Shiny.Prism.Modularity;

namespace Shiny.Prism.Mocks.Modularity
{
    public class MockShinyPrismModule : StartupModule
    {
        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IMockModuleServiceA, MockModuleServiceA>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IMockModuleServiceB, MockModuleServiceB>();
        }
    }
}
