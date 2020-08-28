using System;
using System.Collections.Generic;
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
    public static class RequiredTypesExtensions
    {
        // Provided to keep compatibility with Prism 8.0
        private const string NavigationServiceName = "PageNavigationService";

        public static void RegisterRequiredTypes(this IContainerRegistry containerRegistry)
        {
            if (!containerRegistry.IsRegistered<IAggregateLogger>())
                containerRegistry.UseAggregateLogger();

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
            containerRegistry.Register<INavigationService, ErrorReportingNavigationService>(NavigationServiceName);
            containerRegistry.RegisterScoped<INavigationService, ErrorReportingNavigationService>();
        }

        public static void RegisterPrismCoreServices(this IServiceCollection services)
        {
            if(!services.IsRegistered<IAggregateLogger>())
            {
                services.AddSingleton<AggregateLogger>(p =>
                {
                    var logger = new AggregateLogger();
                    logger.AddLoggers(p.GetService<IEnumerable<IAggregableLogger>>());
                    return logger;
                });
                services.AddTransient<IAggregateLogger>(p => p.GetRequiredService<AggregateLogger>());
                services.AddTransient<ILogger>(p => p.GetRequiredService<AggregateLogger>());
                services.AddTransient<IAnalyticsService>(p => p.GetRequiredService<AggregateLogger>());
                services.AddTransient<ICrashesService>(p => p.GetRequiredService<AggregateLogger>());
            }
            services.RegisterSingletonIfNotRegistered<IEventAggregator, EventAggregator>();
            services.RegisterSingletonIfNotRegistered<IModuleCatalog, ModuleCatalog>();
            services.RegisterSingletonIfNotRegistered<IModuleManager, ModuleManager>();
            services.RegisterSingletonIfNotRegistered<IModuleInitializer, ModuleInitializer>();
        }

        private static bool IsRegistered<T>(this IServiceCollection services) =>
            services.Any(x => x.ServiceType == typeof(T));

        private static void RegisterSingletonIfNotRegistered<T, TImpl>(this IServiceCollection services)
            where T : class
            where TImpl : class, T
        {
            if (!services.Any(s => s.ServiceType == typeof(T)))
                services.AddSingleton<T, TImpl>();
        }
    }
}
