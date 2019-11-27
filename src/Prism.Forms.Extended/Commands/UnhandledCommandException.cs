using System;

namespace Prism.Commands
{
    public class UnhandledCommandException : Exception
    {
        internal UnhandledCommandException(Exception innerException)
            : base("No Handler was found to handle an exception encountered by the Command", innerException)
        {
        }
    }
}
