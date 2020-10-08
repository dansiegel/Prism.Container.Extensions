using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Prism;
using Prism.Common;
using Prism.Ioc;

namespace Prism.Navigation
{
    internal class NavigationServiceDelegate : INavigationServiceDelegate
    {
        // Provided to keep compatibility with Prism 8.0
        private const string NavigationServiceName = "PageNavigationService";

        private IContainerExtension Container { get; }
        private IApplicationProvider ApplicationProvider { get; }

        public NavigationServiceDelegate(IContainerExtension container, IApplicationProvider applicationProvider)
        {
            Container = container;
            ApplicationProvider = applicationProvider;
        }

        #region INavigationService
        Task<INavigationResult> INavigationService.GoBackAsync()
        {
            var navService = GetNavigationService();
            if (navService is null)
            {
                return PrismNotInitialized();
            }
            return navService.GoBackAsync();
        }

        Task<INavigationResult> INavigationService.GoBackAsync(INavigationParameters parameters)
        {
            var navService = GetNavigationService();
            if (navService is null)
            {
                return PrismNotInitialized();
            }
            return navService.GoBackAsync(parameters);
        }

        Task<INavigationResult> INavigationService.NavigateAsync(Uri uri)
        {
            var navService = GetNavigationService();
            if (navService is null)
            {
                return PrismNotInitialized();
            }
            return navService.NavigateAsync(uri);
        }

        Task<INavigationResult> INavigationService.NavigateAsync(Uri uri, INavigationParameters parameters)
        {
            var navService = GetNavigationService();
            if (navService is null)
            {
                return PrismNotInitialized();
            }
            return navService.NavigateAsync(uri, parameters);
        }

        Task<INavigationResult> INavigationService.NavigateAsync(string name)
        {
            var navService = GetNavigationService();
            if (navService is null)
            {
                return PrismNotInitialized();
            }
            return navService.NavigateAsync(name);
        }

        Task<INavigationResult> INavigationService.NavigateAsync(string name, INavigationParameters parameters)
        {
            var navService = GetNavigationService();
            if (navService is null)
            {
                return PrismNotInitialized();
            }
            return navService.NavigateAsync(name, parameters);
        }

        #endregion

        #region IPlatformNavigationService
        Task<INavigationResult> INavigationService.GoBackAsync(INavigationParameters parameters, bool? useModalNavigation, bool animated)
        {
            var navService = GetNavigationService();
            if (navService is null)
            {
                return PrismNotInitialized();
            }
            return navService.GoBackAsync(parameters, useModalNavigation, animated);
        }

        Task<INavigationResult> INavigationService.GoBackToRootAsync(INavigationParameters parameters)
        {
            var navService = GetNavigationService();
            if (navService is null)
            {
                return PrismNotInitialized();
            }
            return navService.GoBackToRootAsync(parameters);
        }

        Task<INavigationResult> INavigationService.NavigateAsync(string name, INavigationParameters parameters, bool? useModalNavigation, bool animated)
        {
            var navService = GetNavigationService();
            if (navService is null)
            {
                return PrismNotInitialized();
            }
            return navService.NavigateAsync(name, parameters, useModalNavigation, animated);
        }

        Task<INavigationResult> INavigationService.NavigateAsync(Uri uri, INavigationParameters parameters, bool? useModalNavigation, bool animated)
        {
            var navService = GetNavigationService();
            if (navService is null)
            {
                return PrismNotInitialized();
            }
            return navService.NavigateAsync(uri, parameters, useModalNavigation, animated);
        }
        #endregion

        private INavigationService GetNavigationService()
        {
            if (PrismApplicationBase.Current is null) return null;

            IContainerProvider container = Container;
            var navService = container.IsRegistered<INavigationService>(NavigationServiceName) ?
                container.Resolve<INavigationService>(NavigationServiceName) :
                container.Resolve<INavigationService>();

            if (navService is IPageAware pa)
            {
                pa.Page = PageUtilities.GetCurrentPage(ApplicationProvider.MainPage);
            }
            return navService;
        }

        private Task<INavigationResult> PrismNotInitialized()
        {
            INavigationResult result = new NavigationResult
            {
                Success = false,
                Exception = new NavigationException("No Prism Application Exists", null)
            };
            return Task.FromResult(result);
        }
    }
}
