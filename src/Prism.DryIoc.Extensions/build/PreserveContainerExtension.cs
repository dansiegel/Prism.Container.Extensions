using Prism.Container.Extensions;

// Force the assembly into the App Domain
[assembly: Preserve(typeof(global::Prism.DryIoc.PrismContainerExtension))]
[assembly: Preserve(typeof(global::DryIoc.Container))]
