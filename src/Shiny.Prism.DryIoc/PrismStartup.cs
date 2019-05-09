using Microsoft.Extensions.DependencyInjection;
using Prism.DryIoc;
using Prism.Ioc;
using System;

namespace Shiny.Prism.DryIoc
{
    public abstract class PrismStartup : Startup
    {
        public override sealed IServiceProvider CreateServiceProvider(IServiceCollection services)
        {
            return CreateContainerExtension().CreateServiceProvider(services);
        }

        protected virtual IContainerExtension CreateContainerExtension() => PrismContainerExtension.Current;
    }
}
