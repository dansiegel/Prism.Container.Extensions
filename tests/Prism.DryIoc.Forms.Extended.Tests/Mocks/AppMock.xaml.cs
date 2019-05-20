using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Ioc;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Prism.DryIoc.Forms.Extended.Mocks
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [AutoRegisterForNavigation(Automatic = true)]
    public partial class AppMock
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
        }
    }
}