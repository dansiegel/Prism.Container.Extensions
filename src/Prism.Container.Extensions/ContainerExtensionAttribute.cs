using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Prism.Ioc;

namespace Prism.Container.Extensions
{
#pragma warning disable CS1591
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
            var containerExtensionProperty = containerExtensionType.GetProperty("Current", BindingFlags.Public | BindingFlags.Static);

            if(containerExtensionProperty is null)
            {
                containerExtensionProperty = containerExtensionType.GetProperties(BindingFlags.Public | BindingFlags.Static)
                                                                   .FirstOrDefault(p => p.PropertyType.IsAssignableFrom(typeof(IContainerExtension)));
            }

            return (IContainerExtension)containerExtensionProperty?.GetValue(null);
        }
    }
#pragma warning restore CS1591
}
