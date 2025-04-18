using System;
using System.Collections.Generic;

namespace Tek.Contract
{
    public class AssertPartition : Query<bool>
    {
        public int PartitionNumber { get; set; }
    }

    public class FetchPartition : Query<PartitionModel>
    {
        public int PartitionNumber { get; set; }
    }

    public class CollectPartitions : Query<IEnumerable<PartitionModel>>, IPartitionCriteria
    {
        public string PartitionEmail { get; set; }
        public string PartitionHost { get; set; }
        public string PartitionName { get; set; }
        public string PartitionSettings { get; set; }
        public string PartitionSlug { get; set; }
        public string PartitionTesters { get; set; }

        public DateTime ModifiedWhen { get; set; }
    }

    public class CountPartitions : Query<int>, IPartitionCriteria
    {
        public string PartitionEmail { get; set; }
        public string PartitionHost { get; set; }
        public string PartitionName { get; set; }
        public string PartitionSettings { get; set; }
        public string PartitionSlug { get; set; }
        public string PartitionTesters { get; set; }

        public DateTime ModifiedWhen { get; set; }
    }

    public class SearchPartitions : Query<IEnumerable<PartitionMatch>>, IPartitionCriteria
    {
        public string PartitionEmail { get; set; }
        public string PartitionHost { get; set; }
        public string PartitionName { get; set; }
        public string PartitionSettings { get; set; }
        public string PartitionSlug { get; set; }
        public string PartitionTesters { get; set; }

        public DateTime ModifiedWhen { get; set; }
    }

    public interface IPartitionCriteria
    {
        Filter Filter { get; set; }
        
        string PartitionEmail { get; set; }
        string PartitionHost { get; set; }
        string PartitionName { get; set; }
        string PartitionSettings { get; set; }
        string PartitionSlug { get; set; }
        string PartitionTesters { get; set; }

        DateTime ModifiedWhen { get; set; }
    }

    public partial class PartitionMatch
    {
        public int PartitionNumber { get; set; }
    }

    public partial class PartitionModel
    {
        public string PartitionEmail { get; set; }
        public string PartitionHost { get; set; }
        public string PartitionName { get; set; }
        public string PartitionSettings { get; set; }
        public string PartitionSlug { get; set; }
        public string PartitionTesters { get; set; }

        public int PartitionNumber { get; set; }

        public DateTime ModifiedWhen { get; set; }
    }
}