using System;

namespace Prism.Ioc
{
    public static class IScopedFactoryRegistryExtensions
    {
        public static IContainerRegistry RegisterScopedFromDelegate<T>(this IContainerRegistry containerRegistry, Func<object> factoryMethod) =>
            RegisterScopedFromDelegate(containerRegistry, typeof(T), factoryMethod);

        public static IContainerRegistry RegisterScopedFromDelegate(this IContainerRegistry containerRegistry, Type serviceType, Func<object> factoryMethod)
        {
            if (containerRegistry is IScopedFactoryRegistry scopedRegistry)
            {
                scopedRegistry.RegisterScopedFromDelegate(serviceType, factoryMethod);
                return containerRegistry;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static IContainerRegistry RegisterScopedFromDelegate<T>(this IContainerRegistry containerRegistry, Func<IContainerProvider, object> factoryMethod) =>
            RegisterScopedFromDelegate(containerRegistry, typeof(T), factoryMethod);

        public static IContainerRegistry RegisterScopedFromDelegate(this IContainerRegistry containerRegistry, Type serviceType, Func<IContainerProvider, object> factoryMethod)
        {
            if (containerRegistry is IScopedFactoryRegistry scopedRegistry)
            {
                scopedRegistry.RegisterScopedFromDelegate(serviceType, factoryMethod);
                return containerRegistry;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static IContainerRegistry RegisterScopedFromDelegate<T>(this IContainerRegistry containerRegistry, Func<IServiceProvider, object> factoryMethod) =>
            RegisterScopedFromDelegate(containerRegistry, typeof(T), factoryMethod);

        public static IContainerRegistry RegisterScopedFromDelegate(this IContainerRegistry containerRegistry, Type serviceType, Func<IServiceProvider, object> factoryMethod)
        {
            if (containerRegistry is IScopedFactoryRegistry scopedRegistry)
            {
                scopedRegistry.RegisterScopedFromDelegate(serviceType, factoryMethod);
                return containerRegistry;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
