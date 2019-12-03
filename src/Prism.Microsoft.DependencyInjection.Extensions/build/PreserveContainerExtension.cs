using Prism.Container.Extensions;

// Force the assembly into the App Domain
[assembly: Preserve(typeof(global::Prism.Microsoft.DependencyInjection.PrismContainerExtension))]
[assembly: Preserve(typeof(global::Microsoft.Extensions.DependencyInjection.ServiceCollection))]
[assembly: Preserve(typeof(global::Microsoft.Extensions.DependencyInjection.ServiceProvider))]
