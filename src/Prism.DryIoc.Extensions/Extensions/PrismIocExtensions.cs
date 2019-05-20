using DryIoc;
using Prism.Ioc;

namespace Prism.DryIoc.Extensions
{
    public static class PrismIocExtensions
    {
        public static IContainer GetContainer(this IContainerProvider containerProvider)
        {
            return ((IContainerExtension<IContainer>)containerProvider).Instance;
        }

        public static IContainer GetContainer(this IContainerRegistry containerRegistry)
        {
            return ((IContainerExtension<IContainer>)containerRegistry).Instance;
        }

        public static bool IsRegistered<T>(this IContainerProvider containerProvider)
        {
            return ((IContainerExtension<IContainer>)containerProvider).Instance.IsRegistered<T>();
        }
    }
}
