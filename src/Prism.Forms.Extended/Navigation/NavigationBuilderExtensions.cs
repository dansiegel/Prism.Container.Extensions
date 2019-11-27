using System;
using Prism.Forms.Extended.Navigation;
using Prism.Navigation;

namespace Prism.Navigation
{
    public static class NavigationBuilderExtensions
    {
        public static NavigationBuilder Navigate(this INavigationService navigationService, string name) =>
            new NavigationBuilder(navigationService, name);

        public static NavigationBuilder Navigate(this INavigationService navigationService, Uri uri) =>
            new NavigationBuilder(navigationService, uri);

        public static NavigationBuilder GoBack(this INavigationService navigationService) =>
            new NavigationBuilder(navigationService, NavigationInstruction.GoBack);

        public static NavigationBuilder GoBackToRoot(this INavigationService navigationService) =>
            new NavigationBuilder(navigationService, NavigationInstruction.GoBackToRoot);
    }
}
