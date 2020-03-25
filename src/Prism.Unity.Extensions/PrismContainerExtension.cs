using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Prism.Container.Extensions;
using Prism.Ioc;
using Prism.Unity;
using Unity;
using Unity.Lifetime;
using Unity.Resolution;

[assembly: Prism.Unity.Preserve]
[assembly: ContainerExtension(typeof(PrismContainerExtension))]
[assembly: InternalsVisibleTo("Prism.Unity.Extensions.Tests")]
[assembly: InternalsVisibleTo("Prism.Unity.Forms.Extended.Tests")]
namespace Prism.Unity
{
    public partial class PrismContainerExtension : IContainerExtension<IUnityContainer>, IExtendedContainerRegistry, IScopeProvider, IScopedFactoryRegistry, IServiceScopeFactory
    {
        private static IContainerExtension<IUnityContainer> _current;
        public static IContainerExtension<IUnityContainer> Current
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
            Create(new UnityContainer());

        public static IContainerExtension Create(IUnityContainer container)
        {
            if (_current != null)
            {
                throw new NotSupportedException($"An instance of {nameof(PrismContainerExtension)} has already been created.");
            }

            return new PrismContainerExtension(container);
        }

        private PrismContainerExtension()
           : this(new UnityContainer())
        {
        }

        private PrismContainerExtension(IUnityContainer container)
        {
            _current = this;
            Instance = container;
            Instance.RegisterInstance<IContainerProvider>(this);
            Instance.RegisterInstance<IContainerExtension>(this);
            Instance.RegisterInstance<IContainerRegistry>(this);
            Instance.RegisterInstance<IServiceProvider>(this);
            Instance.RegisterInstance<IServiceScopeFactory>(this);
        }

        private ServiceScope _currentScope;

        public IUnityContainer Instance { get; private set; }

        public void FinalizeExtension() { }

        public IContainerRegistry RegisterInstance(Type type, object instance)
        {
            Instance.RegisterInstance(type, instance);
            return this;
        }

        public IContainerRegistry RegisterInstance(Type type, object instance, string name)
        {
            Instance.RegisterInstance(type, name, instance);
            return this;
        }

        public IContainerRegistry RegisterSingleton(Type from, Type to)
        {
            Instance.RegisterSingleton(from, to);
            return this;
        }

        public IContainerRegistry RegisterSingleton(Type from, Type to, string name)
        {
            Instance.RegisterSingleton(from, to, name);
            return this;
        }

        public IContainerRegistry Register(Type from, Type to)
        {
            Instance.RegisterType(from, to);
            return this;
        }

        public IContainerRegistry Register(Type from, Type to, string name)
        {
            Instance.RegisterType(from, to, name);
            return this;
        }

        public bool IsRegistered(Type type)
        {
            return Instance.IsRegistered(type);
        }

        public bool IsRegistered(Type type, string name)
        {
            return Instance.IsRegistered(type, name);
        }

        public IContainerRegistry RegisterMany(Type implementingType, params Type[] serviceTypes)
        {
            Instance.RegisterType(implementingType);
            return RegisterManyInternal(implementingType, serviceTypes);
        }

        public IContainerRegistry RegisterManySingleton(Type implementingType, params Type[] serviceTypes)
        {
            Instance.RegisterSingleton(implementingType);
            return RegisterManyInternal(implementingType, serviceTypes);
        }

        private IContainerRegistry RegisterManyInternal(Type implementingType, Type[] serviceTypes)
        {
            if(serviceTypes is null || serviceTypes.Length == 0)
            {
                serviceTypes = implementingType.GetInterfaces().Where(x => x != typeof(IDisposable)).ToArray();
            }

            foreach(var service in serviceTypes)
            {
                Instance.RegisterFactory(service, c => c.Resolve(implementingType));
            }

            return this;
        }

        public IContainerRegistry RegisterDelegate(Type serviceType, Func<object> factoryMethod)
        {
            Instance.RegisterFactory(serviceType, _ => factoryMethod());
            return this;
        }

        public IContainerRegistry RegisterDelegate(Type serviceType, Func<IContainerProvider, object> factoryMethod)
        {
            Instance.RegisterFactory(serviceType, c => factoryMethod(c.Resolve<IContainerProvider>()));
            return this;
        }

