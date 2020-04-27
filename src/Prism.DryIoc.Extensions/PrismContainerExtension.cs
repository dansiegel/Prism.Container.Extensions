using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DryIoc;
using Prism.Container.Extensions;
using Prism.DryIoc;
using Prism.Ioc;
using IContainer = DryIoc.IContainer;

[assembly: Prism.DryIoc.Preserve]
[assembly: ContainerExtension(typeof(PrismContainerExtension))]
[assembly: InternalsVisibleTo("Prism.DryIoc.Extensions.Tests")]
[assembly: InternalsVisibleTo("Prism.DryIoc.Forms.Extended.Tests")]
[assembly: InternalsVisibleTo("Shiny.Prism.Tests")]
namespace Prism.DryIoc
{
    public sealed partial class PrismContainerExtension
    {
        private static IContainerExtension<IContainer> _current;
        public static IContainerExtension<IContainer> Current
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

        /// <summary>
        /// The Reset should ONLY be called for Unit Testing
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void Reset()
        {
            _current = null;
            Prism.Ioc.ContainerLocator.ResetContainer();
            Prism.Container.Extensions.ContainerLocator.Reset();
            GC.Collect(int.MaxValue, GCCollectionMode.Forced);
            GC.WaitForFullGCComplete();
        }

        public static IContainerExtension Create() =>
            Create(CreateContainerRules());

        public static IContainerExtension Create(Rules rules) =>
            Create(new global::DryIoc.Container(rules));

        public static IContainerExtension Create(IContainer container)
        {
            if (_current != null)
            {
                throw new NotSupportedException($"An instance of {nameof(PrismContainerExtension)} has already been created.");
            }

            _current = new DryIocContainerExtension(container);
            return _current;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static Rules CreateContainerRules() => DryIocContainerExtension.DefaultRules;

        
    }
}
