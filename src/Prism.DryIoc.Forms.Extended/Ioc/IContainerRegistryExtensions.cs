using Prism.AppModel;
using Prism.Behaviors;
using Prism.Common;
using Prism.DryIoc.Navigation;
using Prism.Events;
using Prism.Ioc;
using Prism.Logging;
using Prism.Modularity;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;

namespace Prism.DryIoc.Ioc
{
    public static class IContainerRegistryExtensions
    {
        public static void RegisterRequiredTypes(this IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<IContainerExtension>(containerRegistry as IContainerExtension);
            containerRegistry.RegisterManySingleton<ConsoleLoggingService>();
            containerRegistry.RegisterSingleton<IApplicationProvider, ApplicationProvider>();
            containerRegistry.RegisterSingleton<IApplicationStore, ApplicationStore>();
            containerRegistry.RegisterSingleton<IEventAggregator, EventAggregator>();
            containerRegistry.RegisterSingleton<IPageDialogService, PageDialogService>();
            containerRegistry.RegisterSingleton<IDeviceService, DeviceService>();
            containerRegistry.RegisterSingleton<IPageBehaviorFactory, ExtendedPageBehaviorFactory>();
            containerRegistry.RegisterSingleton<IPageBehaviorFactoryOptions, DefaultPageBehaviorFactoryOptions>();
            containerRegistry.RegisterSingleton<IModuleCatalog, ModuleCatalog>();
            containerRegistry.RegisterSingleton<IModuleManager, ModuleManager>();
            containerRegistry.RegisterSingleton<IModuleInitializer, ModuleInitializer>();
            containerRegistry.RegisterSingleton<IDialogService, DialogService>();
            containerRegistry.Register<INavigationService, ErrorReportingNavigationService>(PrismApplicationBase.NavigationServiceName);
        }
    }
}
