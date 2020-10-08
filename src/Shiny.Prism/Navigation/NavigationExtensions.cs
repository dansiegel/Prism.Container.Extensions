using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Prism.Common;
using Prism.Navigation;

namespace Shiny
{
    public static class NavigationExtensions
    {
        public static void UseNavigationDelegate(this IServiceCollection services)
        {
            services.AddSingleton<INavigationServiceDelegate, NavigationServiceDelegate>();
            if (!services.Any(x => x.ServiceType == typeof(IApplicationProvider)))
            {
                services.AddSingleton<IApplicationProvider, ApplicationProvider>();
            }
        }
    }
}
