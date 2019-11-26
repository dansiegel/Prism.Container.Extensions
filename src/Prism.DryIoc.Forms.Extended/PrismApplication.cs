using Prism.Ioc;
using System;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

[assembly: InternalsVisibleTo("Prism.DryIoc.Forms.Extended.Tests")]
namespace Prism.DryIoc
{
    [Obsolete("PrismApplication is now implemented directly in Prism.Forms.Extended. Please uninstall the Prism.DryIoc.Forms.Extended NuGet, install the Prism.DryIoc.Exentensions and update your reference to use PrismApplication in Prism.Forms.Extended", error: true)]
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
    }
}
