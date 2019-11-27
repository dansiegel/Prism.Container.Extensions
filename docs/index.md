# Prism.Container.Extensions

The Prism Container Extensions provide various additional extensions making the Prism Container easier to use with IServiceCollection/IServiceProvider and in scenarios where you may require a Singleton container that may need to be initialized from Platform specific code prior to PrismApplication being created. Note that both the Prism.Container.Extensions and Prism.DryIoc.Extensions are platform agnostic meaning you can use them on WPF or Xamarin Forms.

## Support

This project is maintained by Dan Siegel. If this project or others maintained by Dan have helped you please help support the project by [sponsoring Dan](https://xam.dev/sponsor-container-extensions) on GitHub!

[![GitHub Sponsors](https://github.blog/wp-content/uploads/2019/05/mona-heart-featured.png?fit=600%2C315)](https://xam.dev/sponsor-container-extensions)

## Why use the Container Extensions?

While the Container abstractions provided by `IContainerRegistry` will give you what you need at least 95% of the time, there are still a number of times that you need to be able to do some more advanced registrations for your services. While adding these additional methods may confuse the average developer who is still struggling to understand "What is a Transient, what is a Singleton, when do I use each?". The extensions give you exactly what you need in order to write extremely loosely coupled code that can be reused from one project to another regardless of whether you are developing for Prism for WPF, Prism for Xamarin.Forms or just doing your own thing with Prism.Core.

## NuGet

| Package | NuGet | MyGet |
|-------|:-----:|:------:|
| Prism.Container.Extensions | [![Latest NuGet][ContainerExtensionsShield]][ContainerExtensionsNuGet] | [![Latest CI Package][ContainerExtensionsMyGetShield]][ContainerExtensionsMyGet] |
| Prism.Forms.Extended | [![Latest NuGet][PrismFormsExtendedShield]][PrismFormsExtendedNuGet] | [![Latest CI Package][PrismFormsExtendedMyGetShield]][PrismFormsExtendedMyGet] |
| Prism.DryIoc.Extensions | [![Latest NuGet][DryIocExtensionsShield]][DryIocExtensionsNuGet] | [![Latest CI Package][DryIocExtensionsMyGetShield]][DryIocExtensionsMyGet] |
| Prism.Microsoft.DependencyInjection.Extensions | [![Latest NuGet][MsftDependencyInjectionExtensionsShield]][MsftDependencyInjectionExtensionsNuGet] | [![Latest CI Package][MsftDependencyInjectionExtensionsMyGetShield]][MsftDependencyInjectionExtensionsMyGet] |
| Prism.Unity.Extensions | [![Latest NuGet][UnityExtensionsShield]][UnityExtensionsNuGet] | [![Latest CI Package][UnityExtensionsMyGetShield]][UnityExtensionsMyGet] |
| Shiny.Prism | [![Latest NuGet][ShinyPrismShield]][ShinyPrismNuGet] | [![Latest CI Package][ShinyPrismMyGetShield]][ShinyPrismMyGet] |

### CI NuGet Feed

Want to consume the CI packages? You can add this as a NuGet.config in your project root and Visual Studio will automatically pick up the configuration to provide packages from the CI Feed. Note that packages from this feed have passed all of the tests, but may have code that is still unstable.

```xml
<configuration>
  <packageSources>
    <clear />
    <add key="PrismPlugins-MyGet" value="https://www.myget.org/F/prism-plugins/api/v3/index.json" />
    <add key="NuGet.org" value="https://api.nuget.org/v3/index.json" />
  </packageSources>
</configuration>
```

[ContainerExtensionsNuGet]: https://www.nuget.org/packages/Prism.Container.Extensions
[ContainerExtensionsShield]: https://img.shields.io/nuget/vpre/Prism.Container.Extensions.svg
[ContainerExtensionsMyGet]: https://www.myget.org/feed/prism-plugins/package/nuget/Prism.Container.Extensions
[ContainerExtensionsMyGetShield]: https://img.shields.io/myget/prism-plugins/vpre/Prism.Container.Extensions.svg

[DryIocExtensionsNuGet]: https://www.nuget.org/packages/Prism.DryIoc.Extensions
[DryIocExtensionsShield]: https://img.shields.io/nuget/vpre/Prism.DryIoc.Extensions.svg
[DryIocExtensionsMyGet]: https://www.myget.org/feed/prism-plugins/package/nuget/Prism.DryIoc.Extensions
[DryIocExtensionsMyGetShield]: https://img.shields.io/myget/prism-plugins/vpre/Prism.DryIoc.Extensions.svg

[DryIocFormsExtendedNuGet]: https://www.nuget.org/packages/Prism.DryIoc.Forms.Extended
[DryIocFormsExtendedShield]: https://img.shields.io/nuget/vpre/Prism.DryIoc.Forms.Extended.svg
[DryIocFormsExtendedMyGet]: https://www.myget.org/feed/prism-plugins/package/nuget/Prism.DryIoc.Forms.Extended
[DryIocFormsExtendedMyGetShield]: https://img.shields.io/myget/prism-plugins/vpre/Prism.DryIoc.Forms.Extended.svg

[MsftDependencyInjectionExtensionsNuGet]: https://www.nuget.org/packages/Prism.Microsoft.DependencyInjection.Extensions
[MsftDependencyInjectionExtensionsShield]: https://img.shields.io/nuget/vpre/Prism.Microsoft.DependencyInjection.Extensions.svg
[MsftDependencyInjectionExtensionsMyGet]: https://www.myget.org/feed/prism-plugins/package/nuget/Prism.Microsoft.DependencyInjection.Extensions
[MsftDependencyInjectionExtensionsMyGetShield]: https://img.shields.io/myget/prism-plugins/vpre/Prism.Microsoft.DependencyInjection.Extensions.svg

[MsftDependencyInjectionFormsExtendedNuGet]: https://www.nuget.org/packages/Prism.Microsoft.DependencyInjection.Forms.Extended
[MsftDependencyInjectionFormsExtendedShield]: https://img.shields.io/nuget/vpre/Prism.Microsoft.DependencyInjection.Forms.Extended.svg
[MsftDependencyInjectionFormsExtendedMyGet]: https://www.myget.org/feed/prism-plugins/package/nuget/Prism.Microsoft.DependencyInjection.Forms.Extended
[MsftDependencyInjectionFormsExtendedMyGetShield]: https://img.shields.io/myget/prism-plugins/vpre/Prism.Microsoft.DependencyInjection.Forms.Extended.svg

[PrismFormsExtendedNuGet]: https://www.nuget.org/packages/Prism.Forms.Extended
[PrismFormsExtendedShield]: https://img.shields.io/nuget/vpre/Prism.Forms.Extended.svg
[PrismFormsExtendedMyGet]: https://www.myget.org/feed/prism-plugins/package/nuget/Prism.Forms.Extended
[PrismFormsExtendedMyGetShield]: https://img.shields.io/myget/prism-plugins/vpre/Prism.Forms.Extended.svg

[ShinyPrismNuGet]: https://www.nuget.org/packages/Shiny.Prism
[ShinyPrismShield]: https://img.shields.io/nuget/vpre/Shiny.Prism.svg
[ShinyPrismMyGet]: https://www.myget.org/feed/prism-plugins/package/nuget/Shiny.Prism
[ShinyPrismMyGetShield]: https://img.shields.io/myget/prism-plugins/vpre/Shiny.Prism.svg

[UnityExtensionsNuGet]: https://www.nuget.org/packages/Prism.Unity.Extensions
[UnityExtensionsShield]: https://img.shields.io/nuget/vpre/Prism.Unity.Extensions.svg
[UnityExtensionsMyGet]: https://www.myget.org/feed/prism-plugins/package/nuget/Prism.Unity.Extensions
[UnityExtensionsMyGetShield]: https://img.shields.io/myget/prism-plugins/vpre/Prism.Unity.Extensions.svg

[UnityFormsExtendedNuGet]: https://www.nuget.org/packages/Prism.Unity.Forms.Extended
[UnityFormsExtendedShield]: https://img.shields.io/nuget/vpre/Prism.Unity.Forms.Extended.svg
[UnityFormsExtendedMyGet]: https://www.myget.org/feed/prism-plugins/package/nuget/Prism.Unity.Forms.Extended
[UnityFormsExtendedMyGetShield]: https://img.shields.io/myget/prism-plugins/vpre/Prism.Unity.Forms.Extended.svg