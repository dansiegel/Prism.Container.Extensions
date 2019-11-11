using System;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Prism.Ioc;

[assembly: InternalsVisibleTo("Prism.Microsoft.DependencyInjection.Extensions.Tests")]
[assembly: InternalsVisibleTo("Prism.Microsoft.DependencyInjection.Forms.Extended.Tests")]
[assembly: InternalsVisibleTo("Shiny.Prism.Tests")]
namespace Prism.Microsoft.DependencyInjection
{
    public static class MicrsoftDependencyInjectionExtensions
    {
        public static IServiceCollection ServiceCollection(this IContainerRegistry containerRegistry)
        {
            if(containerRegistry is PrismContainerExtension pce)
            {
                return pce.Services;
            }

            throw new NotImplementedException("IContainerRegistry must be implmented from the concrete type PrismContainerExtension");
        }
    }
}
