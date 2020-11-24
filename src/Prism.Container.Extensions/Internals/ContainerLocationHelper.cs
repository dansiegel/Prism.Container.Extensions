using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Prism.Ioc;

namespace Prism.Container.Extensions.Internals
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ContainerLocationHelper
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IContainerExtension LocateContainer(IContainerExtension container = null)
        {
            if(container != null)
            {
                ContainerLocator.SetContainerExtension(() => container);
            }

            var located = ContainerLocator.Current;
            if (located != null)
                return located;

            var entryAssembly = Assembly.GetEntryAssembly();
            var containerExtensionAttribute = entryAssembly.GetCustomAttributes().OfType<ContainerExtensionAttribute>().FirstOrDefault();
            return containerExtensionAttribute?.GetContainer();
        }
    }
}
