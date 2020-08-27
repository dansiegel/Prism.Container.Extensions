using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Prism.Microsoft.DependencyInjection
{
    public class ConcreteAwareServiceProvider : IServiceProvider
    {
        private bool _isScoped { get; }

        public ConcreteAwareServiceProvider(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public ConcreteAwareServiceProvider(IServiceScope serviceScope)
        {
            ServiceProvider = serviceScope.ServiceProvider;
            _isScoped = true;
        }

        public IServiceProvider ServiceProvider { get; }

        public object GetService(Type serviceType) =>
            ServiceProvider.GetService(serviceType) ?? GetConcreteImplementation(serviceType);

        private object GetConcreteImplementation(Type serviceType)
        {
            if (serviceType.IsInterface || serviceType.IsAbstract) return null;

            if (serviceType.IsClass)
            {
                if (_isScoped)
                    BuildConcreteImplementation(serviceType);

                PrismContainerExtension.Current.Register(serviceType, serviceType);
                var sp = PrismContainerExtension.Current.ServiceCollection().BuildServiceProvider();
                return sp.GetService(serviceType);
            }

            if(serviceType.IsValueType)
            {
                return Activator.CreateInstance(serviceType);
            }

            return null;
        }

        private object BuildConcreteImplementation(Type serviceType)
        {
            var constructors = serviceType.GetConstructors();

            if (!constructors.Any())
                return Activator.CreateInstance(serviceType);

            var ctor = constructors.OrderByDescending(x => x.GetParameters().Length).First();

            var parameters = ctor.GetParameters().Select(p => GetService(p.ParameterType)).ToArray();
            return ctor.Invoke(parameters);
        }
    }
}
