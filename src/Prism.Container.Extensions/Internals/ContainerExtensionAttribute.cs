using System;
using System.ComponentModel;
using Prism.Ioc;

namespace Prism.Container.Extensions.Internals
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public abstract class ContainerExtensionAttribute : Attribute
    {
        public IContainerExtension GetContainer()
        {
            var container = Init();
            ContainerLocator.SetContainerExtension(() => container);
            return ContainerLocator.Current;
        }

        protected abstract IContainerExtension Init();
    }
}
