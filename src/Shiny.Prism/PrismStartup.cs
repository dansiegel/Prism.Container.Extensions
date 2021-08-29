using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Prism.Container.Extensions;
using Prism.Ioc;
using Prism.Modularity;
using Shiny.Prism.Modularity;

namespace Shiny.Prism
{
    public abstract class PrismStartup : IShinyStartup
    {
        private IContainerExtension _container;

        public Action<IServiceCollection> RegisterPlatformServices { get; set; }

        protected PrismStartup()
        {
        }

        protected PrismStartup(IContainerExtension container)
        {
            _container = container;
        }

        public virtual void ConfigureLogging(ILoggingBuilder builder, IPlatform platform) { }
        protected virtual void ConfigureApp(IServiceProvider provider) { }

        protected abstract void ConfigureServices(IServiceCollection services);

        void IShinyStartup.ConfigureServices(IServiceCollection services, IPlatform platform)
        {
            ConfigureServices(services);
            services.RegisterPrismCoreServices();
            services.Remove(services.First(x => x.ServiceType == typeof(IModuleInitializer)));
            services.AddSingleton<IModuleInitializer, ShinyPrismModuleInitializer>();
        }

        IServiceProvider IShinyStartup.CreateServiceProvider(IServiceCollection services)
        {
            if (_container is null)
            {
                _container = CreateContainerExtension();
            }

            var sp = _container.CreateServiceProvider(services);

            var moduleCatalog = _container.Resolve<IModuleCatalog>();
            ConfigureModuleCatalog(moduleCatalog);

            if(moduleCatalog.Modules.Any() && moduleCatalog.HasStartupModules(out var startupModules))
            {
                var moduleInitializer = _container.Resolve<IModuleInitializer>() as IShinyPrismModuleInitializer;
                moduleInitializer.LoadShinyModules(startupModules);
            }

            return sp;
        }

        protected virtual IContainerExtension CreateContainerExtension()
        {
            return _container ?? ContainerLocator.Locate();
        }

        public IShinyStartup WithContainer(IContainerExtension container)
        {
            _container = container;
            return this;
        }

        protected virtual void ConfigureModuleCatalog(IModuleCatalog moduleCatalog) { }
    }

    //public abstract class PrismStartupTask : IShinyStartupTask
    //{
    //    async void IShinyStartupTask.Start()
    //    {
    //        try
    //        {
    //            Start();
    //            await StartAsync();
    //        }
    //        catch (Exception ex)
    //        {
    //            Log.Write(ex);
    //        }
    //    }

    //    protected virtual void Start() { }

    //    protected virtual Task StartAsync() => Task.CompletedTask;
    //}
}
