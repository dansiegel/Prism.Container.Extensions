using Prism.Forms.Extended.Mocks.Views;
using Prism.Ioc;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Prism.Forms.Extended.Mocks
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppMock : PrismApplication
    {
        public AppMock()
        {

        }

        public new INavigationService NavigationService => base.NavigationService;

        protected override void OnInitialized()
        {
            InitializeComponent();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<TabbedPage>();
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<ViewA>();
            containerRegistry.RegisterForNavigation<ViewB>();
            containerRegistry.RegisterForNavigation<ViewC>();
            containerRegistry.RegisterForNavigation<ViewD>();
        }
    }
}