        public IContainerRegistry RegisterDelegate(Type serviceType, Func<IServiceProvider, object> factoryMethod)
        {
            Instance.RegisterFactory(serviceType, c => factoryMethod(c.Resolve<IServiceProvider>()));
            return this;
        }

        public IContainerRegistry RegisterSingletonFromDelegate(Type serviceType, Func<object> factoryMethod)
        {
            Instance.RegisterFactory(serviceType, _ => factoryMethod(), new ContainerControlledLifetimeManager());
            return this;
        }

        public IContainerRegistry RegisterSingletonFromDelegate(Type serviceType, Func<IContainerProvider, object> factoryMethod)
        {
            Instance.RegisterFactory(serviceType, c => factoryMethod(c.Resolve<IContainerProvider>()), new ContainerControlledLifetimeManager());
            return this;
        }

        public IContainerRegistry RegisterSingletonFromDelegate(Type serviceType, Func<IServiceProvider, object> factoryMethod)
        {
            Instance.RegisterFactory(serviceType, c => factoryMethod(c.Resolve<IServiceProvider>()), new ContainerControlledLifetimeManager());
            return this;
        }

        public IContainerRegistry RegisterScoped(Type serviceType)
        {
            Instance.RegisterType(serviceType, new HierarchicalLifetimeManager());
            return this;
        }

        public IContainerRegistry RegisterScoped(Type serviceType, Type implementationType)
        {
            Instance.RegisterType(serviceType, implementationType, new HierarchicalLifetimeManager());
            return this;
        }

        public IContainerRegistry RegisterScopedFromDelegate(Type serviceType, Func<object> factoryMethod)
        {
            Instance.RegisterFactory(serviceType, c => factoryMethod(), new HierarchicalLifetimeManager());
            return this;
        }

        public IContainerRegistry RegisterScopedFromDelegate(Type serviceType, Func<IContainerProvider, object> factoryMethod)
        {
            Instance.RegisterFactory(serviceType, c => factoryMethod(c.Resolve<IContainerProvider>()), new HierarchicalLifetimeManager());
            return this;
        }

        public IContainerRegistry RegisterScopedFromDelegate(Type serviceType, Func<IServiceProvider, object> factoryMethod)
        {
            Instance.RegisterFactory(serviceType, c => factoryMethod(c.Resolve<IServiceProvider>()), new HierarchicalLifetimeManager());
            return this;
        }

        void IScopeProvider.CreateScope()
        {
            CreateScopeInternal();
        }

        IServiceScope IServiceScopeFactory.CreateScope() =>
            CreateScopeInternal();

        private IServiceScope CreateScopeInternal()
        {
            if (_currentScope != null)
            {
                _currentScope.Dispose();
                _currentScope = null;
                GC.Collect();
            }

            _currentScope = new ServiceScope(Instance.CreateChildContainer());
            return _currentScope;
        }

        private class ServiceScope : IServiceScope, IServiceProvider
        {
            public ServiceScope(IUnityContainer container)
            {
                Container = container;
            }

            public IUnityContainer Container { get; private set; }

            public IServiceProvider ServiceProvider => this;

            public object GetService(Type serviceType)
            {
                if (!Container.IsRegistered(serviceType))
                    return null;

                return Container.Resolve(serviceType);
            }

            public void Dispose()
            {
                if (Container != null)
                {
                    Container.Dispose();
                    Container = null;
                }

                GC.Collect();
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
                var c = _currentScope?.Container ?? Instance;
                var overrides = parameters.Select(p => new DependencyOverride(p.Type, p.Instance)).ToArray();
                return c.Resolve(type, overrides);
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
                var c = _currentScope?.Container ?? Instance;
                var overrides = parameters.Select(p => new DependencyOverride(p.Type, p.Instance)).ToArray();
                return c.Resolve(type, name, overrides);
            }
            catch (Exception ex)
            {
                throw new ContainerResolutionException(type, name, ex);
            }
        }

        public object GetService(Type serviceType)
        {
            if (!IsRegistered(serviceType)) return null;

            return Resolve(serviceType);
        }
    }
}
