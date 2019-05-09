using Prism;
using Prism.DryIoc;
using Prism.Ioc;
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

        protected override IContainerExtension CreateContainerExtension() => PrismContainerExtension.Current;
    }
}
