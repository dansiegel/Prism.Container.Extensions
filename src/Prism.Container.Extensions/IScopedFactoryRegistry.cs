using System;
using Prism.Ioc;

namespace Prism.Container.Extensions
{
    public interface IScopedFactoryRegistry
    {
        IContainerRegistry RegisterScopedFromDelegate(Type serviceType, Func<object> factoryMethod);

        IContainerRegistry RegisterScopedFromDelegate(Type serviceType, Func<IContainerProvider, object> factoryMethod);

        IContainerRegistry RegisterScopedFromDelegate(Type serviceType, Func<IServiceProvider, object> factoryMethod);
    }
}
