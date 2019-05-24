using System;
using System.Threading.Tasks;
using Prism.DryIoc.Events;
using Prism.Events;
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

            AppDomain.CurrentDomain.UnhandledException += AppDomain_UnhandledException;

            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            base.Initialize();

            Logger = Container.Resolve<ILogger>();
            Log.Listeners.Add(Container.Resolve<FormsLogListener>());
            Container.Resolve<IEventAggregator>().GetEvent<NavigationErrorEvent>().Subscribe(OnNavigationError);
        }
    }
}