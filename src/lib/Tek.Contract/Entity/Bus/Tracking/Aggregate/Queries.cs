using System;
using System.Collections.Generic;

namespace Tek.Contract
{
    public class AssertAggregate : Query<bool>
    {
        public Guid AggregateId { get; set; }
    }

    public class FetchAggregate : Query<AggregateModel>
    {
        public Guid AggregateId { get; set; }
    }

    public class CollectAggregates : Query<IEnumerable<AggregateModel>>, IAggregateCriteria
    {
        public Guid AggregateRoot { get; set; }

        public string AggregateType { get; set; }
    }

    public class CountAggregates : Query<int>, IAggregateCriteria
    {
        public Guid AggregateRoot { get; set; }

        public string AggregateType { get; set; }
    }

    public class SearchAggregates : Query<IEnumerable<AggregateMatch>>, IAggregateCriteria
    {
        public Guid AggregateRoot { get; set; }

        public string AggregateType { get; set; }
    }

    public interface IAggregateCriteria
    {
        Filter Filter { get; set; }
        
        Guid AggregateRoot { get; set; }

        string AggregateType { get; set; }
    }

    public partial class AggregateMatch
    {
        public Guid AggregateId { get; set; }
    }

    public partial class AggregateModel
    {
        public Guid AggregateId { get; set; }
        public Guid AggregateRoot { get; set; }

        public string AggregateType { get; set; }
    }
}