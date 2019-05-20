using Prism.Behaviors;
using Xamarin.Forms;

namespace Prism.Behaviors
{
    public class TabbedPageChildTitleBehavior : BehaviorBase<TabbedPage>
    {
        protected override void OnAttachedTo(TabbedPage bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.SetBinding(Page.TitleProperty, new Binding { Path = "CurrentPage.Title", Source = bindable });
        }
    }
}
