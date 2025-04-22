using System;

using Tek.Base;

namespace Tek.Base.Timeline
{
    public class EventTypeInfo : IEventTypeInfo
    {
        public Guid ID { get; }
        public Type Type { get; }
        public IAggregateTypeInfo Aggregate { get; }

        public EventTypeInfo(Type t, IAggregateTypeInfo agg)
        {
            ID = UuidFactory.Create();
            Type = t;
            Aggregate = agg;
        }

        public override string ToString() => Type.Name;
    }
}
