using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Prism.Container.Extensions;
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

        protected virtual void ConfigureApp(IServiceProvider provider) { }

        protected abstract void ConfigureServices(IServiceCollection services);

        void IShinyStartup.ConfigureServices(IServiceCollection services)
        {
            ConfigureServices(services);
            services.RegisterPrismCoreServices();
            services.Remove(services.First(x => x.ServiceType == typeof(IModuleInitializer)));
            services.AddSingleton<IModuleInitializer, ShinyPrismModuleInitializer>();
        }

        void IShinyStartup.ConfigureApp(IServiceProvider provider) => ConfigureApp(provider);

        IServiceProvider IShinyStartup.CreateServiceProvider(IServiceCollection services)
        {
            var container = ContainerLocator.Current;
            if(container is null && (container = CreateContainerExtension()) is null)
            {
                throw new NullReferenceException("Call PrismContainerExtension.Init() prior to initializing PrismApplication");
            }

            var sp = container.CreateServiceProvider(services);

            var moduleCatalog = container.Resolve<IModuleCatalog>();
            ConfigureModuleCatalog(moduleCatalog);

            if(moduleCatalog.Modules.Any() && moduleCatalog.HasStartupModules(out var startupModules))
            {
                var moduleInitializer = container.Resolve<IModuleInitializer>() as IShinyPrismModuleInitializer;
                moduleInitializer.LoadShinyModules(startupModules);
            }

            return sp;
        }

        protected virtual IContainerExtension CreateContainerExtension()
        {
            return null;
            //return _container ?? ContainerLocator.Locate();
        }

        public IShinyStartup WithContainer(IContainerExtension container)
        {
            ContainerLocator.SetContainerExtension(() => container);
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
