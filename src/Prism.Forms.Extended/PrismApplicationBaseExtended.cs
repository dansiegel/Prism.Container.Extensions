using System;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

[assembly: InternalsVisibleTo("Prism.DryIoc.Forms.Extended.Tests")]
[assembly: InternalsVisibleTo("Prism.Unity.Forms.Extended.Tests")]
[assembly: XmlnsDefinition("http://prismlibrary.com", "Prism")]
[assembly: XmlnsDefinition("http://prismlibrary.com", "Prism.Platform")]
namespace Prism
{
    [Obsolete("PrismApplication is now directly provided in Prism.Forms.Extended.")]
    public abstract class PrismApplicationBaseExtended : PrismApplication
    {
        protected PrismApplicationBaseExtended()
        {
        }

        protected PrismApplicationBaseExtended(IPlatformInitializer platformInitializer) : base(platformInitializer)
        {
        }

        protected PrismApplicationBaseExtended(IPlatformInitializer platformInitializer, bool setFormsDependencyResolver) : base(platformInitializer, setFormsDependencyResolver)
        {
        }
    }
}
