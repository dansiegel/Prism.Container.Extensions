using System;
using System.ComponentModel;
using Prism.Container.Extensions;
using Prism.Ioc;

namespace Prism.DryIoc
{
    [AttributeUsage(
            AttributeTargets.Assembly
            | AttributeTargets.Class
            | AttributeTargets.Struct
            | AttributeTargets.Enum
            | AttributeTargets.Constructor
            | AttributeTargets.Method
            | AttributeTargets.Property
            | AttributeTargets.Field
            | AttributeTargets.Event
            | AttributeTargets.Interface
            | AttributeTargets.Delegate,
            AllowMultiple = true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class PreserveAttribute : Attribute
    {
        public bool AllMembers { get; }
        public bool Conditional { get; }

        public PreserveAttribute(bool allMembers, bool conditional)
        {
            AllMembers = allMembers;
            Conditional = conditional;
        }

        public PreserveAttribute()
            : this(true, false)
        {
        }

        public PreserveAttribute(Type referenceType)
            : this(true, false)
        {
            if (typeof(IContainerExtension).IsAssignableFrom(referenceType))
            {
                ContainerLocator.LocatePreservedReference(referenceType);
            }
        }
    }
}
