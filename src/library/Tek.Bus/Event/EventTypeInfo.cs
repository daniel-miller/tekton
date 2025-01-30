using System;

using Tek.Common;

namespace Tek.Bus
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
