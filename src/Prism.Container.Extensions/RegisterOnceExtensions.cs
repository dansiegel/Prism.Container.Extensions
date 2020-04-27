using System;
using System.Linq;

namespace Prism.Ioc
{
    public static class RegisterOnceExtensions
    {
        public static IContainerRegistry RegisterSingletonOnce<T, TImp>(this IContainerRegistry containerRegistry)
            where TImp : T
        {
            if (!containerRegistry.IsRegistered<T>())
            {
                containerRegistry.RegisterSingleton<T, TImp>();
            }
            return containerRegistry;
        }

        public static IContainerRegistry RegisterSingletonOnce<T, TImp>(this IContainerRegistry containerRegistry, string name)
            where TImp : T
        {
            if (!containerRegistry.IsRegistered<T>(name))
            {
                containerRegistry.RegisterSingleton<T, TImp>(name);
            }
            return containerRegistry;
        }

        public static IContainerRegistry RegisterOnce<T, TImp>(this IContainerRegistry containerRegistry)
            where TImp : T
        {
            if (!containerRegistry.IsRegistered<T>())
            {
                containerRegistry.Register<T, TImp>();
            }
            return containerRegistry;
        }

        public static IContainerRegistry RegisterOnce<T, TImp>(this IContainerRegistry containerRegistry, string name)
            where TImp : T
        {
            if (!containerRegistry.IsRegistered<T>(name))
            {
                containerRegistry.Register<T, TImp>(name);
            }
            return containerRegistry;
        }

        public static IContainerRegistry RegisterManyOnce<T>(this IContainerRegistry containerRegistry, params Type[] services)
        {
            if(!services?.Any() ?? false)
            {
                services = typeof(T).GetInterfaces();
            }

            services = services.Where(x => !containerRegistry.IsRegistered(x)).ToArray();

            if (services?.Any() ?? false)
            {
                containerRegistry.RegisterMany<T>(services);
            }
            return containerRegistry;
        }

        public static IContainerRegistry RegisterManySingletonOnce<T>(this IContainerRegistry containerRegistry, params Type[] services)
        {
            if (!services?.Any() ?? false)
            {
                services = typeof(T).GetInterfaces();
            }

            services = services.Where(x => !containerRegistry.IsRegistered(x)).ToArray();

            if (services?.Any() ?? false)
            {
                containerRegistry.RegisterManySingleton<T>(services);
            }
            return containerRegistry;
        }
    }
}
