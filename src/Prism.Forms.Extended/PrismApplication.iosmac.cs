using System;
using System.Threading.Tasks;
using ObjCRuntime;
using Prism.Events;
using Prism.Forms.Extended.Styles;
using Prism.Ioc;
using Prism.Logging;
using Xamarin.Forms.Internals;

namespace Prism
{
    public abstract partial class PrismApplication
    {
        protected override void Initialize()
        {
            Resources = new DefaultResources();
            Logger = new ConsoleLoggingService();

            Runtime.MarshalObjectiveCException += Runtime_MarshalObjectiveCException;

            AppDomain.CurrentDomain.UnhandledException += AppDomain_UnhandledException;

            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            base.Initialize();

            Logger = Container.Resolve<ILogger>();
            Log.Listeners.Add(new FormsLogListener(Logger));
            Container.Resolve<IEventAggregator>().GetEvent<NavigationErrorEvent>().Subscribe(OnNavigationError);
        }

        private void Runtime_MarshalObjectiveCException(object sender, MarshalObjectiveCExceptionEventArgs args)
        {
            Console.WriteLine($"Encountered Marshal Objective-C Exception:\nMode: {args.ExceptionMode}\nException:\n{args.Exception}");
        }
    }
}