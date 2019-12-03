using System;
using System.Collections.Generic;
using System.Text;
using Prism.Mvvm;
using Shiny.Net;

namespace PrismSample.ViewModels
{
    public class MainPageViewModel : BindableBase
    {
        private IConnectivity _connectivity { get; set; }

        public MainPageViewModel(IConnectivity connectivity)
        {
            _connectivity = connectivity;
            _connectivity.WhenInternetStatusChanged()
                .Subscribe(OnConnectivityChanged);

            OnConnectivityChanged(_connectivity.IsInternetAvailable());
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
