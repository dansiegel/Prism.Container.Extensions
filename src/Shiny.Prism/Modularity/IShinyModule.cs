using System;
using Microsoft.Extensions.DependencyInjection;
using Prism.Modularity;

namespace Shiny.Prism.Modularity
{
    public interface IShinyModule : IModule
    {
        void ConfigureApp(IServiceProvider provider);
        void ConfigureServices(IServiceCollection services);
    }
}
