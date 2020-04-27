using System;
using Microsoft.Extensions.DependencyInjection;
using Prism.Container.Extensions;
using Prism.Ioc;
using Unity;

namespace Prism.Unity
{
    internal partial class UnityContainerExtension : IServiceScopeFactory
    {

        public UnityContainerExtension()
           : this(new UnityContainer())
        {
        }

        public UnityContainerExtension(IUnityContainer container)
        {
            Instance = container;
            Instance.RegisterInstance<IContainerProvider>(this);
            Instance.RegisterInstance<IContainerExtension>(this);
            Instance.RegisterInstance<IContainerRegistry>(this);
            Instance.RegisterInstance<IServiceScopeFactory>(this);
            Instance.RegisterSingleton<IServiceProvider, PrismServiceProvider>();
        }


        IServiceScope IServiceScopeFactory.CreateScope()
        {
            var scope = CreateScopeInternal();
            return new ServiceScope(scope);
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
    }
}
