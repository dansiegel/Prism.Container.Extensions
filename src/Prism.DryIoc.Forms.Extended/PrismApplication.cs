using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prism.Ioc;
using Prism.Logging;
using Prism.Modularity;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

[assembly: XmlnsDefinition("http://prismlibrary.com", "Prism.DryIoc")]
namespace Prism.DryIoc
{
    public abstract partial class PrismApplication : PrismApplicationBase
    {
        public PrismApplication()
        {
        }

        public PrismApplication(IPlatformInitializer platformInitializer) 
            : base(platformInitializer)
        {
        }

        public PrismApplication(IPlatformInitializer platformInitializer, bool setFormsDependencyResolver) 
            : base(platformInitializer, setFormsDependencyResolver)
        {
        }

        public ILogger Logger { get; private set; }

        protected IModuleCatalog ModuleCatalog { get; private set; }

        protected override IContainerExtension CreateContainerExtension() => PrismContainerExtension.Current;

        protected override sealed void RegisterRequiredTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterManySingleton<ConsoleLoggingService>();
            base.RegisterRequiredTypes(containerRegistry);
        }

        protected override void InitializeModules()
        {
            if(ModuleCatalog is null)
            {
                ModuleCatalog = Container.Resolve<IModuleCatalog>();
            }

            if(ModuleCatalog.Modules.Any())
            {
                var manager = Container.Resolve<IModuleManager>();
                manager.LoadModuleCompleted += PrismApplication_LoadModuleCompleted;
                manager.Run();
            }
        }

        private void PrismApplication_LoadModuleCompleted(object sender, LoadModuleCompletedEventArgs e)
        {
            LoadModuleCompleted(e.ModuleInfo, e.Error, e.IsErrorHandled);
        }

        protected virtual void LoadModuleCompleted(IModuleInfo moduleInfo, Exception error, bool isHandled)
        {
            if (error != null)
            {
                Logger.Debug($"An error occurred while loading {moduleInfo.ModuleName}");
                Logger.Report(error, new Dictionary<string, string>
                {
                    { "moduleName", moduleInfo.ModuleName },
                    { "initializationMode", $"{moduleInfo.InitializationMode}" },
                    { "moduleType", moduleInfo.ModuleType }
                });
            }
        }

        private void AppDomain_UnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            if (args.ExceptionObject is Exception ex)
                TrackError(ex, nameof(AppDomain_UnhandledException));
            else
                TrackError(null, nameof(AppDomain_UnhandledException), args.ExceptionObject);
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs args)
        {
            args.SetObserved();
            TrackError(args.Exception, nameof(TaskScheduler_UnobservedTaskException));
        }

        protected virtual void TrackError(Exception ex, string fromEvent, object errorObject = null)
        {
            if (errorObject != null)
            {
                System.Diagnostics.Trace.WriteLine(errorObject);
            }
            else
            {
                System.Diagnostics.Trace.WriteLine(ex);
                Logger.Report(ex, new Dictionary<string, string> { { "fromUnobservedEvent", fromEvent } });
            }
        }

        class FormsLogListener : LogListener
        {
            private ILogger Logger { get; }

            public FormsLogListener(ILogger logger)
            {
                Logger = logger;
            }

            public override void Warning(string category, string message)
            {
                Logger.Debug($"{category}: {message}");
            }
        }
    }
}
