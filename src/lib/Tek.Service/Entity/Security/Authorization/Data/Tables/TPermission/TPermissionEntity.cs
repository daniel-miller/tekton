namespace Tek.Service.Security;

public partial class TPermissionEntity
{
    public Guid PermissionId { get; set; }
    public Guid ResourceId { get; set; }
    public Guid RoleId { get; set; }

    public string AccessType { get; set; } = null!;

    public int AccessFlags { get; set; }
}