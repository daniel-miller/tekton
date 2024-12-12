using System;

namespace Common.Observation
{
    public interface IAggregateTypeInfo
    {
        Guid ID { get; }
        Type Type { get; }
        string Name { get; }

        IEventTypeInfo[] Events { get; }
    }
}
