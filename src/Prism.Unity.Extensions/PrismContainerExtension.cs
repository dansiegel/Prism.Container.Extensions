using System;
using System.Runtime.CompilerServices;
using Prism.Container.Extensions;
using Prism.Ioc;
using Prism.Unity;
using Unity;

[assembly: Prism.Unity.Preserve]
[assembly: ContainerExtension(typeof(PrismContainerExtension))]
[assembly: InternalsVisibleTo("Prism.Unity.Extensions.Tests")]
[assembly: InternalsVisibleTo("Prism.Unity.Forms.Extended.Tests")]
namespace Prism.Unity
{
    public partial class PrismContainerExtension
    {
        private static IContainerExtension<IUnityContainer> _current;
        public static IContainerExtension<IUnityContainer> Current
        {
            get
            {
                if (_current is null)
                {
                    Create();
                }

                return _current;
            }
        }

        internal static void Reset()
        {
            _current = null;
            Prism.Ioc.ContainerLocator.ResetContainer();
            Prism.Container.Extensions.ContainerLocator.Reset();
            GC.Collect(int.MaxValue, GCCollectionMode.Forced);
            GC.WaitForFullGCComplete();
        }

        public static IContainerExtension Create() =>
            Create(new UnityContainer());

        public static IContainerExtension Create(IUnityContainer container)
        {
            if (_current != null)
            {
                throw new NotSupportedException($"An instance of {nameof(PrismContainerExtension)} has already been created.");
            }

            _current = new UnityContainerExtension(container);
            return _current;
        }
    }
}
