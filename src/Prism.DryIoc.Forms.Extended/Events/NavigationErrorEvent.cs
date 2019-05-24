using System;
using Prism.Events;
using Prism.Navigation;

namespace Prism.DryIoc.Events
{
    public class NavigationErrorEvent : PubSubEvent<INavigationError>
    {
    }

    public interface INavigationError
    {
        string NavigationUri { get; }
        INavigationParameters Parameters { get; }
        Exception Exception { get; }
    }

    internal class NavigationError : INavigationError
    {
        public string NavigationUri { get; set; }
        public INavigationParameters Parameters { get; set; }
        public Exception Exception { get; set; }
    }
}
