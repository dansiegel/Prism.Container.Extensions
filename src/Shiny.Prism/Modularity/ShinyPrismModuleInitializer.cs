using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Prism.Ioc;
using Prism.Modularity;

namespace Shiny.Prism.Modularity
{
    internal class ShinyPrismModuleInitializer : IModuleInitializer, IShinyPrismModuleInitializer
    {
        private IContainerExtension _container { get; }

        private List<string> _loadedShinyModules { get; }

        public ShinyPrismModuleInitializer(IContainerExtension container)
        {
            _container = container;
            _loadedShinyModules = new List<string>();
        }

        public void Initialize(IModuleInfo moduleInfo)
        {
            var module = LoadShinyModule(moduleInfo);

            if (module != null)
            {
                module.RegisterTypes(_container);
                module.OnInitialized(_container);
            }
        }

        public IModule LoadShinyModule(IModuleInfo moduleInfo)
        {
            var module = CreateModule(Type.GetType(moduleInfo.ModuleType, true));

            if (_loadedShinyModules.Contains(moduleInfo.ModuleName)) return module;

            if (module is IShinyModule shinyModule)
            {
                var services = new ServiceCollection();
                shinyModule.ConfigureServices(services);
                var sp = _container.CreateServiceProvider(services);
                shinyModule.ConfigureApp(sp);

                _loadedShinyModules.Add(moduleInfo.ModuleName);
            }

            return module;
        }

        private IModule CreateModule(Type moduleType) =>
            (IModule)_container.Resolve(moduleType);
    }

    internal static class IShinyPrismModuleInitailzierExtensions
    {
        public static void LoadShinyModules(this IShinyPrismModuleInitializer initializer, IEnumerable<IModuleInfo> modules)
        {
            foreach (var module in modules)
            {
                initializer.LoadShinyModule(module);
            }
        }
    }
}
