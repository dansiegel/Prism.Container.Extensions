using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using PrismSample.Views;
using Shiny.Net;
using Xamarin.Forms;

namespace PrismSample.ViewModels
{
    public class MainPageViewModel : BindableBase
    {
        private IConnectivity _connectivity { get; set; }
        private readonly INavigationService _navigationService;

        public MainPageViewModel() { }

        public MainPageViewModel(IConnectivity connectivity, INavigationService navigationService)
        {
            _connectivity = connectivity;
            _navigationService = navigationService;

            GoToOtherCommand = ExecutionAwareCommand.FromTask(GoToOtherAsync);

            _connectivity.WhenInternetStatusChanged()
                    .Subscribe(OnConnectivityChanged);

            OnConnectivityChanged(_connectivity.IsInternetAvailable());
        }

        public ICommand GoToOtherCommand { get; set; }

        private Task GoToOtherAsync()
        {
            return _navigationService.NavigateAsync($"{nameof(OtherPage)}");
        }

        private string _message;
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        private void OnConnectivityChanged(bool connected)
        {
            if(connected)
            {
                Message = "The internet is connected... We can now do our anti-Social Media... and swipe right, and like our friend's lunch...";
            }
            else
            {
                Message = "Whoops! It seems the internet has gone missing... we're going into withdrawls... please bring it back...";
            }
        }
    }
}
