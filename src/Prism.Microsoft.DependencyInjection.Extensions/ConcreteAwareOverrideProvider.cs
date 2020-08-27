using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Prism.Microsoft.DependencyInjection.Extensions
{
    internal class ConcreteAwareOverrideProvider : IServiceProvider
    {
        private IServiceProvider _rootProvider { get; }
        private IServiceCollection _services { get; }
        private (Type type, object instance)[] _overrides { get; }

        public ConcreteAwareOverrideProvider(IServiceProvider serviceProvider, IServiceCollection services, (Type type, object instance)[] overrides)
        {
            _rootProvider = serviceProvider;
            _overrides = overrides;
        }

        public object GetService(Type serviceType)
        {
            if (!serviceType.IsAbstract && serviceType.IsClass && serviceType != typeof(object))
            {
                return BuildInstance(serviceType);
            }

            var serviceDescriptor = _services.LastOrDefault(x => x.ServiceType == serviceType);

            if (serviceDescriptor?.ImplementationType is null)
                return _rootProvider.GetService(serviceType);

            var implType = serviceDescriptor.ImplementationType;
            return BuildInstance(implType);
        }

        private object BuildInstance(Type implType)
        {
            var constructors = implType.GetConstructors();

            if (constructors is null || !constructors.Any())
                return Activator.CreateInstance(implType);

            var ctor = constructors.OrderByDescending(x => x.GetParameters().Length).First();
            var parameters = ctor.GetParameters().Select(x =>
            {
                (var t, var instance) = _overrides.FirstOrDefault(o => x.ParameterType == o.type);
                if (t != null)
                {
                    return instance;
                }

                return _rootProvider.GetService(x.ParameterType);
            }).ToArray();

            return ctor.Invoke(parameters);
        }
    }
}
