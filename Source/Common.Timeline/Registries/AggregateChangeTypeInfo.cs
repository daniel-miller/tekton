using System;

using Common.Timeline.Assistants;

namespace Common.Timeline.Registries
{
    public class AggregateChangeTypeInfo : IAggregateChangeTypeInfo
    {
        public Guid ID { get; }
        public Type Type { get; }
        public IAggregateTypeInfo Aggregate { get; }

        public AggregateChangeTypeInfo(Type t, IAggregateTypeInfo agg)
        {
            ID = GuidGenerator.NewGuid(t);
            Type = t;
            Aggregate = agg;
        }

        public override string ToString() => Type.Name;
    }
}
