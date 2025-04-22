namespace Tek.Service.Security;

public partial class TResourceEntity
{
    public Guid ResourceId { get; set; }

    public string ResourceName { get; set; } = null!;
    public string ResourceType { get; set; } = null!;
}