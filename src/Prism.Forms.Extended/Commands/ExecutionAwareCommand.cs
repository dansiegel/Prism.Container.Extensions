using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Prism.Forms.Extended.Common;

namespace Prism.Commands
{
    public class ExecutionAwareCommand : ExecutionAwareCommandBase
    {
        internal ExecutionAwareCommand() { }

        internal Func<bool> CanExecuteDelegate { get; set; }

        internal Action ExecuteDelegate { get; set; }

        internal Func<Task> AsyncExecuteDelegate { get; set; }

        protected override bool CanExecute(object parameter)
        {
            try
            {
                return CanExecuteDelegate?.Invoke() ?? true;
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

                if(AsyncExecuteDelegate is null)
                {
                    ExecuteDelegate();
                }
                else
                {
                    var task = AsyncExecuteDelegate();
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
                if(!_registry.HandledException(ex))
                {
                    throw new UnhandledCommandException(ex);
                }
            }
            finally
            {
                if(AsyncExecuteDelegate is null)
                {
                    IsExecuting = false;
                }
            }
        }

        public ExecutionAwareCommand ObservesProperty<T>(Expression<Func<T>> propertyExpression)
        {
            ObservesPropertyInternal(propertyExpression);
            return this;
        }

        public ExecutionAwareCommand ObservesCanExecute(Expression<Func<bool>> canExecuteExpression)
        {
            CanExecuteDelegate = canExecuteExpression.Compile();
            ObservesPropertyInternal(canExecuteExpression);
            return this;
        }

        public ExecutionAwareCommand Catch(Action<Exception> handler)
        {
            _registry.CatchAll = handler;
            return this;
        }

        public ExecutionAwareCommand Catch<T>(Action<T> handler)
            where T : Exception
        {
            _registry.Add(typeof(T), e => handler((T)e));
            return this;
        }

        public static ExecutionAwareCommand FromAction(Action executionDelegate, Func<bool> canExecuteDelegate = null)
        {
            return new ExecutionAwareCommand
            {
                ExecuteDelegate = executionDelegate,
                CanExecuteDelegate = canExecuteDelegate
            };
        }

        public static ExecutionAwareCommand FromTask(Func<Task> executionDelegate, Func<bool> canExecuteDelegate = null)
        {
            return new ExecutionAwareCommand
            {
                AsyncExecuteDelegate = executionDelegate,
                CanExecuteDelegate = canExecuteDelegate
            };
        }

        public static ExecutionAwareCommand<T> FromAction<T>(Action<T> executionDelegate, Func<T, bool> canExecuteDelegate = null)
        {
            return new ExecutionAwareCommand<T>
            {
                ExecuteDelegate = executionDelegate,
                CanExecuteDelegate = canExecuteDelegate
            };
        }

        public static ExecutionAwareCommand<T> FromTask<T>(Func<T, Task> executionDelegate, Func<T, bool> canExecuteDelegate = null)
        {
            return new ExecutionAwareCommand<T>
            {
                AsyncExecuteDelegate = executionDelegate,
                CanExecuteDelegate = canExecuteDelegate
            };
        }
    }
}
