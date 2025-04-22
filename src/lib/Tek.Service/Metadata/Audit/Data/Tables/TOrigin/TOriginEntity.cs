namespace Tek.Service.Metadata;

public partial class TOriginEntity
{
    public Guid OrganizationId { get; set; }
    public Guid OriginId { get; set; }
    public Guid? ProxyAgent { get; set; }
    public Guid? ProxySubject { get; set; }
    public Guid UserId { get; set; }

    public string? OriginDescription { get; set; }
    public string? OriginReason { get; set; }
    public string? OriginSource { get; set; }

    public DateTime OriginWhen { get; set; }
}