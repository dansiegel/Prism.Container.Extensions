using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;
using Prism.Forms.Extended.ViewModels;
using Prism.Ioc;
using Prism.Logging;
using Prism.Modularity;
using Prism.Mvvm;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

[assembly: InternalsVisibleTo("Prism.DryIoc.Forms.Extended.Tests")]
[assembly: InternalsVisibleTo("Prism.Unity.Forms.Extended.Tests")]
[assembly: XmlnsDefinition("http://prismlibrary.com", "Prism.Platform")]
namespace Prism
{
    public abstract partial class PrismApplicationBaseExtended : PrismApplicationBase
    {
        protected PrismApplicationBaseExtended()
        {
        }

        protected PrismApplicationBaseExtended(IPlatformInitializer platformInitializer) : base(platformInitializer)
        {
        }

        protected PrismApplicationBaseExtended(IPlatformInitializer platformInitializer, bool setFormsDependencyResolver) : base(platformInitializer, setFormsDependencyResolver)
        {
        }

        public ILogger Logger { get; private set; }

        protected IModuleCatalog ModuleCatalog { get; private set; }



        protected override sealed void RegisterRequiredTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterRequiredTypes();
            ViewModelLocationProvider.Register<TabbedPage, DefaultViewModel>();
        }

        protected sealed override void InitializeModules()
        {
            if (ModuleCatalog is null)
            {
                ModuleCatalog = Container.Resolve<IModuleCatalog>();
            }
            else
            {
                ((IContainerExtension)Container).RegisterInstance<IModuleCatalog>(ModuleCatalog);
            }

            if (ModuleCatalog.Modules.Any())
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

        protected virtual void OnNavigationError(INavigationError navigationError)
        {
            Console.WriteLine("A Navigation Error was encountered from the Default Error Handler PrismApplication.OnNavigationError");

            var parameters = navigationError.Parameters is null ? string.Empty : Serialize(navigationError.Parameters);
            Container.Resolve<ICrashesService>().Report(navigationError.Exception, ("navigationParameters", parameters), ("navigationUri", navigationError.NavigationUri));
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

        private static string Serialize<T>(T instance)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, instance);
                return Encoding.Default.GetString(stream.ToArray());
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
