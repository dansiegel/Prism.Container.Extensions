using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Prism.Container.Extensions;
using Prism.Ioc;
using Prism.Ioc.Internals;
using Prism.Microsoft.DependencyInjection.Extensions;

[assembly: InternalsVisibleTo("Prism.Microsoft.DependencyInjection.Extensions.Tests")]
[assembly: InternalsVisibleTo("Prism.Microsoft.DependencyInjection.Forms.Extended.Tests")]
[assembly: InternalsVisibleTo("Shiny.Prism.Tests")]
namespace Prism.Microsoft.DependencyInjection
{
    public class PrismContainerExtension : IContainerExtension<IServiceProvider>, IServiceCollectionAware, IServiceProvider, IContainerInfo
    {
        public static IContainerExtension Current
        {
            get
            {
                var container = TryGetContainer();
                if (container != null)
                    return container;

                Init();
                return ContainerLocator.Current;
            }
        }

        public IScopedProvider CurrentScope { get; private set; }

        internal static void Reset()
        {
            ContainerLocator.ResetContainer();
            GC.Collect();
        }

        public static IContainerExtension Init() =>
            Init(new ServiceCollection());

        public static IContainerExtension Init(IServiceCollection services)
        {
            if (TryGetContainer() != null)
                throw new NotSupportedException("The PrismContainerExtension has already been initialized.");

            var extension = new PrismContainerExtension(services);
            ContainerLocator.SetContainerExtension(() => extension);
            return extension;
        }

        private static IContainerExtension TryGetContainer()
        {
            try
            {
                return ContainerLocator.Current;
            }
            catch
            {
                return null;
            }
        }

        private Dictionary<string, Type> _typeFactories { get; }
        private Func<Type, Type> _defaultViewTypeToViewModelTypeResolver { get; }

        private IServiceScope _serviceScope;
        private bool requiresRebuild;
        private NamedServiceRegistry NamedServiceRegistry { get; }
        public IServiceCollection Services { get; private set; }


