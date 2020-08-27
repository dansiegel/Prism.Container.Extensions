using System.Reflection;
using Moq;
using Prism.Behaviors;
using Prism.Forms.Extended.ViewModels;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xunit;
using AndroidTabbedPage = Xamarin.Forms.PlatformConfiguration.AndroidSpecific.TabbedPage;
using NavigationPage = Xamarin.Forms.NavigationPage;
using TabbedPage = Xamarin.Forms.TabbedPage;

namespace Prism.Forms.Extended.Tests
{
    public class ExtendedPageBehaviorFactoryTests
    {
        [Fact]
        public void SetsPreferLargeTitles()
        {
            Xamarin.Forms.Mocks.MockForms.Init(Device.iOS);
            var _ = new Xamarin.Forms.Application();
            IPageBehaviorFactory factory = new ExtendedPageBehaviorFactory(new DefaultPageBehaviorFactoryOptions());

            var page = new NavigationPage();
            ConfigurePage(page);
            factory.ApplyPageBehaviors(page);

            Assert.True(page.On<iOS>().PrefersLargeTitles());
        }

        [Fact]
        public void SetsUseSafeArea()
        {
            Xamarin.Forms.Mocks.MockForms.Init(Device.iOS);
            var _ = new Xamarin.Forms.Application();
            IPageBehaviorFactory factory = new ExtendedPageBehaviorFactory(new DefaultPageBehaviorFactoryOptions());

            var page = new ContentPage();
            ConfigurePage(page);
            factory.ApplyPageBehaviors(page);

            Assert.True(page.On<iOS>().UsingSafeArea());
        }

        [Fact]
        public void SetUseBottomTabs()
        {
            Xamarin.Forms.Mocks.MockForms.Init(Device.Android);
            var _ = new Xamarin.Forms.Application();
            IPageBehaviorFactory factory = new ExtendedPageBehaviorFactory(new DefaultPageBehaviorFactoryOptions());

            var page = new TabbedPage();
            ConfigurePage(page);
            factory.ApplyPageBehaviors(page);

            Assert.Equal(ToolbarPlacement.Bottom, AndroidTabbedPage.GetToolbarPlacement(page));
        }

        [Fact]
        public void SetsTitleBinding()
        {
            Xamarin.Forms.Mocks.MockForms.Init();
            var _ = new Xamarin.Forms.Application();
            IPageBehaviorFactory factory = new ExtendedPageBehaviorFactory(new DefaultPageBehaviorFactoryOptions());

            var page = new TabbedPage
            {
                BindingContext = new DefaultViewModel()
            };

            ConfigurePage(page);
            factory.ApplyPageBehaviors(page);

            Assert.True(page.IsSet(Xamarin.Forms.Page.TitleProperty));
        }

        [Fact]
        public void AddsChildTitleBehavior()
        {
            Xamarin.Forms.Mocks.MockForms.Init();
            var _ = new Xamarin.Forms.Application();
            IPageBehaviorFactory factory = new ExtendedPageBehaviorFactory(new DefaultPageBehaviorFactoryOptions());

            var page = new TabbedPage();
            ConfigurePage(page);
            factory.ApplyPageBehaviors(page);

            Assert.Contains(page.Behaviors, b => b.GetType() == typeof(TabbedPageChildTitleBehavior));
        }

        private void ConfigurePage(Xamarin.Forms.Page page)
        {
            var prismNavigationType = typeof(Prism.Navigation.Xaml.Navigation);
            var navServicePropInfo = prismNavigationType.GetField("NavigationServiceProperty", BindingFlags.Static | BindingFlags.NonPublic);
            var navServiceProp = (BindableProperty)navServicePropInfo.GetValue(null);
            page.SetValue(navServiceProp, Mock.Of<INavigationService>());
        }
    }
}
