using Prism.DryIoc.Forms.Extended.ViewModels;
using Prism.Platform;
using Xamarin.Forms;
using AndroidTabbedPage = Xamarin.Forms.PlatformConfiguration.AndroidSpecific.TabbedPage;
using iOSNavPage = Xamarin.Forms.PlatformConfiguration.iOSSpecific.NavigationPage;
using iOSPage = Xamarin.Forms.PlatformConfiguration.iOSSpecific.Page;
using TabbedPage = Xamarin.Forms.TabbedPage;

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

            if (_options.UseBottomTabs && !PlatformSpecifics.GetUseExplicit(page))
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

            if (!PlatformSpecifics.GetUseExplicit(page))
            {
                iOSPage.SetUseSafeArea(page, _options.UseSafeArea);
            }
        }

        public override void ApplyNavigationPageBehaviors(NavigationPage page)
        {
            base.ApplyNavigationPageBehaviors(page);

            if (!PlatformSpecifics.GetUseExplicit(page))
            {
                iOSNavPage.SetPrefersLargeTitles(page, _options.PreferLargeTitles);
            }
        }
    }
}