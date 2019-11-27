using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Prism.Modularity;

namespace Shiny.Prism.Modularity
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class IModuleCatalogExtensions
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static bool HasStartupModules(this IModuleCatalog moduleCatalog, out IEnumerable<IModuleInfo> startupModules)
        {
            startupModules = FilterForStartup(moduleCatalog.Modules);

            if(startupModules.Any())
            {
                startupModules = moduleCatalog.CompleteListWithDependencies(startupModules);
            }

            return startupModules.Any();
        }

        private  static IEnumerable<IModuleInfo> FilterForStartup(IEnumerable<IModuleInfo> modules) =>
            modules.Where(m => Type.GetType(m.ModuleType)
                                   .GetInterfaces()
                                   .Any(x => x == typeof(IStartupModule))
                               && m.InitializationMode == InitializationMode.WhenAvailable);
    }
}
