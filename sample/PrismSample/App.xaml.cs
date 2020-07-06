using System;
using Prism.Ioc;
using PrismSample.ViewModels;
using PrismSample.Views;
using Xamarin.Forms;

namespace PrismSample
{
    public partial class App
    {
        public App()
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            var result = await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(MainPage)}");

            if(!result.Success)
            {
                MainPage = new NavigationPage(new MainPage
                {
                    BindingContext = new MainPageViewModel
                    {
                        Message = result.Exception.Message
                    }
                });
                System.Diagnostics.Debugger.Break();
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<OtherPage, OtherPageViewModel>();
        }
    }
}
