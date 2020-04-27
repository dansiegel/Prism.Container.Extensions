using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Prism.Ioc;

namespace Prism.Container.Extensions
{
#pragma warning disable CS1591
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ContainerLocator
    {
        private static ContainerExtensionAttribute _locatedExtension;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IContainerExtension Locate()
        {
            if (_locatedExtension != null)
            {
                return _locatedExtension.Container;
            }
            else if (!LocatedAssembly(AppDomain.CurrentDomain.GetAssemblies(), out _locatedExtension))
            {
                throw new DllNotFoundException("No assembly containing a reference to an IContainerExtension implementation could be found. You must have a reference to one of the DI Container Extensions in your final project");
            }

            Prism.Ioc.ContainerLocator.SetContainerExtension(() => _locatedExtension.Container);
            return _locatedExtension.Container;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void LocatePreservedReference(Type type)
        {
            if (_locatedExtension is null)
            {
                if (!LocatedAssembly(new[] { type.Assembly }, out _locatedExtension))
                {
                    throw new DllNotFoundException("No assembly containing a reference to an IContainerExtension implementation could be found. You must have a reference to one of the DI Container Extensions in your final project");
                }

                Console.WriteLine($"Located Container: {_locatedExtension.Container.GetType().FullName}");
            }
        }

        private static bool LocatedAssembly(IEnumerable<Assembly> assemblies, out ContainerExtensionAttribute attribute)
        {
            attribute = null;
            foreach(var assembly in assemblies)
            {
                attribute = assembly.GetCustomAttribute<ContainerExtensionAttribute>();
                if(attribute != null)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// This should only ever be called from Unit Tests
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void Reset()
        {
            _locatedExtension = null;
        }
    }
#pragma warning restore CS1591
}
