using Prism.Mvvm;
using Prism.Navigation;

namespace Prism.Forms.Extended.ViewModels
{
    internal class DefaultViewModel : BindableBase, IInitialize
    {
        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue("Title", out string title) ||
                parameters.TryGetValue("title", out title))
            {
                Title = title;
            }
        }
    }
}
