using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Prism.Microsoft.DependencyInjection
{
    public class ConcreteAwareServiceProvider : IServiceProvider
    {
        public ConcreteAwareServiceProvider(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public IServiceProvider ServiceProvider { get; }

        public object GetService(Type serviceType) =>
            ServiceProvider.GetService(serviceType) ?? GetConcreteImplementation(serviceType);

        private object GetConcreteImplementation(Type serviceType)
        {
            if (serviceType.IsInterface || serviceType.IsAbstract) return null;

            if (serviceType.IsClass)
            {
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
    }
}
