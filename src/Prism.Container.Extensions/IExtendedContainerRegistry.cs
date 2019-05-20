using System;

namespace Prism.Ioc
{
    public interface IExtendedContainerRegistry : IContainerRegistry, IServiceProvider
    {
        IContainerRegistry RegisterMany(Type implementingType, params Type[] serviceTypes);

        IContainerRegistry RegisterManySingleton(Type implementingType, params Type[] serviceTypes);

        IContainerRegistry RegisterDelegate(Type serviceType, Func<object> factoryMethod);

        IContainerRegistry RegisterDelegate(Type serviceType, Func<IContainerProvider, object> factoryMethod);

        IContainerRegistry RegisterDelegate(Type serviceType, Func<IServiceProvider, object> factoryMethod);

        IContainerRegistry RegisterSingletonFromDelegate(Type serviceType, Func<object> factoryMethod);

        IContainerRegistry RegisterSingletonFromDelegate(Type serviceType, Func<IContainerProvider, object> factoryMethod);

        IContainerRegistry RegisterSingletonFromDelegate(Type serviceType, Func<IServiceProvider, object> factoryMethod);
    }
}
