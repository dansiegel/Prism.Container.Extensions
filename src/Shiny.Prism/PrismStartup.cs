using Microsoft.Extensions.DependencyInjection;
using Prism.Ioc;
using Prism.Modularity;
using Shiny.Logging;
using Shiny.Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shiny.Prism
{
    public abstract class PrismStartup : IShinyStartup
    {
        private IContainerExtension _container;

        protected PrismStartup()
        {
        }

        protected PrismStartup(IContainerExtension container)
        {
            _container = container;
        }

        protected abstract void ConfigureServices(IServiceCollection services);

        void IShinyStartup.ConfigureServices(IServiceCollection services)
        {
            ConfigureServices(services);
            services.RegisterPrismCoreServices();
            services.Remove(services.First(x => x.ServiceType == typeof(IModuleInitializer)));
            services.AddSingleton<IModuleInitializer, ShinyPrismModuleInitializer>();
        }

        void IShinyStartup.ConfigureApp(IServiceProvider provider) { }

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

        protected virtual IContainerExtension CreateContainerExtension() =>
            _container ??
            throw new NotImplementedException("You must implement PrismStartup.CreateContainerExtension() or Supply the PrismStartup with an instance of IContainerExtension");

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
