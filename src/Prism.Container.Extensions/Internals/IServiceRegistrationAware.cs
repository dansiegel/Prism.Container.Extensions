using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace Prism.Container.Extensions.Internals
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IServiceRegistrationAware
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        IEnumerable<IServiceCollection> GetServiceRegistrations();
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public struct Registration
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Registration(Type serviceType, Type implementingType, string name)
        {
            ServiceType = serviceType;
            ImplementingType = implementingType;
            Name = name;
        }

        public Type ServiceType { get; }
        public Type ImplementingType { get; }
        public string Name { get; }

        public override bool Equals(object obj) =>
            obj is Registration reg && reg.ServiceType == ServiceType && reg.Name == Name;

        public override int GetHashCode() => ServiceType.GetHashCode() + (int)Name?.GetHashCode();

        public static bool operator ==(Registration left, Registration right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Registration left, Registration right)
        {
            return !(left == right);
        }
    }
}
