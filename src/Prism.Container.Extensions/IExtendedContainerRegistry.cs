using System;
using System.ComponentModel;

namespace Prism.Ioc
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IExtendedContainerRegistry : IContainerRegistry, IServiceProvider
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        IContainerRegistry RegisterMany(Type implementingType, params Type[] serviceTypes);

        [EditorBrowsable(EditorBrowsableState.Never)]
        IContainerRegistry RegisterManySingleton(Type implementingType, params Type[] serviceTypes);

        [EditorBrowsable(EditorBrowsableState.Never)]
        IContainerRegistry RegisterDelegate(Type serviceType, Func<object> factoryMethod);

        [EditorBrowsable(EditorBrowsableState.Never)]
        IContainerRegistry RegisterDelegate(Type serviceType, Func<IContainerProvider, object> factoryMethod);

        [EditorBrowsable(EditorBrowsableState.Never)]
        IContainerRegistry RegisterDelegate(Type serviceType, Func<IServiceProvider, object> factoryMethod);

        [EditorBrowsable(EditorBrowsableState.Never)]
        IContainerRegistry RegisterSingletonFromDelegate(Type serviceType, Func<object> factoryMethod);

        [EditorBrowsable(EditorBrowsableState.Never)]
        IContainerRegistry RegisterSingletonFromDelegate(Type serviceType, Func<IContainerProvider, object> factoryMethod);

        [EditorBrowsable(EditorBrowsableState.Never)]
        IContainerRegistry RegisterSingletonFromDelegate(Type serviceType, Func<IServiceProvider, object> factoryMethod);

        [EditorBrowsable(EditorBrowsableState.Never)]
        IContainerRegistry RegisterScoped(Type serviceType);

        [EditorBrowsable(EditorBrowsableState.Never)]
        IContainerRegistry RegisterScoped(Type serviceType, Type implementationType);
    }
}
