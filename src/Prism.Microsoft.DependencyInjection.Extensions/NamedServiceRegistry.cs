using System;
using System.Collections.Generic;
using System.Linq;

namespace Prism.Microsoft.DependencyInjection
{
    internal class NamedServiceRegistry
    {
        List<NamedService> Services { get; } = new List<NamedService>();

        public void Add(string name, Type serviceType, Type implementingType)
        {
            CheckDuplicate(name, serviceType);

            Services.Add(new NamedService
            {
                ImplementationType = implementingType,
                Name = name,
                ServiceType = serviceType
            });
        }

        public void AddSingleton(string name, Type serviceType, Type implementingType)
        {
            CheckDuplicate(name, serviceType);

            Services.Add(new NamedSingletonService
            {
                ImplementationType = implementingType,
                Name = name,
                ServiceType = serviceType
            });
        }

        public void AddSingleton(string name, Type serviceType, object instance)
        {
            CheckDuplicate(name, serviceType);

            Services.Add(new NamedSingletonService
            {
                Instance = instance,
                Name = name,
                ServiceType = serviceType
            });
        }

        public bool IsRegistered(Type type, string name) =>
            Services.Any(x => x.Name == name && x.ServiceType == type);

        public object GetService(IServiceProvider serviceProvider, string name, Type type)
        {
            var registry = GetServiceRegistry(name, type);
            return registry?.GetService(serviceProvider);
        }

        public Type GetRegistrationType(string key)
        {
            return Services.FirstOrDefault(x => x.Name.Equals(key, StringComparison.OrdinalIgnoreCase) || x.ImplementationType.Name.Equals(key, StringComparison.OrdinalIgnoreCase))?.ImplementationType;
        }

        private void CheckDuplicate(string name, Type serviceType)
        {
            var registry = GetServiceRegistry(name, serviceType);
            if (registry != null)
            {
                Console.WriteLine($"'{serviceType.FullName}' with the name '{name}' has already been registered and will be replaced.");
                Services.Remove(registry);
            }
        }

        private NamedService GetServiceRegistry(string name, Type type) =>
            Services.FirstOrDefault(x => x.Name == name && x.ServiceType == type);

        class NamedService
        {
            public string Name { get; set; }
            public Type ServiceType { get; set; }
            public Type ImplementationType { get; set; }

            public virtual object GetService(IServiceProvider serviceProvider) =>
                serviceProvider.GetOrConstructService(ImplementationType);
        }

        class NamedSingletonService : NamedService
        {
            public object Instance { get; set; }

            public override object GetService(IServiceProvider serviceProvider)
            {
                if (Instance is null)
                {
                    Instance = serviceProvider.GetOrConstructService(ImplementationType);
                }

                return Instance;
            }
        }
    }
}
