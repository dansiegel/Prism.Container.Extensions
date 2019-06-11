using Prism.AppModel;
using Prism.Mvvm;

namespace Prism.DryIoc.Forms.Extended.ViewModels
{
    internal class DefaultViewModel : BindableBase, IAutoInitialize
    {
        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
    }
}
