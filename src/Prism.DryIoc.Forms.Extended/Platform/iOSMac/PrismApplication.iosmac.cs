using System;
using System.Threading.Tasks;
using ObjCRuntime;
using Prism.Behaviors;
using Prism.DryIoc.Extensions;
using Prism.Ioc;
using Prism.Logging;
using Xamarin.Forms.Internals;

namespace Prism.DryIoc
{
    public abstract partial class PrismApplication
    {
        public override void Initialize()
        {
            Logger = new ConsoleLoggingService();

            Runtime.MarshalObjectiveCException += Runtime_MarshalObjectiveCException;

            AppDomain.CurrentDomain.UnhandledException += AppDomain_UnhandledException;

            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            base.Initialize();

            if (Container.IsRegistered<IPageBehaviorFactoryOptions>())
            {
                ((IContainerRegistry)Container).RegisterSingleton<IPageBehaviorFactoryOptions, DefaultPageBehaviorFactoryOptions>();
            }

            Logger = Container.Resolve<ILogger>();
            Log.Listeners.Add(Container.Resolve<FormsLogListener>());
        }

        private void Runtime_MarshalObjectiveCException(object sender, MarshalObjectiveCExceptionEventArgs args)
        {
            Console.WriteLine($"Encountered Marshal Objective-C Exception:\nMode: {args.ExceptionMode}\nException:\n{args.Exception}");
        }
    }
}