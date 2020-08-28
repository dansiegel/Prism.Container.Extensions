using System;
using System.Collections.Generic;
using System.Text;
using DryIoc;
using Microsoft.Extensions.DependencyInjection;
using Prism.DryIoc;
using Prism.Ioc;

namespace Prism.DryIoc
{
    partial class DryIocContainerExtension : IServiceScopeFactory
    {
        public DryIocContainerExtension(IContainer container)
        {
            Instance = container;
            Instance.RegisterInstanceMany(new[]
            {
                typeof(IContainerExtension),
                typeof(IContainerProvider),
                typeof(IServiceScopeFactory)
            }, this);
            Instance.RegisterDelegate<IServiceProvider>(r => r);
        }

        IServiceScope IServiceScopeFactory.CreateScope()
        {
            var scope = CreateScopeInternal();
            return new PrismServiceScope(scope);
        }
    }
}
