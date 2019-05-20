using System;
using System.Threading.Tasks;
using Android.Runtime;
using Java.Lang;
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

            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;

            Thread.DefaultUncaughtExceptionHandler = new ExceptHandler(TrackError);

            AppDomain.CurrentDomain.UnhandledException += AppDomain_UnhandledException;

            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            base.Initialize();

            if(Container.IsRegistered<IPageBehaviorFactoryOptions>())
            {
                ((IContainerRegistry)Container).RegisterSingleton<IPageBehaviorFactoryOptions, DefaultPageBehaviorFactoryOptions>();
            }

            Logger = Container.Resolve<ILogger>();
            Log.Listeners.Add(Container.Resolve<FormsLogListener>());
        }

        private void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs args)
        {
            TrackError(args.Exception, nameof(AndroidEnvironment_UnhandledExceptionRaiser));
        }

        private class ExceptHandler : Java.Lang.Object, Thread.IUncaughtExceptionHandler
        {
            public delegate void TrackErrorHandler(System.Exception ex, string fromEvent, object errorObject = null);

            private TrackErrorHandler _handler { get; }

            public ExceptHandler(TrackErrorHandler handler)
            {
                _handler = handler;
            }

            public void UncaughtException(Thread t, Throwable e)
            {
                _handler(e.GetBaseException(), "Thread_DefaultUncaughtExceptionHandler");
            }
        }
    }
}