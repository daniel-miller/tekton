using System;

namespace Tek.Bus
{
    public interface IEventTypeInfo
    {
        Guid ID { get; }
        Type Type { get; }
        IAggregateTypeInfo Aggregate { get; }
    }
}
