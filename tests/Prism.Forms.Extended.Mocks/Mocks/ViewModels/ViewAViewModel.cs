using System;
using System.Collections.Generic;
using System.Text;
using Prism.Forms.Extended.Mocks.Views;
using Prism.Navigation;

namespace Prism.Forms.Extended.Mocks.ViewModels
{
    public class ViewAViewModel
    {
        public INavigationService NavigationService { get; }

        public ViewAViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }
    }
}
