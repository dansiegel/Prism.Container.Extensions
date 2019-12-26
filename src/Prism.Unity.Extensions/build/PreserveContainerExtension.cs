using Prism.Unity;

// Force the assembly into the App Domain
[assembly: Preserve(typeof(global::Prism.Unity.PrismContainerExtension))]
[assembly: Preserve(typeof(global::Unity.UnityContainer))]
