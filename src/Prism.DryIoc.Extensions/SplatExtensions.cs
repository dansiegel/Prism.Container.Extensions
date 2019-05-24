using DryIoc;
using Splat;
using System;
using System.Collections.Generic;

namespace Prism.DryIoc
{
    public sealed partial class PrismContainerExtension : IDependencyResolver
    {
        object IReadonlyDependencyResolver.GetService(Type serviceType, string contract = null) =>
            string.IsNullOrEmpty(contract)
                ? Instance.Resolve(serviceType, IfUnresolved.ReturnDefault)
                : Instance.Resolve(serviceType, contract, IfUnresolved.ReturnDefault);

        IEnumerable<object> IReadonlyDependencyResolver.GetServices(Type serviceType, string contract = null) =>
            string.IsNullOrEmpty(contract)
                ? Instance.ResolveMany(serviceType)
                : Instance.ResolveMany(serviceType, serviceKey: contract);

        void IMutableDependencyResolver.Register(Func<object> factory, Type serviceType, string contract = null)
        {
            if (string.IsNullOrEmpty(contract))
            {
                Instance.UseInstance(serviceType, factory(), IfAlreadyRegistered.AppendNewImplementation);
            }
            else
            {
                Instance.UseInstance(serviceType, factory(), IfAlreadyRegistered.AppendNewImplementation, serviceKey: contract);
            }
        }

        void IMutableDependencyResolver.UnregisterCurrent(Type serviceType, string contract = null)
        {
            if (string.IsNullOrEmpty(contract))
            {
                Instance.Unregister(serviceType);
            }
            else
            {
                Instance.Unregister(serviceType, contract);
            }
        }

        void IMutableDependencyResolver.UnregisterAll(Type serviceType, string contract = null)
        {
            if (string.IsNullOrEmpty(contract))
            {
                Instance.Unregister(serviceType);
            }
            else
            {
                Instance.Unregister(serviceType, contract);
            }
        }

        IDisposable IMutableDependencyResolver.ServiceRegistrationCallback(Type serviceType, string contract, Action<IDisposable> callback)
        {
            // this method is not used by RxUI
            throw new NotImplementedException();
        }

        public bool IsDisposed { get; private set; }

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            IsDisposed = true;
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                Instance?.Dispose();
                Instance = null;
            }
        }
    }
}
