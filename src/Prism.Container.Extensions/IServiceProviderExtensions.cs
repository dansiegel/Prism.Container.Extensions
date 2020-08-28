using Microsoft.Extensions.DependencyInjection;
using System;

namespace Prism.Ioc
{
    public static class IServiceProviderExtensions
    {
        /// <summary>
        /// Creates a <see cref="IServiceProvider" /> using the underlying Container
        /// </summary>
        /// <param name="container">The <see cref="IContainerExtension" /></param>
        /// <param name="services">The <see cref="IServiceCollection"/> to register with the container</param>
        /// <returns>The <see cref="IServiceProvider" /></returns>
        public static IServiceProvider CreateServiceProvider(this IContainerExtension container, IServiceCollection services)
        {
            var containerRegistry = container as IContainerRegistry;
            RegisterTypesWithPrismContainer(containerRegistry, services);
            var serviceProvider = container is IServiceProvider sp ? sp : new PrismServiceProvider(container);

            if (!containerRegistry.IsRegistered<IServiceProvider>())
            {
                containerRegistry.RegisterInstance<IServiceProvider>(serviceProvider);
            }

            return container.Resolve<IServiceProvider>();
        }

        internal static void RegisterTypesWithPrismContainer(IContainerRegistry containerRegistry, IServiceCollection services)
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
                            containerRegistry.RegisterSingleton(service.ServiceType, c => ServiceFactory(c, service.ImplementationFactory));
                        break;
                    case ServiceLifetime.Transient:
                        if (service.ImplementationType != null)
                            containerRegistry.Register(service.ServiceType, service.ImplementationType);
                        else if (service.ImplementationFactory != null)
                            containerRegistry.Register(service.ServiceType, c => ServiceFactory(c, service.ImplementationFactory));
                        // Transient Lifetime cannot occur with an Instance
                        break;
                    case ServiceLifetime.Scoped:
                        if (service.ImplementationType != null)
                            containerRegistry.RegisterScoped(service.ServiceType, service.ImplementationType);
                        else if (service.ImplementationType is null)
                            containerRegistry.RegisterScoped(service.ServiceType, c => ServiceFactory(c, service.ImplementationFactory));
                        else if (service.ServiceType.IsAbstract)
                            throw new NotSupportedException($"Cannot register the service {service.ServiceType.FullName} as it is an abstract type");
                        else if (service.ServiceType.IsInterface)
                            throw new NotSupportedException($"Cannot register the service {service.ServiceType.FullName} as it is an interface. You must provide a concrete implementation");
                        else
                            containerRegistry.RegisterScoped(service.ServiceType);
                        break;
                }
            }
        }

        private static object ServiceFactory(IContainerProvider container, Func<IServiceProvider, object> implementationFactory)
        {
            var sp = container.Resolve<IServiceProvider>();
            return implementationFactory(sp);
        }
    }
}
