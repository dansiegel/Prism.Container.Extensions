using System.ComponentModel;
using Xamarin.Forms;

namespace Prism.Behaviors
{
    public class TabbedPageChildTitleBehavior : BehaviorBase<TabbedPage>
    {
        protected override void OnAttachedTo(TabbedPage bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.SetBinding(Page.TitleProperty, new Binding { Path = "CurrentPage.Title", Source = bindable });
            
            if(bindable.BindingContext is INotifyPropertyChanged vm)
            {
                vm.PropertyChanged += OnViewModelPropertyChanged;
            }
        }

        protected override void OnDetachingFrom(TabbedPage bindable)
        {
            base.OnDetachingFrom(bindable);

            if (bindable.BindingContext is INotifyPropertyChanged vm)
            {
                vm.PropertyChanged -= OnViewModelPropertyChanged;
            }
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "Title")
            {
                AssociatedObject.RemoveBinding(Page.TitleProperty);
                var property = AssociatedObject.BindingContext.GetType().GetProperty(e.PropertyName);
                AssociatedObject.Title = (string)property.GetValue(AssociatedObject.BindingContext);
            }
        }
    }
}
