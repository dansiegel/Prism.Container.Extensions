using System;

namespace Prism.Ioc
{
    internal class PrismServiceProvider : IServiceProvider
    {
        private IContainerProvider _container { get; }

        public PrismServiceProvider(IContainerRegistry container)
            : this(container as IContainerProvider) { }

        public PrismServiceProvider(IContainerProvider container)
        {
            _container = container;
        }

        object IServiceProvider.GetService(Type serviceType) => _container.Resolve(serviceType);
    }
}
