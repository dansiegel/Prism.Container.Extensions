using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prism.Common;
using Prism.Forms.Extended.Common;
using Prism.Navigation;

namespace Prism.Forms.Extended.Navigation
{
    public class NavigationBuilder
    {
        internal const string RemovePageRelativePath = "../";
        internal const string RemovePageInstruction = "__RemovePage/";
        internal const string RemovePageSegment = "__RemovePage";
        private readonly ErrorHandlerRegistry _errorHandlers = new ErrorHandlerRegistry();

        internal NavigationBuilder(INavigationService navigationService, NavigationInstruction instruction)
        {
            _navigationService = navigationService;
            _instruction = instruction;
        }

        internal NavigationBuilder(INavigationService navigationService, string name)
            : this(navigationService, NavigationInstruction.Navigate)
        {
            if (name.StartsWith(RemovePageRelativePath))
                name = name.Replace(RemovePageRelativePath, RemovePageInstruction);

            _uri = UriParsingHelper.Parse(name);
        }

        internal NavigationBuilder(INavigationService navigationService, Uri uri)
            : this(navigationService, NavigationInstruction.Navigate)
        {
            _uri = uri;
        }

        private INavigationService _navigationService { get; }
        private NavigationInstruction _instruction { get; }
        private Uri _uri { get; }
        private INavigationParameters _parameters;

        private bool? _modalNavigation;
        private bool _animated = true;

        public NavigationBuilder WithParameters(INavigationParameters parameters)
        {
            _parameters = parameters;
            return this;
        }

        public NavigationBuilder UseModalNavigation(bool value = true)
        {
            _modalNavigation = value;
            return this;
        }

        public NavigationBuilder WithAnimated(bool value)
        {
            _animated = value;
            return this;
        }

        public NavigationBuilder WithParameters(params (string key, object value)[] parameters)
        {
            if (_parameters is null)
            {
                _parameters = new NavigationParameters();
            }

            foreach ((var key, var value) in parameters)
            {
                _parameters.Add(key, value);
            }

            return this;
        }

        public NavigationBuilder WithParameter(string key, object value) =>
            WithParameters((key, value));

        public NavigationBuilder Catch(Action<Exception> action) =>
            Catch<Exception>(action);

        public NavigationBuilder Catch<T>(Action<T> action)
            where T : Exception
        {
            _errorHandlers.Add(typeof(T), e => action((T)e));
            return this;
        }

        public async Task<bool> ExecuteAsync()
        {
            INavigationResult result;
            switch (_instruction)
            {
                case NavigationInstruction.GoBack:
                    result = await _navigationService.GoBackAsync(_parameters);
                    break;
                case NavigationInstruction.GoBackToRoot:
                    result = await _navigationService.GoBackToRootAsync(_parameters);
                    break;
                default:
                    result = await _navigationService.NavigateAsync(_uri, _parameters, _modalNavigation, _animated);
                    break;
            }

            _errorHandlers.HandledException(result.Exception);

            return result.Success;
        }
    }
}
