using System;

namespace Tek.Bus
{
    /// <summary>
    /// Provides a serialization wrapper for aggregates so we can use Entity Framework for basic DAL operations.
    /// </summary>
    public class SerializedAggregate
    {
        public string AggregateClass { get; set; }
        public DateTimeOffset? AggregateExpires { get; set; }
        public Guid AggregateIdentifier { get; set; }
        public string AggregateType { get; set; }
        public int OriginShard { get; set; }
        public Guid RootAggregateIdentifier { get; set; }
    }
}
