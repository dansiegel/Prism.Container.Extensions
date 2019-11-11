using System;
using Microsoft.Extensions.DependencyInjection;

namespace Prism.Microsoft.DependencyInjection
{
    public class ConcreteAwareServiceScope : IServiceScope
    {
        private IServiceScope _serviceScope;

        public ConcreteAwareServiceScope(IServiceScope serviceScope)
        {
            _serviceScope = serviceScope;
        }

        public IServiceProvider ServiceProvider => _serviceScope.ServiceProvider;

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _serviceScope.Dispose();
                    _serviceScope = null;
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
