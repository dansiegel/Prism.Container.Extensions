using Microsoft.Extensions.DependencyInjection;
using System;

namespace Prism.Ioc
{
    public static class IServiceProviderExtensions
    {
        public static IServiceProvider CreateServiceProvider(this IContainerProvider container, IServiceCollection services)
        {
            var containerRegistry = container as IContainerRegistry;
            RegisterTypesWithPrismContainer(containerRegistry, services);
            var serviceProvider = container is IServiceProvider sp ? sp : new PrismServiceProvider(container);

            if (!containerRegistry.IsRegistered<IServiceProvider>())
            {
                containerRegistry.RegisterInstance<IServiceProvider>(serviceProvider);
            }

            return serviceProvider;
        }

        private static void RegisterTypesWithPrismContainer(IContainerRegistry containerRegistry, IServiceCollection services)
        {
            foreach (var service in services)
            {
                switch (service.Lifetime)
                {
                    case ServiceLifetime.Singleton:
                        if (service.ImplementationType != null)
                            containerRegistry.RegisterSingleton(service.ServiceType, service.ImplementationType);
                        else if (service.ImplementationInstance != null)
                            containerRegistry.RegisterInstance(service.ServiceType, service.ImplementationInstance);
                        else if (service.ImplementationFactory != null)
                            containerRegistry.RegisterSingletonFromDelegate(service.ServiceType, service.ImplementationFactory);
                        break;
                    case ServiceLifetime.Transient:
                        if (service.ImplementationType != null)
                            containerRegistry.Register(service.ServiceType, service.ImplementationType);
                        else if (service.ImplementationFactory != null)
                            containerRegistry.RegisterDelegate(service.ServiceType, service.ImplementationFactory);
                        // Transient Lifetime cannot occur with an Instance
                        break;
                    case ServiceLifetime.Scoped:
                        if (service.ImplementationType is null)
                            containerRegistry.RegisterScoped(service.ServiceType);
                        else
                            containerRegistry.RegisterScoped(service.ServiceType, service.ImplementationType);
                        break;
                }
            }
        }
    }
}
