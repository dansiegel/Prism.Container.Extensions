using System;
using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Prism.Ioc;

namespace Prism.Container.Extensions
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IServiceCollectionAware
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        IContainerRegistry RegisterServices(Action<IServiceCollection> registerServices);
    }
}
