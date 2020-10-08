using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Forms.Extended.Common;

namespace Prism.Commands
{
    public class ExecutionAwareCommand<T> : ExecutionAwareCommandBase
    {
        internal ExecutionAwareCommand()
        {
        }

        public bool ThrowOnInvalidCast { get; private set; }

        internal Func<T, bool> CanExecuteDelegate { get; set; }

        internal Action<T> ExecuteDelegate { get; set; }

        internal Func<T, Task> AsyncExecuteDelegate { get; set; }

        protected override bool CanExecute(object parameter)
        {
            try
            {
                T param = default;
                if (parameter is T TParam)
                {
                    param = TParam;
                }
                else if (parameter != null && ThrowOnInvalidCast)
                {
                    throw new InvalidCastException($"The Command expected a typeof '{typeof(T).FullName}' but recieved a parameter of type '{parameter.GetType().FullName}");
                }

                return CanExecuteDelegate?.Invoke(param) ?? true;
            }
            catch (Exception ex)
            {
                _registry.HandledException(ex);
                return false;
            }
        }

        protected override void Execute(object parameter)
        {
            try
            {
                IsExecuting = true;

                T param = default;
                if (parameter is T TParam)
                {
                    param = TParam;
                }
                else if (parameter != null && ThrowOnInvalidCast)
                {
                    throw new InvalidCastException($"The Command expected a typeof '{typeof(T).FullName}' but recieved a parameter of type '{parameter.GetType().FullName}");
                }

                if (AsyncExecuteDelegate is null)
                {
                    ExecuteDelegate(param);
                }
                else
                {
                    var task = AsyncExecuteDelegate(param);
                    task.ContinueWith(t =>
                    {
                        IsExecuting = false;
                        if (!_registry.HandledException(t.Exception))
                        {
                            throw new UnhandledCommandException(t.Exception);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                if (!_registry.HandledException(ex))
                {
                    throw new UnhandledCommandException(ex);
                }
            }
            finally
            {
                if (AsyncExecuteDelegate is null)
                {
                    IsExecuting = false;
                }
            }
        }

        public ExecutionAwareCommand<T> SetThrowOnInvalidCast(bool shouldThrow)
        {
            ThrowOnInvalidCast = shouldThrow;
            return this;
        }

        public ExecutionAwareCommand<T> ObservesProperty<TType>(Expression<Func<TType>> propertyExpression)
        {
            ObservesPropertyInternal(propertyExpression);
            return this;
        }

        public ExecutionAwareCommand<T> ObservesCanExecute(Expression<Func<bool>> canExecuteExpression)
        {
            Expression<Func<T, bool>> expression = Expression.Lambda<Func<T, bool>>(canExecuteExpression.Body, Expression.Parameter(typeof(T), "o"));
            CanExecuteDelegate = expression.Compile();
            ObservesPropertyInternal(canExecuteExpression);
            return this;
        }

        public ExecutionAwareCommand<T> Catch(Action<Exception> handler)
        {
            _registry.CatchAll = handler;
            return this;
        }

        public ExecutionAwareCommand<T> Catch<TException>(Action<TException> handler)
            where TException : Exception
        {
            _registry.Add(typeof(T), e => handler((TException)e));
            return this;
        }
    }
}
