using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Xunit;

namespace Prism.Forms.Extended.Tests
{
    public class ExecutionAwareCommandTests
    {
        [Fact]
        public void IsExecutingIsChanged()
        {
            bool didExecute = false;
            bool didCommandIsExecutingChange = false;
            ICommand command = ExecutionAwareCommand.FromAction(() => didExecute = true)
                .OnIsExecutingChanged(_ => didCommandIsExecutingChange = true);

            command.Execute(null);

            Assert.True(didExecute);
            Assert.True(didCommandIsExecutingChange);
        }

        [Fact]
        public void CatchesException()
        {
            Exception caughtException = null;

            ICommand command = ExecutionAwareCommand.FromAction(() => throw new Exception("Test"))
                .Catch(e => caughtException = e);

            var ex = Record.Exception(() => command.Execute(null));

            Assert.Null(ex);
            Assert.NotNull(caughtException);
            Assert.Equal("Test", caughtException.Message);
        }

        [Fact]
        public void ThrowsOnInvalidCast()
        {
            int number = 0;
            ICommand command = ExecutionAwareCommand.FromAction<int>(i => number = i)
                .SetThrowOnInvalidCast(true);

            var ex = Record.Exception(() => command.Execute("Hello World"));

            Assert.NotNull(ex);
            Assert.IsType<UnhandledCommandException>(ex);
            Assert.IsType<InvalidCastException>(ex.InnerException);
        }

        [Fact]
        public void SuppressesInvalidCast()
        {
            int number = 1;
            ICommand command = ExecutionAwareCommand.FromAction<int>(i => number = i)
                .SetThrowOnInvalidCast(false);

            var ex = Record.Exception(() => command.Execute("Hello World"));

            Assert.Null(ex);
            Assert.Equal(0, number);
        }

        [Fact]
        public void ProperlyCatchesThrownInvalidCast()
        {
            int number = 0;
            Exception caughtException = null;
            ICommand command = ExecutionAwareCommand.FromAction<int>(i => number = i)
                .SetThrowOnInvalidCast(true)
                .Catch(e => caughtException = e);

            var ex = Record.Exception(() => command.Execute("Hello World"));

            Assert.Null(ex);
            Assert.NotNull(caughtException);
            Assert.IsType<InvalidCastException>(caughtException);
        }

        [Fact]
        public async Task IsExecutingChangedWhileTaskCompletes()
        {
            bool isExecuting = false;
            var delay = Task.Delay(1000);

            var propertiesChanged = new List<string>();
            ExecutionAwareCommand command = null;
            var source = new TaskCompletionSource<bool>();
            command = ExecutionAwareCommand.FromTask(async () =>
            {
                isExecuting = true;
                await delay;
                source.SetResult(true);
            });

            Assert.Null(command.ExecuteDelegate);
            Assert.NotNull(command.AsyncExecuteDelegate);

            void PropertyChanged(object sender, PropertyChangedEventArgs args)
            {
                propertiesChanged.Add(args.PropertyName);
            }

            command.PropertyChanged += PropertyChanged;

            ((ICommand)command).Execute(null);
            Assert.True(command.IsExecuting);
            Assert.True(isExecuting);
            await source.Task;

            // add some extra delay to ensure that source task has fully completed including the Completion Task.
            await Task.Delay(200);
            Assert.False(command.IsExecuting);

            command.PropertyChanged -= PropertyChanged;

            Assert.Equal(2, propertiesChanged.Where(x => x == "IsExecuting").Count());
        }

        [Fact]
        public async Task CatchesUnhandledExceptionFromTask()
        {
            var delay = Task.Delay(200);

            string message = null;
            ExecutionAwareCommand command = null;
            var source = new TaskCompletionSource<bool>();
            command = ExecutionAwareCommand.FromTask(async () =>
            {
                await delay;
                source.SetResult(true);
                throw new Exception("Test");
            })
                .Catch(e => message = e.Message);

            var ex = Record.Exception(() => ((ICommand)command).Execute(null));
            await source.Task;

            // add some extra delay to ensure that source task has fully completed including the Completion Task.
            await Task.Delay(200);
            Assert.False(command.IsExecuting);

            Assert.Null(ex);
            Assert.Equal("Test", message);
        }
    }
}
