using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.Ioc
{
    public static class IServiceProviderFactoryExtensions
    {
        public static IContainerRegistry Register<T>(this IContainerRegistry containerRegistry, Func<IServiceProvider, object> factoryDelegate) =>
            Register(containerRegistry, typeof(T), factoryDelegate);

        public static IContainerRegistry Register(this IContainerRegistry containerRegistry, Type type, Func<IServiceProvider, object> factoryDelegate)
        {
            object ResolveDelegate(IContainerProvider containerProvider)
            {
                var sp = containerProvider.Resolve<IServiceProvider>();
                return factoryDelegate(sp);
            }

            return containerRegistry.Register(type, ResolveDelegate);
        }

        public static IContainerRegistry RegisterSingleton<T>(this IContainerRegistry containerRegistry, Func<IServiceProvider, object> factoryDelegate) =>
            RegisterSingleton(containerRegistry, typeof(T), factoryDelegate);

        public static IContainerRegistry RegisterSingleton(this IContainerRegistry containerRegistry, Type type, Func<IServiceProvider, object> factoryDelegate)
        {
            object ResolveDelegate(IContainerProvider containerProvider)
            {
                var sp = containerProvider.Resolve<IServiceProvider>();
                return factoryDelegate(sp);
            }

            return containerRegistry.Register(type, ResolveDelegate);
        }

        public static IContainerRegistry RegisterScoped<T>(this IContainerRegistry containerRegistry, Func<IServiceProvider, object> factoryDelegate) =>
            RegisterScoped(containerRegistry, typeof(T), factoryDelegate);

        public static IContainerRegistry RegisterScoped(this IContainerRegistry containerRegistry, Type type, Func<IServiceProvider, object> factoryDelegate)
        {
            object ResolveDelegate(IContainerProvider containerProvider)
            {
                var sp = containerProvider.Resolve<IServiceProvider>();
                return factoryDelegate(sp);
            }

            return containerRegistry.Register(type, ResolveDelegate);
        }
    }
}
