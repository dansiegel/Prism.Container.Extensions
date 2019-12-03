# Prism.Container.Extensions

The Prism Container Extensions provide various additional extensions making the Prism Container easier to use with IServiceCollection/IServiceProvider and in scenarios where you may require a Singleton container that may need to be initialized from Platform specific code prior to PrismApplication being created. Note that both the Prism.Container.Extensions and Prism.DryIoc.Extensions are platform agnostic meaning you can use them on WPF or Xamarin Forms.

For more information be sure to check out the docs at [prismplugins.com](https://prismplugins.com)

| Stage | Status |
|:-----:|--------|
| Build Extensions | [![Build Status](https://dev.azure.com/dansiegel/Prism.Plugins/_apis/build/status/dansiegel.Prism.Container.Extensions?branchName=master&stageName=Build%20NuGet%20Packages&jobName=Build%20Container%20Extensions)](https://dev.azure.com/dansiegel/Prism.Plugins/_build/latest?definitionId=41&branchName=master) |
| MyGet Deploy | [![Build Status](https://dev.azure.com/dansiegel/Prism.Plugins/_apis/build/status/dansiegel.Prism.Container.Extensions?branchName=master&stageName=Deploy%20NuGets&jobName=MyGet.org)](https://dev.azure.com/dansiegel/Prism.Plugins/_build/latest?definitionId=41&branchName=master) |
| NuGet Deploy | [![Build Status](https://dev.azure.com/dansiegel/Prism.Plugins/_apis/build/status/dansiegel.Prism.Container.Extensions?branchName=master&stageName=Deploy%20NuGets&jobName=NuGet.org)](https://dev.azure.com/dansiegel/Prism.Plugins/_build/latest?definitionId=41&branchName=master) |

## NuGet

You can add the MyGet CI feed to nuget by adding it as a source in Visual Studio:

`https://www.myget.org/F/prism-plugins/api/v3/index.json`

| Package | NuGet | MyGet |
|-------|:-----:|:------:|
| Prism.Container.Extensions | [![Latest NuGet][ContainerExtensionsShield]][ContainerExtensionsNuGet] | [![Latest CI Package][ContainerExtensionsMyGetShield]][ContainerExtensionsMyGet] |
| Prism.Forms.Extended | [![Latest NuGet][PrismFormsExtendedShield]][PrismFormsExtendedNuGet] | [![Latest CI Package][PrismFormsExtendedMyGetShield]][PrismFormsExtendedMyGet] |
| Prism.DryIoc.Extensions | [![Latest NuGet][DryIocExtensionsShield]][DryIocExtensionsNuGet] | [![Latest CI Package][DryIocExtensionsMyGetShield]][DryIocExtensionsMyGet] |
| Prism.Microsoft.DependencyInjection.Extensions | [![Latest NuGet][MsftDependencyInjectionExtensionsShield]][MsftDependencyInjectionExtensionsNuGet] | [![Latest CI Package][MsftDependencyInjectionExtensionsMyGetShield]][MsftDependencyInjectionExtensionsMyGet] |
| Prism.Unity.Extensions | [![Latest NuGet][UnityExtensionsShield]][UnityExtensionsNuGet] | [![Latest CI Package][UnityExtensionsMyGetShield]][UnityExtensionsMyGet] |
| Shiny.Prism | [![Latest NuGet][ShinyPrismShield]][ShinyPrismNuGet] | [![Latest CI Package][ShinyPrismMyGetShield]][ShinyPrismMyGet] |

## Support

If this project helped you reduce time to develop and made your app better, please be sure to star the project help support Dan.

[![GitHub Sponsors](https://github.blog/wp-content/uploads/2019/05/mona-heart-featured.png?fit=600%2C315)](https://xam.dev/sponsor-container-extensions)

[ContainerExtensionsNuGet]: https://www.nuget.org/packages/Prism.Container.Extensions
[ContainerExtensionsShield]: https://img.shields.io/nuget/vpre/Prism.Container.Extensions.svg
[ContainerExtensionsMyGet]: https://www.myget.org/feed/prism-plugins/package/nuget/Prism.Container.Extensions
[ContainerExtensionsMyGetShield]: https://img.shields.io/myget/prism-plugins/vpre/Prism.Container.Extensions.svg

[DryIocExtensionsNuGet]: https://www.nuget.org/packages/Prism.DryIoc.Extensions
[DryIocExtensionsShield]: https://img.shields.io/nuget/vpre/Prism.DryIoc.Extensions.svg
[DryIocExtensionsMyGet]: https://www.myget.org/feed/prism-plugins/package/nuget/Prism.DryIoc.Extensions
[DryIocExtensionsMyGetShield]: https://img.shields.io/myget/prism-plugins/vpre/Prism.DryIoc.Extensions.svg

[MsftDependencyInjectionExtensionsNuGet]: https://www.nuget.org/packages/Prism.Microsoft.DependencyInjection.Extensions
[MsftDependencyInjectionExtensionsShield]: https://img.shields.io/nuget/vpre/Prism.Microsoft.DependencyInjection.Extensions.svg
[MsftDependencyInjectionExtensionsMyGet]: https://www.myget.org/feed/prism-plugins/package/nuget/Prism.Microsoft.DependencyInjection.Extensions
[MsftDependencyInjectionExtensionsMyGetShield]: https://img.shields.io/myget/prism-plugins/vpre/Prism.Microsoft.DependencyInjection.Extensions.svg

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
