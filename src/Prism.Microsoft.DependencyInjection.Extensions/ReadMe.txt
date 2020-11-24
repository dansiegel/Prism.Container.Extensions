Thank you for using the Prism.Container.Extensions!

Please note that as of Prism 8, this library is in maintenance mode. The extensions first introduced here, are now mostly included out of the box with Prism 8 with the exception of the IServiceCollection & ShinyLib support. This support is better provided via the Prism.Magician. The Magician is available to all GitHub sponsors for their use, and available via corporate license through AvantiPoint.

Docs are available at https://prismplugins.com

** SUPPORT **

This is an open source project and is provided as is. If you require support please contact AvantiPoint for a Support Contract.

** IMPORTANT **

This is an experimental package. There may be numerous issues with it and it is not recommended that you use this for production. This uses the Microsoft.Extensions.DependencyInjection package as the Prism Container. Do NOT use this with any other container packages!

** NOTE **

This package is meant to replace the container package shipped by the Prism team for the platform of your choice. You should reference the base platform package and update your app from PrismApplication to instead inherit from PrismApplicationBase and add the following code to your app:

protected override IContainerExtension CreateContainerExtension()
{
    return ContainerLocator.Current;
}

If building a Xamarin.Forms app you may instead use Prism.Forms.Extended which requires no changes to your code.