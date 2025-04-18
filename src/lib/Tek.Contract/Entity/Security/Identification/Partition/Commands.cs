using System;

namespace Tek.Contract
{
    public class CreatePartition
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

    public class ModifyPartition
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

    public class DeletePartition
    {
        public int PartitionNumber { get; set; }
    }
}