using System;

namespace Prism.Ioc
{
    public interface IExtendedContainerRegistry : IContainerRegistry, IServiceProvider
    {
        IContainerRegistry RegisterMany(Type implementingType, params Type[] serviceTypes);

        IContainerRegistry RegisterManySingleton(Type implementingType, params Type[] serviceTypes);

        IContainerRegistry Register<T>(Func<T> factoryMethod);

        IContainerRegistry Register<T>(Func<IContainerProvider, T> factoryMethod);

        IContainerRegistry Register<T>(Func<IServiceProvider, T> factoryMethod);

        IContainerRegistry Register(Type serviceType, Func<IServiceProvider, object> factoryMethod);

        IContainerRegistry RegisterSingleton<T>(Func<T> factoryMethod);

        IContainerRegistry RegisterSingleton<T>(Func<IContainerProvider, T> factoryMethod);

        IContainerRegistry RegisterSingleton<T>(Func<IServiceProvider, T> factoryMethod);
    }

    public static class IExtendedContainerRegistryExtensions
    {
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

        public static IContainerRegistry Register<T>(this IContainerRegistry containerRegistry, Func<T> factoryMethod)
        {
            if (containerRegistry is IExtendedContainerRegistry ecr)
            {
                ecr.Register<T>(factoryMethod);
                return ecr;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static IContainerRegistry Register<T>(this IContainerRegistry containerRegistry, Func<IContainerProvider, T> factoryMethod)
        {
            if (containerRegistry is IExtendedContainerRegistry ecr)
            {
                ecr.Register<T>(factoryMethod);
                return ecr;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static IContainerRegistry Register<T>(this IContainerRegistry containerRegistry, Func<IServiceProvider, T> factoryMethod)
        {
            if (containerRegistry is IExtendedContainerRegistry ecr)
            {
                ecr.Register<T>(factoryMethod);
                return ecr;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static IContainerRegistry Register(this IContainerRegistry containerRegistry, Type serviceType, Func<IServiceProvider, object> factoryMethod)
        {
            if (containerRegistry is IExtendedContainerRegistry ecr)
            {
                ecr.Register(serviceType, factoryMethod);
                return ecr;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static IContainerRegistry RegisterSingleton<T>(this IContainerRegistry containerRegistry, Func<T> factoryMethod)
        {
            if (containerRegistry is IExtendedContainerRegistry ecr)
            {
                ecr.Register<T>(factoryMethod);
                return ecr;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static IContainerRegistry RegisterSingleton<T>(this IContainerRegistry containerRegistry, Func<IContainerProvider, T> factoryMethod)
        {
            if (containerRegistry is IExtendedContainerRegistry ecr)
            {
                ecr.Register<T>(factoryMethod);
                return ecr;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
