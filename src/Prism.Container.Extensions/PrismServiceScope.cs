using System;
using Microsoft.Extensions.DependencyInjection;

namespace Prism.Ioc
{
    public class PrismServiceScope : IServiceScope
    {
        private IScopedProvider _scopedProvider;

        public PrismServiceScope(IScopedProvider scopedProvider)
        {
            _scopedProvider = scopedProvider;
            ServiceProvider = new PrismServiceProvider(scopedProvider.CurrentScope);
        }

        public IServiceProvider ServiceProvider { get; }

        public void Dispose()
        {
            if (_scopedProvider != null)
            {
                _scopedProvider.Dispose();
                _scopedProvider = null;
            }
        }
    }
}
