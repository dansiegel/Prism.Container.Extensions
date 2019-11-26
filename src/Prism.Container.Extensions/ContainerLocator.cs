using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Prism.Ioc;

namespace Prism.Container.Extensions
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ContainerLocator
    {
        private static ContainerExtensionAttribute _locatedExtension;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IContainerExtension Locate()
        {
            if (_locatedExtension != null) return _locatedExtension.Container;

            var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetCustomAttribute<ContainerExtensionAttribute>() != null);
            _locatedExtension = assembly?.GetCustomAttribute<ContainerExtensionAttribute>();

            return _locatedExtension?.Container ?? throw new DllNotFoundException("No assembly containing a reference to an IContainerExtension implementation could be found. You must have a reference to one of the DI Container Extensions in your final project");
        }
    }
}
