using Microsoft.Extensions.DependencyInjection;
using Prism.DryIoc;
using Prism.Ioc;
using System;

namespace Shiny.Prism.DryIoc
{
    public abstract class PrismStartup : IStartup
    {
        public abstract void ConfigureServices(IServiceCollection services);

        void IStartup.ConfigureApp(IServiceProvider provider) { }

        IServiceProvider IStartup.CreateServiceProvider(IServiceCollection services)
        {
            return CreateContainerExtension().CreateServiceProvider(services);
        }

        protected virtual IContainerExtension CreateContainerExtension() => PrismContainerExtension.Current;
    }
}
