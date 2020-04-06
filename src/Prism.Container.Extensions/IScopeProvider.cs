using System;
using System.ComponentModel;

namespace Prism.Ioc
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IScopeProvider
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
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
