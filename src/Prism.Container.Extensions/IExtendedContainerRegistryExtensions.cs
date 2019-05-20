using System;

namespace Prism.Ioc
{
    public static class IExtendedContainerRegistryExtensions
    {
        #region Register Many

        public static IContainerRegistry RegisterManySingleton(this IContainerRegistry containerRegistry, Type implementingType, params Type[] serviceTypes)
        {
            if(containerRegistry is IExtendedContainerRegistry ecr)
            {
                ecr.RegisterManySingleton(implementingType, serviceTypes);
                return ecr;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static IContainerRegistry RegisterManySingleton<T>(this IContainerRegistry containerRegistry, params Type[] serviceTypes) =>
            containerRegistry.RegisterManySingleton(typeof(T), serviceTypes);

        public static IContainerRegistry RegisterMany(this IContainerRegistry containerRegistry, Type implementingType, params Type[] serviceTypes)
        {
            if (containerRegistry is IExtendedContainerRegistry ecr)
            {
                ecr.RegisterMany(implementingType, serviceTypes);
                return ecr;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static IContainerRegistry RegisterMany<T>(this IContainerRegistry containerRegistry, params Type[] serviceTypes) =>
            containerRegistry.RegisterMany(typeof(T), serviceTypes);

        #endregion Register Many

        public static IContainerRegistry RegisterDelegate<T>(this IContainerRegistry containerRegistry, Func<T> factoryMethod) =>
            containerRegistry.RegisterDelegate(typeof(T), () => factoryMethod());

        public static IContainerRegistry RegisterDelegate(this IContainerRegistry containerRegistry, Type serviceType, Func<object> factoryMethod)
        {
            if (containerRegistry is IExtendedContainerRegistry ecr)
            {
                ecr.RegisterDelegate(serviceType, factoryMethod);
                return ecr;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static IContainerRegistry RegisterDelegate<T>(this IContainerRegistry containerRegistry, Func<IContainerProvider, object> factoryMethod) =>
            containerRegistry.RegisterDelegate(typeof(T), factoryMethod);

        public static IContainerRegistry RegisterDelegate(this IContainerRegistry containerRegistry, Type serviceType, Func<IContainerProvider, object> factoryMethod)
        {
            if (containerRegistry is IExtendedContainerRegistry ecr)
            {
                ecr.RegisterDelegate(serviceType, factoryMethod);
                return ecr;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static IContainerRegistry RegisterDelegate<T>(this IContainerRegistry containerRegistry, Func<IServiceProvider, object> factoryMethod) =>
            containerRegistry.RegisterDelegate(typeof(T), factoryMethod);

        public static IContainerRegistry RegisterDelegate(this IContainerRegistry containerRegistry, Type serviceType, Func<IServiceProvider, object> factoryMethod)
        {
            if (containerRegistry is IExtendedContainerRegistry ecr)
            {
                ecr.RegisterDelegate(serviceType, factoryMethod);
                return ecr;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static IContainerRegistry RegisterSingletonFromDelegate<T>(this IContainerRegistry containerRegistry, Func<object> factoryMethod) =>
            containerRegistry.RegisterSingletonFromDelegate(typeof(T), factoryMethod);

        public static IContainerRegistry RegisterSingletonFromDelegate(this IContainerRegistry containerRegistry, Type serviceType, Func<object> factoryMethod)
        {
            if (containerRegistry is IExtendedContainerRegistry ecr)
            {
                ecr.RegisterSingletonFromDelegate(serviceType, factoryMethod);
                return ecr;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static IContainerRegistry RegisterSingletonFromDelegate<T>(this IContainerRegistry containerRegistry, Func<IContainerProvider, object> factoryMethod) =>
            containerRegistry.RegisterSingletonFromDelegate(typeof(T), factoryMethod);

        public static IContainerRegistry RegisterSingletonFromDelegate(this IContainerRegistry containerRegistry, Type serviceType, Func<IContainerProvider, object> factoryMethod)
        {
            if (containerRegistry is IExtendedContainerRegistry ecr)
            {
                ecr.RegisterSingletonFromDelegate(serviceType, factoryMethod);
                return ecr;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static IContainerRegistry RegisterSingletonFromDelegate<T>(this IContainerRegistry containerRegistry, Func<IServiceProvider, object> factoryMethod) =>
            containerRegistry.RegisterSingletonFromDelegate(typeof(T), factoryMethod);

        public static IContainerRegistry RegisterSingletonFromDelegate(this IContainerRegistry containerRegistry, Type serviceType, Func<IServiceProvider, object> factoryMethod)
        {
            if (containerRegistry is IExtendedContainerRegistry ecr)
            {
                ecr.RegisterSingletonFromDelegate(serviceType, factoryMethod);
                return ecr;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
