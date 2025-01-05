using System;

namespace Atomic.Common.Bus
{
    public interface IAggregateTypeInfo
    {
        Guid ID { get; }
        Type Type { get; }
        string Name { get; }

        IEventTypeInfo[] Events { get; }
    }
}