        private IServiceScope _parentScope = null;
        private IServiceProvider _instance;
        public IServiceProvider Instance
        {
            get
            {
                bool createScope = false;
                if (requiresRebuild || _instance is null)
                {
                    if (_serviceScope != null)
                    {
                        createScope = true;
                        _parentScope?.Dispose();
                    }
                    _instance = new ConcreteAwareServiceProvider(Services.BuildServiceProvider());
                    requiresRebuild = false;

                    if (createScope)
                    {
                        _parentScope = _instance.CreateScope();
                        _serviceScope = new ConcreteAwareServiceScope(_parentScope);
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
            if (Services.Any())
            {
                foreach (var service in Services)
                {
                    services.Add(service);
                }
            }

            Services = services;
        }

        public void FinalizeExtension() { }

        public object Resolve(Type type) =>
            Resolve(type, Array.Empty<(Type, object)>());

        public object Resolve(Type type, string name) =>
            Resolve(type, name, Array.Empty<(Type, object)>());

        public object Resolve(Type type, params (Type Type, object Instance)[] parameters)
        {
            try
            {
                var provider = parameters.Length > 0 ? GetChildProvider(parameters) : Instance;
                return provider.GetService(type) ?? throw NullResolutionException(type);
            }
            catch (Exception ex)
            {
                throw new ContainerResolutionException(type, ex);
            }
        }

        public object Resolve(Type type, string name, params (Type Type, object Instance)[] parameters)
        {
            try
            {
                var provider = parameters.Length > 0 ? GetChildProvider(parameters) : Instance;
                return NamedServiceRegistry.GetService(provider, name, type) ?? throw NullResolutionException(type, name);
            }
            catch (Exception ex)
            {
                throw new ContainerResolutionException(type, name, ex);
            }
        }

        private static Exception NullResolutionException(Type type, string name = null)
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
            Services.AddTransient(to);
            NamedServiceRegistry.Add(name, from, to);

            if (from == typeof(object) && to.Namespace.Contains("Views"))
            {

            }

            return this;
        }

        public bool IsRegistered(Type type) =>
            Services.Any(x => x.ServiceType == type);

        public bool IsRegistered(Type type, string name) =>
            NamedServiceRegistry.IsRegistered(type, name);

        public IContainerRegistry RegisterServices(Action<IServiceCollection> registerServices)
        {
            requiresRebuild = true;
            registerServices(Services);
            return this;
        }

        public IContainerRegistry RegisterMany(Type implementingType, params Type[] serviceTypes)
        {
            requiresRebuild = true;
            if (serviceTypes is null || serviceTypes.Count() == 0)
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

        public IContainerRegistry Register(Type serviceType, Func<object> factoryMethod)
        {
            requiresRebuild = true;
            Services.AddTransient(serviceType, _ => factoryMethod());
            return this;
        }

        public IContainerRegistry Register(Type serviceType, Func<IContainerProvider, object> factoryMethod)
        {
            requiresRebuild = true;
            Services.AddTransient(serviceType, sp => factoryMethod(sp.GetService<IContainerProvider>()));
            return this;
        }

        public IContainerRegistry Register(Type serviceType, Func<IServiceProvider, object> factoryMethod)
        {
            requiresRebuild = true;
            Services.AddTransient(serviceType, factoryMethod);
            return this;
        }

        public IContainerRegistry RegisterSingleton(Type serviceType, Func<object> factoryMethod)
        {
            requiresRebuild = true;
            Services.AddSingleton(serviceType, _ => factoryMethod());
            return this;
        }

        public IContainerRegistry RegisterSingleton(Type serviceType, Func<IContainerProvider, object> factoryMethod)
        {
            requiresRebuild = true;
            Services.AddSingleton(serviceType, sp => factoryMethod(sp.GetService<IContainerProvider>()));
            return this;
        }

        public IContainerRegistry RegisterSingleton(Type serviceType, Func<IServiceProvider, object> factoryMethod)
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

        public IContainerRegistry RegisterScoped(Type serviceType, Func<object> factoryMethod)
        {
            requiresRebuild = true;
            Services.AddScoped(serviceType, s => factoryMethod());
            return this;
        }

        public IContainerRegistry RegisterScoped(Type serviceType, Func<IContainerProvider, object> factoryMethod)
        {
            requiresRebuild = true;
            Services.AddScoped(serviceType, s => factoryMethod(s.GetService<IContainerProvider>()));
            return this;
        }

        public IContainerRegistry RegisterScoped(Type serviceType, Func<IServiceProvider, object> factoryMethod)
        {
            requiresRebuild = true;
            Services.AddScoped(serviceType, factoryMethod);
            return this;
        }

        public object GetService(Type serviceType) =>
            Instance.GetService(serviceType);

        public IScopedProvider CreateScope()
        {

            _parentScope = Instance.CreateScope();
            _serviceScope = new ConcreteAwareServiceScope(_parentScope);
            return new ScopedProvider(_serviceScope, NamedServiceRegistry, Services);
        }

        // TODO: Refactor to share this with the child scope
        private IServiceProvider GetChildProvider((Type Type, object Instance)[] parameters)
        {
            if (parameters is null || parameters.Length == 0)
                return Instance;

            var services = new ServiceCollection();
            foreach (var service in Services)
            {
                if (parameters.Any(x => x.Type == service.ServiceType))
                    continue;

                services.Add(service);
            }

            foreach (var param in parameters)
            {
                services.AddSingleton(param.Type, param.Instance);
            }

            var rootSP = services.BuildServiceProvider();

            if (_serviceScope is null)
            {
                return new ConcreteAwareServiceProvider(rootSP);
            }

            return new ConcreteAwareOverrideProvider(Instance, Services, parameters);
        }

        Type IContainerInfo.GetRegistrationType(string key)
        {
            var matchingRegistration = NamedServiceRegistry.GetRegistrationType(key);
            if (matchingRegistration != null)
                return matchingRegistration;

            return Services.FirstOrDefault(r => key.Equals(r.ImplementationType.Name, StringComparison.Ordinal))?.ImplementationType;
        }

        Type IContainerInfo.GetRegistrationType(Type serviceType)
        {
            return Services.FirstOrDefault(x => x.ServiceType == serviceType)?.ImplementationType;
        }

        private class ScopedProvider : IScopedProvider
        {
            private IServiceScope _serviceScope;
            private NamedServiceRegistry _namedServiceRegistry;
            private IServiceCollection _services;

            public ScopedProvider(IServiceScope serviceScope, NamedServiceRegistry namedServiceRegistry, IServiceCollection services)
            {
                _serviceScope = serviceScope;
                _namedServiceRegistry = namedServiceRegistry;
                _services = services;
            }

            public bool IsAttached { get; set; }

            public IScopedProvider CurrentScope { get; }

            public IScopedProvider CreateScope()
            {
                throw new NotImplementedException();
            }

            public void Dispose()
            {
                if (_serviceScope != null)
                {
                    _serviceScope.Dispose();
                    _serviceScope = null;
                }
            }


            public object Resolve(Type type) =>
            Resolve(type, Array.Empty<(Type, object)>());

            public object Resolve(Type type, string name) =>
                Resolve(type, name, Array.Empty<(Type, object)>());

            public object Resolve(Type type, params (Type Type, object Instance)[] parameters)
            {
                try
                {
                    var provider = parameters.Length > 0 ? GetChildProvider(parameters) : _serviceScope.ServiceProvider;
                    return provider.GetService(type) ?? throw NullResolutionException(type);
                }
                catch (Exception ex)
                {
                    throw new ContainerResolutionException(type, ex);
                }
            }

            public object Resolve(Type type, string name, params (Type Type, object Instance)[] parameters)
            {
                try
                {
                    var provider = parameters.Length > 0 ? GetChildProvider(parameters) : _serviceScope.ServiceProvider;
                    return _namedServiceRegistry.GetService(provider, name, type) ?? throw NullResolutionException(type, name);
                }
                catch (Exception ex)
                {
                    throw new ContainerResolutionException(type, name, ex);
                }
            }

            private static Exception NullResolutionException(Type type, string name = null)
            {
                return new Exception(string.IsNullOrEmpty(name) ? $"There was a problem while attempting create an instance of {type.FullName}" : $"Unable to create an instance of '{type.FullName}' with the service name '{name}'.");
            }

            private IServiceProvider GetChildProvider((Type Type, object Instance)[] parameters)
            {
                if (parameters is null || parameters.Length == 0)
                    return _serviceScope.ServiceProvider;

                var services = new ServiceCollection();
                foreach (var service in _services)
                {
                    if (parameters.Any(x => x.Type == service.ServiceType))
                        continue;

                    services.Add(service);
                }

                foreach (var param in parameters)
                {
                    services.AddSingleton(param.Type, param.Instance);
                }

                var rootSP = services.BuildServiceProvider();

                if (_serviceScope is null)
                {
                    return new ConcreteAwareServiceProvider(rootSP);
                }

                throw new NotSupportedException("We do not currently support using a child container within a ServiceScope");
            }
        }
    }
}
