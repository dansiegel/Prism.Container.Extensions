using Prism.AppModel;
using Prism.Behaviors;
using Prism.Common;
using Prism.Events;
using Prism.Logging;
using Prism.Modularity;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;

namespace Prism.Ioc
{
    public static class IContainerRegistryExtensions
    {
        public static void RegisterRequiredTypes(this IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterManySingletonOnce<ConsoleLoggingService>();
            containerRegistry.RegisterSingletonOnce<IApplicationProvider, ApplicationProvider>();
            containerRegistry.RegisterSingletonOnce<IApplicationStore, ApplicationStore>();
            containerRegistry.RegisterSingletonOnce<IEventAggregator, EventAggregator>();
            containerRegistry.RegisterSingletonOnce<IPageDialogService, PageDialogService>();
            containerRegistry.RegisterSingletonOnce<IDeviceService, DeviceService>();
            containerRegistry.RegisterSingletonOnce<IPageBehaviorFactory, ExtendedPageBehaviorFactory>();
            containerRegistry.RegisterSingletonOnce<IPageBehaviorFactoryOptions, DefaultPageBehaviorFactoryOptions>();
            containerRegistry.RegisterSingletonOnce<IModuleCatalog, ModuleCatalog>();
            containerRegistry.RegisterSingletonOnce<IModuleManager, ModuleManager>();
            containerRegistry.RegisterSingletonOnce<IModuleInitializer, ModuleInitializer>();
            containerRegistry.RegisterSingletonOnce<IDialogService, DialogService>();
            containerRegistry.RegisterOnce<INavigationService, ErrorReportingNavigationService>(PrismApplicationBase.NavigationServiceName);
        }
    }
}
