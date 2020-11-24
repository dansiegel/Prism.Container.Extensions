Thank you for using the Prism.Container.Extensions!

Please note that as of Prism 8, this library is in maintenance mode. The extensions first introduced here, are now mostly included out of the box with Prism 8 with the exception of the IServiceCollection & ShinyLib support. This support is better provided via the Prism.Magician. The Magician is available to all GitHub sponsors for their use, and available via corporate license through AvantiPoint.

Docs are available at https://prismplugins.com

** SUPPORT **

This is an open source project and is provided as is. If you require support please contact AvantiPoint for a Support Contract.

** NOTE **

This package is meant to be used with one of the Container Extensions that are part of this project:

- Prism.DryIoc.Extensions
- Prism.Microsoft.DependencyInjection.Extensions
- Prism.Unity.Extensions

You should not be using a container package from Prism directly such as Prism.DryIoc.Forms or Prism.Unity.Forms. Be sure to use Prism.Forms.Extended along with one of the above container extensions to properly support Shiny.Prism. Alternatively you may use Prism.Forms along with one of the above container extensions. In that case you will need to update your app to inherit from PrismApplicationBase and add the following override to your App class:

protected override IContainerExtension CreateContainerExtension()
{
    return ContainerLocator.Current;
}