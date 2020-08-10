using Prism.Navigation;

namespace Prism.Forms.Extended.Mocks.ViewModels
{
    public class ViewDViewModel
    {
        public INavigationService NavigationService { get; }

        public ViewDViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }
    }
}
