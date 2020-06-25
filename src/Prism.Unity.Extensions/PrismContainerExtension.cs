using System;
using Prism.Ioc;
using Unity;

namespace Prism.Unity
{
    public static class PrismContainerExtension
    {
        public static IContainerExtension Current => ContainerLocator.Current ?? Init();

        public static IContainerExtension Init() =>
            Init(new UnityContainer());

        public static IContainerExtension Init(IUnityContainer container)
        {
            if (ContainerLocator.Current != null)
                throw new NotSupportedException("The PrismContainerExtension has already been initialized.");

            var extension = new UnityContainerExtension(container);
            ContainerLocator.SetContainerExtension(() => extension);
            return extension;
        }

        internal static void Reset()
        {
            ContainerLocator.ResetContainer();
            GC.Collect(int.MaxValue, GCCollectionMode.Forced);
            GC.WaitForFullGCComplete();
        }
    }
}
