using System;
using Microsoft.Extensions.DependencyInjection;
using Prism.Ioc;
using Unity;

namespace Prism.Unity
{
    partial class UnityContainerExtension : IServiceScopeFactory
    {
        public UnityContainerExtension(IUnityContainer container)
        {
            Instance = container;
            string currentContainer = "CurrentContainer";
            Instance.RegisterInstance(currentContainer, this);
            Instance.RegisterFactory(typeof(IContainerExtension), c => c.Resolve<UnityContainerExtension>(currentContainer));
            Instance.RegisterFactory(typeof(IContainerProvider), c => c.Resolve<UnityContainerExtension>(currentContainer));
            Instance.RegisterFactory(typeof(IServiceScopeFactory), c => c.Resolve<UnityContainerExtension>(currentContainer));
            Instance.RegisterFactory(typeof(IServiceProvider), c => new UnityProvider(c));
        }

        IServiceScope IServiceScopeFactory.CreateScope()
        {
            var scope = CreateScopeInternal();
            return new PrismServiceScope(scope);
        }

        private class UnityProvider : IServiceProvider
        {
            private IUnityContainer _container { get; }

            public UnityProvider(IUnityContainer container)
            {
                _container = container;
            }

            public object GetService(Type serviceType)
            {
                try
                {
                    return _container.Resolve(serviceType);
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
