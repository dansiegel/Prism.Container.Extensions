Have you ever looked at ReactiveCommand and wished that the DelegateCommand could be more like that, while also wishing the ReactiveCommand could be more like the DelegateCommand. This was the reason that yet another command was introduced in the Prism.Forms.Extended package. The ExecutionAwareCommand solves a few problems.

1. Fluent API
1. Support for Async Delegates
1. Support for ObservesProperty
1. Support for ObservesCanExecute
1. Support for adding Exception Handlers directly on the Command
1. Support for adding a IsExecuting Changed delegate
1. Support for opting in or out of InvalidCastExceptions

## Using the ExecutionAwareCommand

```c#
public class ViewAViewModel : BindableBase
{
    public ViewAViewModel()
    {
        FooCommand = ExecutionAwareCommand.FromAction(DoFoo)
                            .OnIsExecutingChanged(b => IsBusy = b)
                            .Catch(e => {
                                // equivalent to catch(Exception e)
                            })
                            .Catch<NullReferenceException>(e => {
                                // e is strongly typed as NullReferenceException
                            });
    }

    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }

    public ICommand FooCommand { get; }

    private void DoFoo()
    {
        // Do Foo
    }
}
```

!!! note "Note"
    Both `ExecutionAwareCommand` and `ExecutionAwareCommand<T>` only support creation from the factory methods:

      - FromAction
      - FromTask