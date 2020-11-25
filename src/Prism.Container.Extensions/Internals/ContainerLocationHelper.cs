using System;
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
            if (container != null)
            {
                ContainerLocator.SetContainerExtension(() => container);
            }

            try
            {
                var located = ContainerLocator.Current;
                if (located != null)
                    return located;

                // If somehow we have an actual null container let's be sure to refresh the Locator
                ContainerLocator.ResetContainer();
            }
            catch
            {
                // suppress any errors
                ContainerLocator.ResetContainer();
            }

            var entryAssembly = Assembly.GetEntryAssembly();
            var containerAttributes = entryAssembly.GetCustomAttributes().OfType<ContainerExtensionAttribute>();

            if (!containerAttributes.Any())
                throw new InvalidOperationException("An instance of the IContainerExtension has not been registered with the ContainerLocator, and no ContainerExtensionAttribute has been found in the entry assembly.");
            else if (containerAttributes.Count() > 1)
                throw new InvalidOperationException("More than one ContainerExtensionAttribute has been found on the entry assembly. Only a single ContainerExtension should be referenced.");

            var containerExtensionAttribute = containerAttributes.First();
            return containerExtensionAttribute.GetContainer();
        }
    }
}
