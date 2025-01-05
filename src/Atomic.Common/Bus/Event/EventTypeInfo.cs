using System;

namespace Atomic.Common.Bus
{
    public class EventTypeInfo : IEventTypeInfo
    {
        public Guid ID { get; }
        public Type Type { get; }
        public IAggregateTypeInfo Aggregate { get; }

        public EventTypeInfo(Type t, IAggregateTypeInfo agg)
        {
            ID = GuidFactory.Create();
            Type = t;
            Aggregate = agg;
        }

        public override string ToString() => Type.Name;
    }
}
