using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Forms.Extended.Common;

namespace Prism.Commands
{
    public abstract class ExecutionAwareCommandBase : DelegateCommandBase, ICommand, INotifyPropertyChanged
    {
        protected readonly ErrorHandlerRegistry _registry = new ErrorHandlerRegistry();

        private bool _isExecuting;
        public bool IsExecuting
        {
            get => _isExecuting;
            set => SetProperty(ref _isExecuting, value, OnIsExecutingChanged);
        }

        private void OnIsExecutingChanged()
        {
            onIsExecutingDelegate?.Invoke(IsExecuting);
        }

        internal Action<bool> onIsExecutingDelegate;

        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T storage, T value, Action onChange, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value)) return false;

            storage = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            onChange();

            return true;
        }
    }

    public static class ExecutionAwareCommandExtensions
    {
        public static T OnIsExecutingChanged<T>(this T command, Action<bool> delegateAction)
            where T : ExecutionAwareCommandBase
        {
            command.onIsExecutingDelegate = delegateAction;
            return command;
        }
    }
}
