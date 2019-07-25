using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
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

        public static void RegisterPrismCoreServices(this IServiceCollection services)
        {
            services.RegisterSingletonIfNotRegistered<ILogger, ConsoleLoggingService>();
            services.RegisterSingletonIfNotRegistered<ILoggerFacade>(sp => (ILogger)sp.GetService(typeof(ILogger)));
            services.RegisterSingletonIfNotRegistered<IAnalyticsService>(sp => (ILogger)sp.GetService(typeof(ILogger)));
            services.RegisterSingletonIfNotRegistered<ICrashesService>(sp => (ILogger)sp.GetService(typeof(ILogger)));
            services.RegisterSingletonIfNotRegistered<IEventAggregator, EventAggregator>();
            services.RegisterSingletonIfNotRegistered<IModuleCatalog, ModuleCatalog>();
            services.RegisterSingletonIfNotRegistered<IModuleManager, ModuleManager>();
            services.RegisterSingletonIfNotRegistered<IModuleInitializer, ModuleInitializer>();
        }

        private static void RegisterSingletonIfNotRegistered<T, TImpl>(this IServiceCollection services)
            where T : class
            where TImpl : class, T
        {
            if (!services.Any(s => s.ServiceType == typeof(T)))
                services.AddSingleton<T, TImpl>();
        }

        private static void RegisterSingletonIfNotRegistered<T>(this IServiceCollection services, Func<IServiceProvider, T> implementationFactory)
            where T : class
        {
            if (!services.Any(s => s.ServiceType == typeof(T)))
                services.AddSingleton<T>(implementationFactory);
        }
    }
}
