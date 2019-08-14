using System;

namespace Prism.Ioc
{
    public interface IScopeProvider
    {
        void CreateScope();
    }

    public static class IContainerProviderExtensions
    {
        public static void CreateScope(this IContainerProvider container)
        {
            if(container is IScopeProvider scopeProvider)
            {
                scopeProvider.CreateScope();
            }
            else
            {
                throw new NotSupportedException("The specified container provider does not implement IScopeProvider");
            }
        }
    }
}
