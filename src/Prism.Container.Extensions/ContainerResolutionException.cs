using System;

namespace Prism.Ioc
{
    public class ContainerResolutionException : Exception
    {
        public ContainerResolutionException(Type serviceType, Exception innerException)
            : this(serviceType, null, innerException)
        {
        }

        public ContainerResolutionException(Type serviceType, string serviceName, Exception innerException)
            : base(GetErrorMessage(serviceType, serviceName), innerException)
        {
            ServiceType = serviceType;
            ServiceName = serviceName;
        }

        public Type ServiceType { get; }

        public string ServiceName { get; }

        private static string GetErrorMessage(Type type, string name)
        {
            var message = $"An unexpected error occurred while resolving '{type.FullName}'";
            if (!string.IsNullOrEmpty(name))
                message += $", with the service name '{name}'";

            return message;
        }
    }
}
