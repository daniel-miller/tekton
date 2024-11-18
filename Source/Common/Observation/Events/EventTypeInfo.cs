using System;

namespace Common.Observation
{
    public class EventTypeInfo : IEventTypeInfo
    {
        public Guid ID { get; }
        public Type Type { get; }
        public IAggregateTypeInfo Aggregate { get; }

        public EventTypeInfo(Type t, IAggregateTypeInfo agg)
        {
            ID = GuidGenerator.NewGuid();
            Type = t;
            Aggregate = agg;
        }

        public override string ToString() => Type.Name;
    }
}
