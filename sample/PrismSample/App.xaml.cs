using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Humanizer;
using Prism.Ioc;
using Prism.Navigation;
using PrismSample.ViewModels;
using PrismSample.Views;
using Xamarin.Forms;

namespace PrismSample
{
    [AutoRegisterForNavigation]
    public partial class App
    {
        public App()
        {
        }

        protected override void OnInitialized()
        {
            InitializeComponent();

            NavigationService.NavigateAsync("navigation/main").Await(OnNavigationResult);

            
        }

        private void OnNavigationResult(INavigationResult result)
        {
            if (!result.Success)
            {
                MainPage = new MainPage
                {
                    BindingContext = new MainPageViewModel
                    {
                        Message = result.Exception.Message
                    }
                };
                System.Diagnostics.Debugger.Break();
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
           // var isRegistered = containerRegistry.IsRegistered<INavigationService>(App.NavigationServiceName);
            System.Diagnostics.Debugger.Break();
            //containerRegistry.RegisterForNavigation<NavigationPage>();
            //containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
        }

        protected override string GetNavigationSegmentNameFromType(Type pageType)
        {
            var name = Regex.Replace(pageType.Name, "Page$", string.Empty).Camelize();
            return name;
        }
    }
}
