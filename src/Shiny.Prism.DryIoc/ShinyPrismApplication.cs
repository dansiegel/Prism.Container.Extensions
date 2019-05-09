using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using Xamarin.Forms;

[assembly: XmlnsDefinition("http://prismlibrary.com", "Shiny.Prism.DryIoc")]
namespace Shiny.Prism.DryIoc
{
    public abstract class ShinyPrismApplication : PrismApplicationBase
    {
        public ShinyPrismApplication()
        {
        }

        public ShinyPrismApplication(IPlatformInitializer platformInitializer) : base(platformInitializer)
        {
        }

        public ShinyPrismApplication(IPlatformInitializer platformInitializer, bool setFormsDependencyResolver) : base(platformInitializer, setFormsDependencyResolver)
        {
        }

        protected override IContainerExtension CreateContainerExtension() => PrismContainerExtension.Current;
    }
}
