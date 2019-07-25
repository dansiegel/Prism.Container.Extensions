using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Prism;
using Prism.Ioc;
using Xamarin.Forms;

namespace Shiny.Prism.Modularity
{
    internal static class IContainerRegistryAutoLoadExtensions
    {
        public static void AutoRegisterViews(this Type type, IContainerRegistry containerRegistry)
        {
            if (!type.GetCustomAttributes().Any(a => a is AutoRegisterForNavigationAttribute)) return;

            var regAttr = type.GetCustomAttribute<AutoRegisterForNavigationAttribute>();
            var assembly = type.Assembly;

            var viewTypes = assembly.ExportedTypes.Where(t => t.IsSubclassOf(typeof(Page)));
            RegisterViewsAutomatically(containerRegistry, viewTypes);
        }

        private static void RegisterViewsAutomatically(IContainerRegistry containerRegistry, IEnumerable<Type> viewTypes)
        {
            foreach (var viewType in viewTypes)
            {
                RegisterView(containerRegistry, viewType);
            }

            RegisterView(containerRegistry, typeof(NavigationPage), true);
            RegisterView(containerRegistry, typeof(TabbedPage), true);
        }

        private static void RegisterView(IContainerRegistry containerRegistry, Type viewType, bool checkIfRegistered = false)
        {
            //var name = AutoRegistrationViewNameProvider.GetNavigationSegmentName(viewType);
            var name = GetNavigationSegmentName(viewType);

            if (!checkIfRegistered || containerRegistry.IsRegistered<object>(name))
            {
                containerRegistry.RegisterForNavigation(viewType, name);
            }
        }

        internal static string GetNavigationSegmentName(Type type)
        {
            var nameProviderType = Type.GetType("Prism.Ioc.AutoRegistrationViewNameProvider, Prism.Forms");
            return (string)nameProviderType.GetMethod(nameof(GetNavigationSegmentName)).Invoke(null, new[] { type });
        }
    }
}
