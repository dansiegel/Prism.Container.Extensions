using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Prism.Container.Extensions;
using Prism.Ioc;
using Prism.Microsoft.DependencyInjection;

[assembly: InternalsVisibleTo("Prism.Microsoft.DependencyInjection.Extensions.Tests")]
[assembly: InternalsVisibleTo("Prism.Microsoft.DependencyInjection.Forms.Extended.Tests")]
[assembly: InternalsVisibleTo("Shiny.Prism.Tests")]
namespace Prism.Microsoft.DependencyInjection
{
    public class PrismContainerExtension : IContainerExtension<IServiceProvider>, IExtendedContainerRegistry, IScopeProvider
    {
        private static IContainerExtension<IServiceProvider> _current;
        public static IContainerExtension<IServiceProvider> Current
        {
            get
            {
                if (_current is null)
                {
                    Create();
                }

                return _current;
            }
        }

        internal static void Reset()
        {
            _current = null;
            GC.Collect();
        }

        public static IContainerExtension Create() =>
            Create(new ServiceCollection());

        public static IContainerExtension Create(IServiceCollection services)
        {
            if (_current != null)
            {
                throw new NotSupportedException($"An instance of {nameof(PrismContainerExtension)} has already been created.");
            }

            return new PrismContainerExtension(services);
        }

        private IServiceScope _serviceScope;
        private bool requiresRebuild;
        private NamedServiceRegistry NamedServiceRegistry { get; }
        public IServiceCollection Services { get; private set; }

        private IServiceProvider _instance;
        public IServiceProvider Instance
        {
            get
            {
                bool createScope = false;
                if(requiresRebuild || _instance is null)
                {
                    if (_serviceScope != null)
                    {
                        createScope = true;
                        _serviceScope.Dispose();
                    }

                    _instance = new ConcreteAwareServiceProvider(Services.BuildServiceProvider());
                    requiresRebuild = false;

                    if(createScope)
                    {
                        _serviceScope = new ConcreteAwareServiceScope(_instance.CreateScope());
                    }
                }

                return _serviceScope?.ServiceProvider ?? _instance;
            }
        }

        public PrismContainerExtension()
            : this(new ServiceCollection())
        {

        }

        public PrismContainerExtension(IServiceCollection services)
        {
            _current = this;
            Services = services;
            NamedServiceRegistry = new NamedServiceRegistry();

            Services.AddSingleton<PrismContainerExtension>(this);
            Services.AddSingleton<IContainerRegistry>(sp => sp.GetService<PrismContainerExtension>());
            Services.AddSingleton<IContainerExtension>(sp => sp.GetService<PrismContainerExtension>());
            Services.AddSingleton<IContainerProvider>(sp => sp.GetService<PrismContainerExtension>());
            Services.AddSingleton<IServiceProvider>(sp => sp.GetService<PrismContainerExtension>());
        }

        public void SetServiceCollection(IServiceCollection services)
        {
            if(Services.Any())
            {
                foreach(var service in Services)
                {
                    services.Add(service);
                }
            }

            Services = services;
        }

        public void FinalizeExtension() { }

        public object Resolve(Type type) =>
            Instance.GetService(type) ?? throw NullResolutionException(type);

        public object Resolve(Type type, params (Type Type, object Instance)[] parameters)
        {
            var childProvider = GetChildProvider(parameters);
            return childProvider.GetService(type) ?? throw NullResolutionException(type);
        }

        public object Resolve(Type type, string name)
        {
            return NamedServiceRegistry.GetService(this, name, type) ?? throw NullResolutionException(type, name);
        }

        public object Resolve(Type type, string name, params (Type Type, object Instance)[] parameters)
        {
            var childProvider = GetChildProvider(parameters);
            return NamedServiceRegistry.GetService(childProvider, name, type) ?? throw NullResolutionException(type, name);
        }

        private Exception NullResolutionException(Type type, string name = null)
        {
            return new Exception(string.IsNullOrEmpty(name) ? $"There was a problem while attempting create an instance of {type.FullName}" : $"Unable to create an instance of '{type.FullName}' with the service name '{name}'.");
        }

        public IContainerRegistry RegisterInstance(Type type, object instance)
        {
            requiresRebuild = true;
            Services.AddSingleton(type, instance);
            return this;
        }

        public IContainerRegistry RegisterInstance(Type type, object instance, string name)
        {
            requiresRebuild = true;
            NamedServiceRegistry.AddSingleton(name, type, instance);
            return this;
        }

