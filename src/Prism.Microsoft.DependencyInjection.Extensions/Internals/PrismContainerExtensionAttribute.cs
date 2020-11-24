using System;
using System.ComponentModel;
using Prism.Ioc;

namespace Prism.Microsoft.DependencyInjection.Internals
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public sealed class PrismContainerExtensionAttribute : Attribute
    {
        public PrismContainerExtensionAttribute()
        {
            ContainerLocator.SetContainerExtension(() => PrismContainerExtension.Init());
        }
    }
}
