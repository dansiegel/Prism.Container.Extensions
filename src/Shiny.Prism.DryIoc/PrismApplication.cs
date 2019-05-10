using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Logging;
using Xamarin.Forms;

[assembly: XmlnsDefinition("http://prismlibrary.com", "Shiny.Prism.DryIoc")]
namespace Shiny.Prism.DryIoc
{
    public abstract class PrismApplication : PrismApplicationBase
    {
        public PrismApplication()
        {
        }

        public PrismApplication(IPlatformInitializer platformInitializer) : base(platformInitializer)
        {
        }

        public PrismApplication(IPlatformInitializer platformInitializer, bool setFormsDependencyResolver) : base(platformInitializer, setFormsDependencyResolver)
        {
        }

        public ILogger Logger { get; private set; }

        protected override IContainerExtension CreateContainerExtension() => PrismContainerExtension.Current;

        public override void Initialize()
        {
            base.Initialize();
            Logger = Container.Resolve<ILogger>();
        }

        protected override sealed void RegisterRequiredTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterManySingleton<ConsoleLoggingService>();
            base.RegisterRequiredTypes(containerRegistry);
        }
    }
}
