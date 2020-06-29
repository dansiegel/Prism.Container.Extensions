using Prism.Navigation;

namespace Prism.Forms.Extended.Mocks.ViewModels
{
    public class ViewCViewModel
    {
        public INavigationService NavigationService { get; }

        public ViewCViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }
    }
}
