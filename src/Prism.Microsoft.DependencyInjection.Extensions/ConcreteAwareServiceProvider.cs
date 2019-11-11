using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Prism.Microsoft.DependencyInjection
{
    public class ConcreteAwareServiceProvider : IServiceProvider
    {
        public ConcreteAwareServiceProvider(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public IServiceProvider ServiceProvider { get; }

        public void Dispose()
        {
        }

        public object GetService(Type serviceType) =>
            ServiceProvider.GetService(serviceType) ?? GetConcreteImplementation(serviceType);

        private object GetConcreteImplementation(Type serviceType)
        {
            if (serviceType.IsClass && !serviceType.IsAbstract)
            {
                var constructorInfos = serviceType.GetConstructors(BindingFlags.Public).OrderByDescending(x => x.GetParameters().Count());
                if (constructorInfos.Any())
                {
                    var constructorInfo = constructorInfos.First();
                    var parameters = constructorInfo.GetParameters().Select(x => ServiceProvider.GetService(x.ParameterType) ?? GetConcreteImplementation(x.ParameterType)).ToArray();
                    return constructorInfo.Invoke(parameters);
                }

                return Activator.CreateInstance(serviceType);
            }

            return serviceType.IsValueType ? Activator.CreateInstance(serviceType) : null;
        }
    }
}
