using Xamarin.Forms;

namespace Prism.Platform
{
    public static class PlatformSpecifics
    {
        public static readonly BindableProperty UseExplicitProperty =
            BindableProperty.CreateAttached("UseExplicit", typeof(bool), typeof(PlatformSpecifics), false);

        public static bool GetUseExplicit(BindableObject bindable) =>
            (bool)bindable.GetValue(UseExplicitProperty);

        public static void SetUseExplicit(BindableObject bindable, bool value) =>
            bindable.SetValue(UseExplicitProperty, value);
    }
}