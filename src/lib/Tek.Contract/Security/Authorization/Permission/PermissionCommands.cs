using System;

namespace Tek.Contract
{
    public class CreatePermission
    {
        public Guid PermissionId { get; set; }
        public Guid ResourceId { get; set; }
        public Guid RoleId { get; set; }

        public string AccessType { get; set; }

        public int AccessFlags { get; set; }
    }

    public class ModifyPermission
    {
        public Guid PermissionId { get; set; }
        public Guid ResourceId { get; set; }
        public Guid RoleId { get; set; }

        public string AccessType { get; set; }

        public int AccessFlags { get; set; }
    }

    public class DeletePermission
    {
        public Guid PermissionId { get; set; }
    }
}