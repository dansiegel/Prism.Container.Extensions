using System;
using Prism.Ioc;
using Xamarin.Forms;

namespace Prism.Microsoft.DependencyInjection
{
    [Obsolete("PrismApplication is now implemented directly in Prism.Forms.Extended. Please uninstall the Prism.Microsoft.DependencyInjection.Forms.Extended NuGet, install the Prism.Microsoft.DependencyInjection.Extensions NuGet and update your reference to use PrismApplication in Prism.Forms.Extended", error: true)]
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
    }
}
