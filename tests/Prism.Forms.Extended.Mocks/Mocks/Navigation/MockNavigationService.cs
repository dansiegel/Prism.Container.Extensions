using System;
using System.Threading.Tasks;
using Prism.Navigation;

namespace Prism.Forms.Extended.Mocks.Navigation
{
    internal class MockNavigationService : INavigationService
    {
        public bool GoBackCalled { get; private set; }
        public bool GoBackToRootCalled { get; private set; }
        public bool NavigateCalled { get; private set; }
        public INavigationParameters ParametersUsed { get; private set; }
        public string NavigationName { get; private set; }
        public Uri NavigationUri { get; private set; }
        public INavigationResult Result { get; set; }

        public Task<INavigationResult> GoBackAsync() => GoBackAsync(null);

        public Task<INavigationResult> GoBackAsync(INavigationParameters parameters)
        {
            GoBackCalled = true;
            ParametersUsed = parameters;
            return Task.FromResult(Result);
        }

        public Task<INavigationResult> NavigateAsync(Uri uri) => NavigateAsync(uri, null);

        public Task<INavigationResult> NavigateAsync(Uri uri, INavigationParameters parameters)
        {
            NavigateCalled = true;
            NavigationUri = uri;
            ParametersUsed = parameters;
            return Task.FromResult(Result);
        }

        public Task<INavigationResult> NavigateAsync(string name) => NavigateAsync(name, null);

        public Task<INavigationResult> NavigateAsync(string name, INavigationParameters parameters)
        {
            NavigateCalled = true;
            NavigationName = name;
            ParametersUsed = parameters;
            return Task.FromResult(Result);
        }
    }

    public class MockNavigationResult : INavigationResult
    {
        public bool Success { get; set; }
        public Exception Exception { get; set; }
    }
}
