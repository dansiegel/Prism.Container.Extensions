using Prism.Ioc;
using Xamarin.Forms;

[assembly: XmlnsDefinition("http://prismlibrary.com", "Prism.Microsoft.DependencyInjection")]
namespace Prism.Microsoft.DependencyInjection
{
    public abstract class PrismApplication : PrismApplicationBaseExtended
    {
        protected PrismApplication()
        {
        }

        protected PrismApplication(IPlatformInitializer platformInitializer)
            : base(platformInitializer)
        {
        }

        protected PrismApplication(IPlatformInitializer platformInitializer, bool setFormsDependencyResolver)
            : base(platformInitializer, setFormsDependencyResolver)
        {
        }

        protected override IContainerExtension CreateContainerExtension() =>
            PrismContainerExtension.Current;
    }
}
