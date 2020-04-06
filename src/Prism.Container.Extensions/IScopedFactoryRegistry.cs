using System;
using System.ComponentModel;
using Prism.Ioc;

namespace Prism.Container.Extensions
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IScopedFactoryRegistry
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        IContainerRegistry RegisterScopedFromDelegate(Type serviceType, Func<object> factoryMethod);

        [EditorBrowsable(EditorBrowsableState.Never)]
        IContainerRegistry RegisterScopedFromDelegate(Type serviceType, Func<IContainerProvider, object> factoryMethod);

        [EditorBrowsable(EditorBrowsableState.Never)]
        IContainerRegistry RegisterScopedFromDelegate(Type serviceType, Func<IServiceProvider, object> factoryMethod);
    }
}
