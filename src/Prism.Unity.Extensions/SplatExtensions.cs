using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity;

namespace Prism.Unity.Extensions
{
    public partial class PrismContainerExtension : IDependencyResolver
    {
        object IReadonlyDependencyResolver.GetService(Type serviceType, string contract = null) =>
            string.IsNullOrEmpty(contract)
                ? Instance.Resolve(serviceType)
                : Instance.Resolve(serviceType, contract);

        IEnumerable<object> IReadonlyDependencyResolver.GetServices(Type serviceType, string contract = null) =>
            string.IsNullOrEmpty(contract)
                ? Instance.ResolveAll(serviceType)
                : Instance.ResolveAll(serviceType/*, contract*/);

        void IMutableDependencyResolver.Register(Func<object> factory, Type serviceType, string contract = null)
        {
            if (string.IsNullOrEmpty(contract))
            {
                Instance.RegisterInstance(serviceType, factory());
            }
            else
            {
                Instance.RegisterInstance(serviceType, contract, factory());
            }
        }

        void IMutableDependencyResolver.UnregisterCurrent(Type serviceType, string contract = null)
        {
            var registration = Instance.Registrations.FirstOrDefault(p => p.RegisteredType == serviceType && p.Name == contract);
            registration.LifetimeManager.RemoveValue();
        }

        void IMutableDependencyResolver.UnregisterAll(Type serviceType, string contract = null)
        {
            var registrations = Instance.Registrations.Where(p => p.RegisteredType == serviceType && p.Name == contract);

            foreach(var r in registrations)
            {
                r.LifetimeManager.RemoveValue();
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
