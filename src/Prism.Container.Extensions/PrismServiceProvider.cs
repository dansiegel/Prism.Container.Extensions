using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Prism.Ioc;
using Prism.Ioc.Internals;

namespace Prism.Container.Extensions
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class PrismServiceProvider : IServiceProvider
    {
        private IContainerExtension _container { get; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public PrismServiceProvider(IContainerExtension container)
        {
            _container = container;
        }

        [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "The point of this method is to suppress any exceptions")]
        object IServiceProvider.GetService(Type serviceType)
        {
            try
            {
                return _container.Resolve(serviceType);
            }
            catch (Exception)
            {
                return TryBuildInstance(serviceType);
            }
        }

        [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "The point of this method is to suppress any exceptions")]
        private object TryBuildInstance(Type serviceType)
        {
            try
            {
                var instanceType = _container.GetRegistrationType(serviceType);
                var ctor = instanceType.GetConstructors().OrderByDescending(x => x.GetParameters().Length).FirstOrDefault();
                var parameters = ctor.GetParameters().Select(x => TryResolve(x.ParameterType)).ToArray();
                return ctor.Invoke(parameters);
            }
            catch
            {
                return null;
            }
        }

        [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "The point of this method is to suppress any exceptions")]
        private object TryResolve(Type serviceType)
        {
            try
            {
                if(_container.IsRegistered(serviceType))
                {
                    return _container.Resolve(serviceType);
                }
            }
            catch
            {
            }

            return null;
        }
    }
}
