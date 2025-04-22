using System;

namespace Tek.Base.Timeline
{
    public interface IEventTypeInfo
    {
        Guid ID { get; }
        Type Type { get; }
        IAggregateTypeInfo Aggregate { get; }
    }
}
