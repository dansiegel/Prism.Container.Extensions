using Prism.Ioc;
using Prism.Unity.Extensions;
using System;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

[assembly: XmlnsDefinition("http://prismlibrary.com", "Prism.Unity")]
namespace Prism.Unity
{
    public abstract class PrismApplication : PrismApplicationBaseExtended
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

        protected override IContainerExtension CreateContainerExtension() => PrismContainerExtension.Current;
    }
}
