using System;
using System.Threading.Tasks;
using Prism.Behaviors;
using Prism.Common;
using Prism.DryIoc.Events;
using Prism.Events;
using Prism.Ioc;
using Prism.Logging;
using Prism.Navigation;

namespace Prism.DryIoc.Navigation
{
    public class ErrorReportingNavigationService : PageNavigationService
    {
        private IEventAggregator EventAggregator { get; }

        public ErrorReportingNavigationService(IContainerExtension container,
                                               IApplicationProvider applicationProvider,
                                               IPageBehaviorFactory pageBehaviorFactory,
                                               ILoggerFacade logger,
                                               IEventAggregator eventAggregator)
            : base(container, applicationProvider, pageBehaviorFactory, logger)
        {
            EventAggregator = eventAggregator;
        }

        protected async override Task<INavigationResult> GoBackInternal(INavigationParameters parameters, bool? useModalNavigation, bool animated)
        {
            var result = await base.GoBackInternal(parameters, useModalNavigation, animated);

            if(result.Exception != null)
            {
                EventAggregator.GetEvent<NavigationErrorEvent>().Publish(new NavigationError
                {
                    Exception = result.Exception,
                    Parameters = parameters,
                    NavigationUri = nameof(GoBackAsync)
                });
            }

            return result;
        }

        protected override async Task<INavigationResult> GoBackToRootInternal(INavigationParameters parameters)
        {
            var result = await  base.GoBackToRootInternal(parameters);

            if (result.Exception != null)
            {
                EventAggregator.GetEvent<NavigationErrorEvent>().Publish(new NavigationError
                {
                    Exception = result.Exception,
                    Parameters = parameters,
                    NavigationUri = nameof(INavigationServiceExtensions.GoBackToRootAsync)
                });
            }

            return result;
        }

        protected override async Task<INavigationResult> NavigateInternal(Uri uri, INavigationParameters parameters, bool? useModalNavigation, bool animated)
        {
            var result = await base.NavigateInternal(uri, parameters, useModalNavigation, animated);

            if (result.Exception != null)
            {
                EventAggregator.GetEvent<NavigationErrorEvent>().Publish(
                    new NavigationError
                    {
                        Exception = result.Exception,
                        Parameters = parameters,
                        NavigationUri = uri.ToString()
                    });
            }

            return result;
        }
    }
}
