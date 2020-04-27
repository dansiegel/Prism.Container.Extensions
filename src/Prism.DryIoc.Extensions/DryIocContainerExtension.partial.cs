using System;
using DryIoc;
using Microsoft.Extensions.DependencyInjection;
using Prism.Container.Extensions;
using Prism.Ioc;

namespace Prism.DryIoc
{
    // NOTE: The IContainerExtension implementation comes from Prism.
    internal partial class DryIocContainerExtension : IServiceScopeFactory
    {
        public DryIocContainerExtension()
            : this(DefaultRules)
        {
        }

        public DryIocContainerExtension(Rules rules)
            : this(new global::DryIoc.Container(rules))
        {
        }

        public DryIocContainerExtension(IContainer container)
        {
            Instance = container;
            Instance.RegisterInstanceMany(new[]
            {
                typeof(IContainerExtension),
                typeof(IContainerRegistry),
                typeof(IContainerProvider),
                typeof(IServiceScopeFactory)
            }, this);
            Instance.Register<IServiceProvider, PrismServiceProvider>();
        }

        IServiceScope IServiceScopeFactory.CreateScope()
        {
            var scope = CreateScopeInternal();
            return new ServiceScope(scope);
        }

        private class ServiceScope : IServiceScope
        {
            public ServiceScope(IResolverContext context)
            {
                Context = context;
            }

            public IResolverContext Context { get; private set; }

            public IServiceProvider ServiceProvider => Context;

            public void Dispose()
            {
                if (Context != null)
                {
                    Context.Dispose();
                    Context = null;
                }

                GC.Collect();
            }
        }
    }
}
