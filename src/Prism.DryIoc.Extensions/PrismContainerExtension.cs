using System;
using DryIoc;
using Prism.Ioc;

namespace Prism.DryIoc
{
    public static class PrismContainerExtension
    {
        public static Rules DefaultRules => DryIocContainerExtension.DefaultRules;

        public static IContainerExtension Current => ContainerLocator.Current;

        public static void Init() =>
            Init(DefaultRules);

        public static void Init(Rules rules) =>
            Init(new global::DryIoc.Container(rules));

        public static void Init(IContainer container)
        {
            ContainerLocator.SetContainerExtension(() => new DryIocContainerExtension(container));
        }

        internal static void Reset()
        {
            ContainerLocator.ResetContainer();
            GC.Collect(int.MaxValue, GCCollectionMode.Forced);
            GC.WaitForFullGCComplete();
        }
    }
}
