using System;
using Prism.Behaviors;
using Prism.Ioc;
using Prism.Forms.Extended.Mocks;
using Xunit;
using Xamarin.Forms;
using System.Threading.Tasks;
using Prism.Forms.Extended.ViewModels;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using NavigationPage = Xamarin.Forms.NavigationPage;
using TabbedPage = Xamarin.Forms.TabbedPage;
using AndroidTabbedPage = Xamarin.Forms.PlatformConfiguration.AndroidSpecific.TabbedPage;
using iOSPage = Xamarin.Forms.PlatformConfiguration.iOSSpecific.Page;
using iOSNavPage = Xamarin.Forms.PlatformConfiguration.iOSSpecific.NavigationPage;
using Prism.Forms.Extended.Mocks.Views;
using Prism.Unity.Extensions;

namespace Prism.Unity.Forms.Extended.Tests
{
    public class PrismApplicationTests
    {
        [Fact]
        public void PageBehaviorFactorySetsTabbedPageTitle()
        {
            var app = CreateApp();
            var pageBehaviorFactory = app.Container.Resolve<IPageBehaviorFactory>();

            var tabbedPage = new TabbedPage();
            tabbedPage.Children.Add(new ContentPage { Title = "Page 1" });
            tabbedPage.Children.Add(new ContentPage { Title = "Page 2" });
            tabbedPage.Children.Add(new ContentPage { Title = "Page 3" });

            pageBehaviorFactory.ApplyPageBehaviors(tabbedPage);

            Assert.Equal("Page 1", tabbedPage.Title);

            tabbedPage.CurrentPage = tabbedPage.Children[1];

            Assert.Equal("Page 2", tabbedPage.Title);

            tabbedPage.CurrentPage = tabbedPage.Children[2];

            Assert.Equal("Page 3", tabbedPage.Title);
        }

        [Fact]
        public async Task TabbedPageGetsTitleSetFromNavigationUri()
        {
            var app = CreateApp();
            var result = await app.NavigationService.NavigateAsync("/TabbedPage?createTab=ViewA&createTab=ViewB&title=Title%20From%20Uri");

            Assert.True(result.Success);
            Assert.IsType<TabbedPage>(app.MainPage);
            Assert.IsType<DefaultViewModel>(app.MainPage.BindingContext);
            Assert.Equal("Title From Uri", app.MainPage.Title);
        }

        [Fact]
        public async Task AndroidTabbedPageHasBottomTabs()
        {
            var app = CreateApp(Device.Android);
            var result = await app.NavigationService.NavigateAsync("/TabbedPage?createTab=ViewA&createTab=ViewB");

            Assert.True(result.Success);
            Assert.IsType<TabbedPage>(app.MainPage);

            var toolbarPlacement = AndroidTabbedPage.GetToolbarPlacement(app.MainPage);
            Assert.Equal(ToolbarPlacement.Bottom, toolbarPlacement);
        }

        [Fact]
        public async Task PrefersLargeTitles()
        {
            var app = CreateApp(Device.iOS);
            var result = await app.NavigationService.NavigateAsync("NavigationPage/ViewA");

            Assert.True(result.Success);
            Assert.IsType<NavigationPage>(app.MainPage);

            Assert.True(iOSNavPage.GetPrefersLargeTitles(app.MainPage));
        }

        [Fact]
        public async Task UsesSafeArea()
        {
            var app = CreateApp(Device.iOS);
            var result = await app.NavigationService.NavigateAsync("ViewA");

            Assert.True(result.Success);
            Assert.IsType<ViewA>(app.MainPage);

            Assert.True(iOSPage.GetUseSafeArea(app.MainPage));
        }

        private AppMock CreateApp(string runtimePlatform = "Test")
        {
            Xamarin.Forms.Mocks.MockForms.Init(runtimePlatform);
            PrismContainerExtension.Reset();
            return new AppMock();
        }
    }
}
