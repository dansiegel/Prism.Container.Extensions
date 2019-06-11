using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Prism.Forms.Extended.Styles
{
    internal sealed class DefaultResources : ResourceDictionary
    {
        public DefaultResources()
        {
            var primary = Color.FromHex("#2196F3");
            var primaryDark = Color.FromHex("#1976D2");
            var primarylight = Color.FromHex("#BBDEFB");
            var accent = Color.FromHex("#9E9E9E");
            var navigationText = Color.FromHex("#FFFFFF");
            var primaryText = Color.FromHex("#212121");
            var secondaryText = Color.FromHex("#757575");
            var dividerColor = Color.FromHex("#BDBDBD");

            Add("Primary", primary);
            Add("PrimaryDark", primaryDark);
            Add("PrimaryLight", primarylight);
            Add("Accent", accent);
            Add("NavigationText", navigationText);
            Add("PrimaryText", primaryText);
            Add("SecondaryText", secondaryText);
            Add("DividerColor", dividerColor);

            var tabbedStyle = new Style(typeof(TabbedPage));
            tabbedStyle.Setters.Add(new Setter { Property = TabbedPage.BarBackgroundColorProperty, Value = primary });
            tabbedStyle.Setters.Add(new Setter { Property = TabbedPage.BarTextColorProperty, Value = navigationText });

            var navigationStyle = new Style(typeof(NavigationPage));
            navigationStyle.Setters.Add(new Setter { Property = NavigationPage.BarBackgroundColorProperty, Value = primary });
            navigationStyle.Setters.Add(new Setter { Property = NavigationPage.BarTextColorProperty, Value = navigationText });

            var button = new Style(typeof(Button));
            button.Setters.Add(new Setter { Property = Button.BackgroundColorProperty, Value = primaryDark });
            button.Setters.Add(new Setter { Property = Button.TextColorProperty, Value = navigationText });

            var label = new Style(typeof(Label));
            label.Setters.Add(new Setter { Property = Label.TextColorProperty, Value = primaryText });

            Add(tabbedStyle);
            Add(navigationStyle);
            Add(button);
            Add(label);
        }
    }
}