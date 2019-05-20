using Xamarin.Forms;
using AndroidTabbedPage = Xamarin.Forms.PlatformConfiguration.AndroidSpecific.TabbedPage;
using TabbedPage = Xamarin.Forms.TabbedPage;
using iOSPage = Xamarin.Forms.PlatformConfiguration.iOSSpecific.Page;
using iOSNavPage = Xamarin.Forms.PlatformConfiguration.iOSSpecific.NavigationPage;
using Prism.DryIoc.Forms.Extended.ViewModels;

namespace Prism.Behaviors
{
    public class ExtendedPageBehaviorFactory : PageBehaviorFactory
    {
        private IPageBehaviorFactoryOptions _options { get; }

        public ExtendedPageBehaviorFactory(IPageBehaviorFactoryOptions options)
        {
            _options = options;
        }

        public override void ApplyTabbedPageBehaviors(TabbedPage page)
        {
            base.ApplyTabbedPageBehaviors(page);

            if (page.BindingContext is DefaultViewModel)
            {
                page.SetBinding(Page.TitleProperty, new Binding { Path = "Title" });
            }

            if (_options.UseBottomTabs)
            {
                AndroidTabbedPage.SetToolbarPlacement(page, Xamarin.Forms.PlatformConfiguration.AndroidSpecific.ToolbarPlacement.Bottom);
            }

            if (_options.UseChildTitle)
            {
                page.Behaviors.Add(new TabbedPageChildTitleBehavior());
            }
        }

        public override void ApplyPageBehaviors(Page page)
        {
            base.ApplyPageBehaviors(page);

            iOSPage.SetUseSafeArea(page, _options.UseSafeArea);
        }

        public override void ApplyNavigationPageBehaviors(NavigationPage page)
        {
            base.ApplyNavigationPageBehaviors(page);

            iOSNavPage.SetPrefersLargeTitles(page, _options.PreferLargeTitles);
        }
    }
}