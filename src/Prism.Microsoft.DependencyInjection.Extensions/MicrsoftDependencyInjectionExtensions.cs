using System;
using System.Linq;
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
            if (containerRegistry is PrismContainerExtension pce)
            {
                return pce.Services;
            }

            throw new NotImplementedException("IContainerRegistry must be implmented from the concrete type PrismContainerExtension");
        }

        internal static object GetOrConstructService(this IServiceProvider provider, Type type, params (Type Type, object Instance)[] parameters)
        {
            var instance = provider.GetService(type);
            if (instance is null && !type.IsInterface && !type.IsAbstract)
            {
                var ctor = type.GetConstructors().OrderByDescending(x => x.GetParameters().Length).FirstOrDefault();
                if (ctor is null)
                    throw new NullReferenceException($"Could not locate a public constructor for {type.FullName}");

                var ctorParameters = ctor.GetParameters();
                var args = ctor.GetParameters().Select(x =>
                {
                    object arg = parameters.FirstOrDefault(p => x.ParameterType.IsAssignableFrom(p.Instance.GetType())).Instance;
                    if (arg != null)
                        return arg;

                    return provider.GetService(x.ParameterType);
                });
                return ctor.Invoke(args.ToArray());
            }
            return instance;
        }
    }
}