        public IContainerRegistry RegisterSingleton(Type from, Type to)
        {
            requiresRebuild = true;
            Services.AddSingleton(from, to);
            return this;
        }

        public IContainerRegistry RegisterSingleton(Type from, Type to, string name)
        {
            requiresRebuild = true;
            NamedServiceRegistry.AddSingleton(name, from, to);
            return this;
        }

        public IContainerRegistry Register(Type from, Type to)
        {
            requiresRebuild = true;
            Services.AddTransient(from, to);
            return this;
        }

        public IContainerRegistry Register(Type from, Type to, string name)
        {
            requiresRebuild = true;
            NamedServiceRegistry.Add(name, from, to);
            return this;
        }

        public bool IsRegistered(Type type) =>
            Services.Any(x => x.ServiceType == type);

        public bool IsRegistered(Type type, string name) =>
            NamedServiceRegistry.IsRegistered(type, name);

        public IContainerRegistry RegisterMany(Type implementingType, params Type[] serviceTypes)
        {
            requiresRebuild = true;
            if(serviceTypes is null || serviceTypes.Count() == 0)
            {
                serviceTypes = implementingType.GetInterfaces();
            }

            Services.AddTransient(implementingType);
            foreach (var type in serviceTypes)
            {
                Services.AddTransient(type, sp => sp.GetService(implementingType));
            }
            return this;
        }

        public IContainerRegistry RegisterManySingleton(Type implementingType, params Type[] serviceTypes)
        {
            requiresRebuild = true;
            Services.AddSingleton(implementingType);
            if (serviceTypes is null || serviceTypes.Count() == 0)
            {
                serviceTypes = implementingType.GetInterfaces();
            }

            foreach (var type in serviceTypes)
            {
                Services.AddTransient(type, sp => sp.GetService(implementingType));
            }
            return this;
        }

        public IContainerRegistry RegisterDelegate(Type serviceType, Func<object> factoryMethod)
        {
            requiresRebuild = true;
            Services.AddTransient(serviceType, _ => factoryMethod());
            return this;
        }

        public IContainerRegistry RegisterDelegate(Type serviceType, Func<IContainerProvider, object> factoryMethod)
        {
            requiresRebuild = true;
            Services.AddTransient(serviceType, sp => factoryMethod(sp.GetService<IContainerProvider>()));
            return this;
        }

        public IContainerRegistry RegisterDelegate(Type serviceType, Func<IServiceProvider, object> factoryMethod)
        {
            requiresRebuild = true;
            Services.AddTransient(serviceType, factoryMethod);
            return this;
        }

        public IContainerRegistry RegisterSingletonFromDelegate(Type serviceType, Func<object> factoryMethod)
        {
            requiresRebuild = true;
            Services.AddSingleton(serviceType, _ => factoryMethod());
            return this;
        }

        public IContainerRegistry RegisterSingletonFromDelegate(Type serviceType, Func<IContainerProvider, object> factoryMethod)
        {
            requiresRebuild = true;
            Services.AddSingleton(serviceType, sp => factoryMethod(sp.GetService<IContainerProvider>()));
            return this;
        }

        public IContainerRegistry RegisterSingletonFromDelegate(Type serviceType, Func<IServiceProvider, object> factoryMethod)
        {
            requiresRebuild = true;
            Services.AddSingleton(serviceType, factoryMethod);
            return this;
        }

        public IContainerRegistry RegisterScoped(Type serviceType)
        {
            requiresRebuild = true;
            Services.AddScoped(serviceType);
            return this;
        }

        public IContainerRegistry RegisterScoped(Type serviceType, Type implementationType)
        {
            requiresRebuild = true;
            Services.AddScoped(serviceType, implementationType);
            return this;
        }

        public object GetService(Type serviceType) =>
            Instance.GetService(serviceType);

        public void CreateScope()
        {
            _serviceScope?.Dispose();
            _serviceScope = new ConcreteAwareServiceScope(Instance.CreateScope());
        }

        private IServiceProvider GetChildProvider((Type Type, object Instance)[] parameters)
        {
            if (parameters is null || parameters.Length == 0)
                return Instance;

            var services = new ServiceCollection();
            foreach (var service in Services)
            {
                services.Add(service);
            }

            foreach (var param in parameters)
            {
                services.AddSingleton(param.Type, param.Instance);
            }

            var rootSP = services.BuildServiceProvider();

            return new ConcreteAwareServiceProvider(rootSP);
        }
    }
}
