using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Prism.Container.Extensions.Internals;
using Prism.Ioc;
using Prism.Modularity;
using Shiny.Prism.Modularity;

namespace Shiny.Prism
{
    public abstract class PrismStartup : IShinyStartup
    {
        protected PrismStartup()
        {
        }

        protected PrismStartup(IContainerExtension container)
        {
            WithContainer(container);
        }

        protected virtual void ConfigureLogging(ILoggingBuilder builder, IPlatform platform) { }

        protected abstract void ConfigureServices(IServiceCollection services, IPlatform platform);

        protected virtual void RegisterServices(IContainerRegistry containerRegistry) { }

        void IShinyStartup.ConfigureLogging(ILoggingBuilder builder, IPlatform platform) =>
            ConfigureLogging(builder, platform);

        void IShinyStartup.ConfigureServices(IServiceCollection services, IPlatform platform)
        {
            ConfigureServices(services, platform);
            services.RegisterPrismCoreServices();
            services.Remove(services.First(x => x.ServiceType == typeof(IModuleInitializer)));
            services.AddSingleton<IModuleInitializer, ShinyPrismModuleInitializer>();
        }

        IServiceProvider IShinyStartup.CreateServiceProvider(IServiceCollection services)
        {
            var container = ContainerLocationHelper.LocateContainer(CreateContainerExtension()) ??
                throw new NullReferenceException("Call PrismContainerExtension.Init() prior to initializing PrismApplication");

            var sp = container.CreateServiceProvider(services);
            RegisterServices(container);

            var moduleCatalog = container.Resolve<IModuleCatalog>();
            ConfigureModuleCatalog(moduleCatalog);

            if (moduleCatalog.Modules.Any() && moduleCatalog.HasStartupModules(out var startupModules))
            {
                var moduleInitializer = container.Resolve<IModuleInitializer>() as IShinyPrismModuleInitializer;
                moduleInitializer.LoadShinyModules(startupModules);
            }

            return sp;
        }

        protected virtual IContainerExtension CreateContainerExtension()
        {
            return null;
        }

        public IShinyStartup WithContainer(IContainerExtension container)
        {
            ContainerLocator.SetContainerExtension(() => container);
            var _ = ContainerLocator.Container;
            return this;
        }

        protected virtual void ConfigureModuleCatalog(IModuleCatalog moduleCatalog) { }
    }
}
