using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Prism.Container.Extensions;

namespace Prism.Ioc
{
    public static class IServiceCollectionExtensions
    {
        public static IContainerRegistry RegisterServices(this IContainerRegistry containerRegistry, Action<IServiceCollection> registerServices)
        {
            if(containerRegistry is IServiceCollectionAware sca)
            {
                sca.RegisterServices(registerServices);
            }
            else
            {
                var services = new ServiceCollection();
                registerServices(services);
                IServiceProviderExtensions.RegisterTypesWithPrismContainer(containerRegistry, services);
            }

            return containerRegistry;
        }
    }

    internal class ServiceCollection : List<ServiceDescriptor>, IServiceCollection
    {

    }
}
