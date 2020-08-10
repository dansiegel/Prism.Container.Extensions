using Prism.Navigation;

namespace Prism.Forms.Extended.Mocks.ViewModels
{
    public class ViewBViewModel
    {
        public INavigationService NavigationService { get; }

        public ViewBViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }
    }
}
