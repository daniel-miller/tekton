using System;

namespace Tek.Base.Timeline
{
    public interface IAggregateTypeInfo
    {
        Guid ID { get; }
        Type Type { get; }
        string Name { get; }

        IEventTypeInfo[] Events { get; }
    }
}
