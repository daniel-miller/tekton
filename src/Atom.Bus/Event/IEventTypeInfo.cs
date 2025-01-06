using System;

namespace Atom.Bus
{
    public interface IEventTypeInfo
    {
        Guid ID { get; }
        Type Type { get; }
        IAggregateTypeInfo Aggregate { get; }
    }
}
