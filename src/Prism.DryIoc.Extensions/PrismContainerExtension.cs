using System;
using DryIoc;
using Prism.Ioc;

namespace Prism.DryIoc
{
    public static class PrismContainerExtension
    {
        public static Rules DefaultRules => DryIocContainerExtension.DefaultRules;

        public static IContainerExtension Current => ContainerLocator.Current ?? Init();

        public static IContainerExtension Init() =>
            Init(DefaultRules);

        public static IContainerExtension Init(Rules rules) =>
            Init(new global::DryIoc.Container(rules));

        public static IContainerExtension Init(IContainer container)
        {
            if (ContainerLocator.Current != null)
                throw new NotSupportedException("The PrismContainerExtension has already been initialized.");

            var extension = new DryIocContainerExtension(container);
            ContainerLocator.SetContainerExtension(() => extension);
            return ContainerLocator.Current;
        }

        internal static void Reset()
        {
            ContainerLocator.ResetContainer();
            GC.Collect(int.MaxValue, GCCollectionMode.Forced);
            GC.WaitForFullGCComplete();
        }
    }
}
