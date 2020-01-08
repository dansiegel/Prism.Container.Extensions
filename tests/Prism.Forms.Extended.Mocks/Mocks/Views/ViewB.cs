using Prism.Platform;
using Xamarin.Forms;

namespace Prism.Forms.Extended.Mocks.Views
{
    public class ViewB : ContentPage
    {
        public ViewB()
        {
            Title = "ViewB";
            PlatformSpecifics.SetUseExplicit(this, true);
        }
    }
}
