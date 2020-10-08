using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Prism.Container.Extensions;

namespace Prism.Ioc
{
    /// <summary>
    /// Adds extensions for <see cref="IContainerRegistry" /> to use <see cref="IServiceCollection" />
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Registers services with the Container using the <see cref="IServiceCollection" />
        /// </summary>
        /// <param name="containerRegistry">The <see cref="IContainerRegistry" /></param>
        /// <param name="registerServices">The <see cref="Action" /> to perform on the <see cref="IServiceCollection" /></param>
        /// <returns></returns>
        public static IContainerRegistry RegisterServices(this IContainerRegistry containerRegistry, Action<IServiceCollection> registerServices)
        {
            if (containerRegistry is IServiceCollectionAware sca)
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
