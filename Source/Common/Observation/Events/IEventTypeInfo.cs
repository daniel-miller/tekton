using System;

namespace Common.Observation
{
    public interface IEventTypeInfo
    {
        Guid ID { get; }
        Type Type { get; }
        IAggregateTypeInfo Aggregate { get; }
    }
}
