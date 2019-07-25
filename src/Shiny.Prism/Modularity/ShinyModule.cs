using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Prism.Ioc;
using Prism.Modularity;

namespace Shiny.Prism.Modularity
{
    public abstract class ShinyModule : IShinyModule
    {
        protected virtual void ConfigureApp(IServiceProvider provider) { }

        void IShinyModule.ConfigureApp(IServiceProvider provider) => ConfigureApp(provider);

        protected abstract void ConfigureServices(IServiceCollection services);

        void IShinyModule.ConfigureServices(IServiceCollection services) => ConfigureServices(services);

        protected virtual void OnInitialized(IContainerProvider containerProvider) { }

        void IModule.OnInitialized(IContainerProvider containerProvider) => OnInitialized(containerProvider);

        protected abstract void RegisterTypes(IContainerRegistry containerRegistry);

        void IModule.RegisterTypes(IContainerRegistry containerRegistry) => RegisterTypes(containerRegistry);
    }
}
