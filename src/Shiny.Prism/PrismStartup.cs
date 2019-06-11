using Microsoft.Extensions.DependencyInjection;
using Prism.Ioc;
using System;

namespace Shiny.Prism
{
    public abstract class PrismStartup : IStartup
    {
        private IContainerExtension _container;

        protected PrismStartup()
        {
        }

        protected PrismStartup(IContainerExtension container)
        {
            _container = container;
        }

        public abstract void ConfigureServices(IServiceCollection services);

        void IStartup.ConfigureApp(IServiceProvider provider) { }

        IServiceProvider IStartup.CreateServiceProvider(IServiceCollection services)
        {
            return CreateContainerExtension().CreateServiceProvider(services);
        }

        protected virtual IContainerExtension CreateContainerExtension() => _container;

        public IStartup WithContainer(IContainerExtension container)
        {
            _container = container;
            return this;
        }
    }
}
