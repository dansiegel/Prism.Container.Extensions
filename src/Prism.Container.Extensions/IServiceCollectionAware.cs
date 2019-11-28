using System;
using Microsoft.Extensions.DependencyInjection;
using Prism.Ioc;

namespace Prism.Container.Extensions
{
    public interface IServiceCollectionAware
    {
        IContainerRegistry RegisterServices(Action<IServiceCollection> registerServices);
    }
}
