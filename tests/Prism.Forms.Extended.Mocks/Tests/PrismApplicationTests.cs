using System;
using System.Linq;
using System.Threading.Tasks;
using Prism.Behaviors;
using Prism.Common;
using Prism.Forms.Extended.ViewModels;
using Prism.Ioc;
using Prism.Forms.Extended.Mocks;
using Prism.Forms.Extended.Mocks.ViewModels;
using Prism.Forms.Extended.Mocks.Views;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xunit;

using AndroidTabbedPage = Xamarin.Forms.PlatformConfiguration.AndroidSpecific.TabbedPage;
using iOSPage = Xamarin.Forms.PlatformConfiguration.iOSSpecific.Page;
using iOSNavPage = Xamarin.Forms.PlatformConfiguration.iOSSpecific.NavigationPage;
using NavigationPage = Xamarin.Forms.NavigationPage;
using TabbedPage = Xamarin.Forms.TabbedPage;

#if DRYIOC
using Prism.DryIoc;
#elif UNITY
using Prism.Unity;
#elif MICROSOFT_DI
using Prism.Microsoft.DependencyInjection;
#endif

namespace Prism.Forms.Extended.Tests
{
    public class PrismApplicationTests : IDisposable
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

        [Fact]
        public async Task UsesExplicitValue()
        {
            var app = CreateApp(Device.iOS);
            var result = await app.NavigationService.NavigateAsync("ViewA");

            Assert.True(result.Success);
            Assert.IsType<ViewA>(app.MainPage);

            Assert.True(iOSPage.GetUseSafeArea(app.MainPage));

            result = await app.NavigationService.NavigateAsync("/ViewB"); 
            Assert.True(result.Success);
            Assert.IsType<ViewB>(app.MainPage);

            Assert.False(iOSPage.GetUseSafeArea(app.MainPage));
        }

        [Fact]
        public void ErrorReportingNavigationServiceIsRegistered()
        {
            var app = CreateApp();
            INavigationService navService = null;

            // TODO: Remove internal constants
            var ex = Record.Exception(() => navService = app.Container.Resolve<INavigationService>(PrismApplicationBase.NavigationServiceName));

            Assert.Null(ex);
            Assert.NotNull(navService);
            Assert.IsType<ErrorReportingNavigationService>(navService);
        }

#if MICROSOFT_DI
        [Fact(Skip = "Not supported")]
#else
        [Fact]
#endif
        public async Task NavigationServiceHasExpectedPage()
        {
            var app = CreateApp();
            await app.NavigationService.NavigateAsync("ViewA");
            Assert.IsType<ViewA>(app.MainPage);
            Assert.NotNull(app.MainPage.BindingContext);
            Assert.IsType<ViewAViewModel>(app.MainPage.BindingContext);
            var vm = (ViewAViewModel)app.MainPage.BindingContext;
            var pa = (IPageAware)vm.NavigationService;
            Assert.NotNull(pa.Page);
            Assert.Same(app.MainPage, pa.Page);

            Assert.Empty(app.MainPage.Navigation.ModalStack);
            await vm.NavigationService.NavigateAsync("ViewB");
            Assert.NotEmpty(app.MainPage.Navigation.ModalStack);
            var modal = app.MainPage.Navigation.ModalStack.First();
            Assert.IsType<ViewB>(modal);
            Assert.NotNull(modal.BindingContext);
            Assert.IsType<ViewBViewModel>(modal.BindingContext);
            var vm2 = (ViewBViewModel)modal.BindingContext;
            var pa2 = (IPageAware)vm2.NavigationService;
            Assert.NotNull(pa2.Page);
            Assert.Same(modal, pa2.Page);
        }

        private AppMock CreateApp(string runtimePlatform = "Test")
        {
            Xamarin.Forms.Mocks.MockForms.Init(runtimePlatform);
            PrismContainerExtension.Reset();
            return new AppMock();
        }

        public void Dispose()
        {
            PrismContainerExtension.Reset();
        }
    }
}
