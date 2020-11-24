using System;
using System.ComponentModel;
using Prism.Container.Extensions.Internals;
using Prism.Ioc;

namespace Prism.DryIoc.Internals
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public sealed class PrismContainerExtensionAttribute : ContainerExtensionAttribute
    {
        protected override IContainerExtension Init() => PrismContainerExtension.Init();
    }
}
