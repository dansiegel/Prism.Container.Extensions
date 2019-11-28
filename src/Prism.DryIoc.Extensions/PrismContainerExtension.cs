using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using DryIoc;
using Prism.Container.Extensions;
using Prism.DryIoc;
using Prism.Ioc;
using IContainer = DryIoc.IContainer;

[assembly: ContainerExtension(typeof(PrismContainerExtension))]
[assembly: InternalsVisibleTo("Prism.DryIoc.Extensions.Tests")]
[assembly: InternalsVisibleTo("Prism.DryIoc.Forms.Extended.Tests")]
[assembly: InternalsVisibleTo("Shiny.Prism.Tests")]
namespace Prism.DryIoc
{
    public sealed partial class PrismContainerExtension : IContainerExtension<IContainer>, IExtendedContainerRegistry, IScopeProvider, IScopedFactoryRegistry
    {
        private static IContainerExtension<IContainer> _current;
        public static IContainerExtension<IContainer> Current
        {
            get
            {
                if(_current is null)
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
            Create(CreateContainerRules());

        public static IContainerExtension Create(Rules rules) =>
            Create(new global::DryIoc.Container(rules));

        public static IContainerExtension Create(IContainer container)
        {
            if(_current != null)
            {
                throw new NotSupportedException($"An instance of {nameof(PrismContainerExtension)} has already been created.");
            }

            return new PrismContainerExtension(container);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static Rules CreateContainerRules() => Rules.Default.WithAutoConcreteTypeResolution()
                                                                    .With(Made.Of(FactoryMethod.ConstructorWithResolvableArguments))
                                                                    .WithoutThrowOnRegisteringDisposableTransient()
            .WithFuncAndLazyWithoutRegistration()
#if __IOS__
                                                                    .WithoutFastExpressionCompiler()
#endif
                                                                    .WithDefaultIfAlreadyRegistered(IfAlreadyRegistered.Replace);

        private PrismContainerExtension() 
            : this(CreateContainerRules())
        {
        }

        private PrismContainerExtension(Rules rules) 
            : this(new global::DryIoc.Container(rules))
        {
        }

        private PrismContainerExtension(IContainer container)
        {
            _current = this;
            Instance = container;
            Instance.UseInstance<IContainerExtension>(this);
            Instance.UseInstance<IContainerRegistry>(this);
            Instance.UseInstance<IContainerProvider>(this);
            Instance.UseInstance<IServiceProvider>(this);
        }

        private IResolverContext _currentScope;

        public IContainer Instance { get; private set; }

        public void FinalizeExtension() { }

        public IContainerRegistry RegisterInstance(Type type, object instance)
        {
            Instance.UseInstance(type, instance);
            return this;
        }

        public IContainerRegistry RegisterInstance(Type type, object instance, string name)
        {
            Instance.UseInstance(type, instance, serviceKey: name);
            return this;
        }

        public IContainerRegistry RegisterSingleton(Type from, Type to)
        {
            Instance.Register(from, to, Reuse.Singleton);
            return this;
        }

        public IContainerRegistry RegisterSingleton(Type from, Type to, string name)
        {
            Instance.Register(from, to, Reuse.Singleton, serviceKey: name);
            return this;
        }

        public IContainerRegistry Register(Type from, Type to)
        {
            Instance.Register(from, to);
            return this;
        }

        public IContainerRegistry Register(Type from, Type to, string name)
        {
            Instance.Register(from, to, serviceKey: name);
            return this;
        }

        public IContainerRegistry RegisterMany(Type implementingType, params Type[] serviceTypes)
        {
            if(serviceTypes.Length == 0)
            {
                serviceTypes = implementingType.GetInterfaces();
            }

            Instance.RegisterMany(serviceTypes, implementingType, Reuse.Transient);
            return this;
        }

        public IContainerRegistry RegisterManySingleton(Type implementingType, params Type[] serviceTypes)
        {
            if (serviceTypes.Length == 0)
            {
                serviceTypes = implementingType.GetInterfaces();
            }

            Instance.RegisterMany(serviceTypes, implementingType, Reuse.Singleton);
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

        public IContainerRegistry RegisterDelegate(Type serviceType, Func<object> factoryMethod)
        {
            Instance.RegisterDelegate(serviceType, r => factoryMethod());
            return this;
        }

        public IContainerRegistry RegisterDelegate(Type serviceType, Func<IContainerProvider, object> factoryMethod)
        {
            Instance.RegisterDelegate(serviceType, r => factoryMethod(r.Resolve<IContainerProvider>()));
            return this;
        }

        public IContainerRegistry RegisterDelegate(Type serviceType, Func<IServiceProvider, object> factoryMethod)
        {
            Instance.RegisterDelegate(serviceType, r => factoryMethod(r));
            return this;
        }

        public IContainerRegistry RegisterSingletonFromDelegate(Type serviceType, Func<object> factoryMethod)
        {
            Instance.RegisterDelegate(serviceType, r => factoryMethod(), Reuse.Singleton);
            return this;
        }

        public IContainerRegistry RegisterSingletonFromDelegate(Type serviceType, Func<IContainerProvider, object> factoryMethod)
        {
            Instance.RegisterDelegate(serviceType, r => factoryMethod(r.Resolve<IContainerProvider>()), Reuse.Singleton);
            return this;
        }

        public IContainerRegistry RegisterSingletonFromDelegate(Type serviceType, Func<IServiceProvider, object> factoryMethod)
        {
            Instance.RegisterDelegate(serviceType, r => factoryMethod(r), Reuse.Singleton);
            return this;
        }

        public IContainerRegistry RegisterScoped(Type serviceType) =>
            RegisterScoped(serviceType, serviceType);

        public IContainerRegistry RegisterScoped(Type serviceType, Type implementationType)
        {
            Instance.Register(serviceType, implementationType, Reuse.Scoped);
            return this;
        }

        public IContainerRegistry RegisterScopedFromDelegate(Type serviceType, Func<object> factoryMethod)
        {
            Instance.RegisterDelegate(serviceType, r => factoryMethod(), Reuse.Scoped);
            return this;
        }

        public IContainerRegistry RegisterScopedFromDelegate(Type serviceType, Func<IContainerProvider, object> factoryMethod)
        {
            Instance.RegisterDelegate(serviceType, r => factoryMethod(r.Resolve<IContainerProvider>()));
            return this;
        }

        public IContainerRegistry RegisterScopedFromDelegate(Type serviceType, Func<IServiceProvider, object> factoryMethod)
        {
            Instance.RegisterDelegate(serviceType, r => factoryMethod(r), Reuse.Scoped);
            return this;
        }

        void IScopeProvider.CreateScope()
        {
            _currentScope?.Dispose();
            _currentScope = null;
            GC.Collect();
            _currentScope = Instance.OpenScope();
        }

        public object Resolve(Type type) => 
            Resolve(type, Array.Empty<(Type, object)>());

        public object Resolve(Type type, string name) =>
            Resolve(type, name, Array.Empty<(Type, object)>());

        public object Resolve(Type type, params (Type Type, object Instance)[] parameters)
        {
            try
            {
                var container = _currentScope ?? Instance;
                return container.Resolve(type, args: parameters.Select(p => p.Instance).ToArray());
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
                var container = _currentScope ?? Instance;
                return container.Resolve(type, name, args: parameters.Select(p => p.Instance).ToArray());
            }
            catch(Exception ex)
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
