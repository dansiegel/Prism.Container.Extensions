using System;
using Microsoft.Extensions.DependencyInjection;

namespace Prism.Microsoft.DependencyInjection
{
    public class ConcreteAwareServiceScope : IServiceScope
    {
        public ConcreteAwareServiceScope(IServiceScope serviceScope)
        {
            ServiceProvider = new ConcreteAwareServiceProvider(serviceScope.ServiceProvider);
        }

        public IServiceProvider ServiceProvider { get; }

        #region IDisposable Support


        protected virtual void Dispose(bool disposing)
        {

        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
