using System;

namespace Prism.Unity
{
    [Obsolete("PrismApplication is now implemented directly in Prism.Forms.Extended. Please uninstall the Prism.Unity.Forms.Extended NuGet, install the Prism.Unity.Extensions and update your reference to use PrismApplication in Prism.Forms.Extended", error: true)]
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
