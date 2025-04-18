namespace Tek.Service.Security;

public partial class TPartitionEntity
{
    public string PartitionEmail { get; set; } = null!;
    public string PartitionHost { get; set; } = null!;
    public string PartitionName { get; set; } = null!;
    public string? PartitionSettings { get; set; }
    public string PartitionSlug { get; set; } = null!;
    public string? PartitionTesters { get; set; }

    public int PartitionNumber { get; set; }

    public DateTime ModifiedWhen { get; set; }
}