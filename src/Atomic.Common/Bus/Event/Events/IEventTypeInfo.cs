using System;

namespace Atomic.Common.Bus
{
    public interface IEventTypeInfo
    {
        Guid ID { get; }
        Type Type { get; }
        IAggregateTypeInfo Aggregate { get; }
    }
}
