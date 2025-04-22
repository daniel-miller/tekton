namespace Tek.Service.Security;

public partial class TOrganizationEntity
{
    public Guid OrganizationId { get; set; }

    public string OrganizationName { get; set; } = null!;
    public string OrganizationSlug { get; set; } = null!;

    public int OrganizationNumber { get; set; }
    public int PartitionNumber { get; set; }

    public DateTime ModifiedWhen { get; set; }
}