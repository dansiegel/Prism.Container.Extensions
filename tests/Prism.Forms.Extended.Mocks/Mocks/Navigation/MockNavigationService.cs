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

        public Task<INavigationResult> GoBackAsync(INavigationParameters parameters, bool? useModalNavigation, bool animated)
        {
            if (parameters is null)
            {
                parameters = new NavigationParameters();
            }
            if (useModalNavigation.HasValue)
            {
                parameters.Add(KnownNavigationParameters.UseModalNavigation, useModalNavigation);
            }
            return GoBackAsync(parameters);
        }

        public Task<INavigationResult> GoBackToRootAsync(INavigationParameters parameters)
        {
            return GoBackAsync(parameters);
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

        public Task<INavigationResult> NavigateAsync(string name, INavigationParameters parameters, bool? useModalNavigation, bool animated)
        {
            if (parameters is null)
            {
                parameters = new NavigationParameters();
            }
            if (useModalNavigation.HasValue)
            {
                parameters.Add(KnownNavigationParameters.UseModalNavigation, useModalNavigation);
            }
            return NavigateAsync(name, parameters);
        }

        public Task<INavigationResult> NavigateAsync(Uri uri, INavigationParameters parameters, bool? useModalNavigation, bool animated)
        {
            if (parameters is null)
            {
                parameters = new NavigationParameters();
            }
            if (useModalNavigation.HasValue)
            {
                parameters.Add(KnownNavigationParameters.UseModalNavigation, useModalNavigation);
            }
            return NavigateAsync(uri, parameters);
        }
    }

    public class MockNavigationResult : INavigationResult
    {
        public bool Success { get; set; }
        public Exception Exception { get; set; }
    }
}
