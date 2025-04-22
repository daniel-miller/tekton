namespace Tek.Service.Security;

public partial class TRoleEntity
{
    public Guid RoleId { get; set; }

    public string RoleName { get; set; } = null!;
    public string RoleType { get; set; } = null!;
}