using System;
using System.ComponentModel;
using System.Reflection;
using Prism.Ioc;

namespace Prism.Container.Extensions
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public class ContainerExtensionAttribute : Attribute
    {
        private Type containerExtensionType { get; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public ContainerExtensionAttribute(Type type)
        {
            containerExtensionType = type;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public IContainerExtension Container => GetContainerExtension();

        private IContainerExtension GetContainerExtension()
        {
            var current = containerExtensionType.GetProperty("Current", BindingFlags.Public | BindingFlags.Static);
            return (IContainerExtension)current.GetValue(null);
        }
    }
}
