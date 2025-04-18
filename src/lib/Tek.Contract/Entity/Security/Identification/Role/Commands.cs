using System;

namespace Tek.Contract
{
    public class CreateRole
    {
        public Guid RoleId { get; set; }

        public string RoleName { get; set; }
        public string RoleType { get; set; }
    }

    public class ModifyRole
    {
        public Guid RoleId { get; set; }

        public string RoleName { get; set; }
        public string RoleType { get; set; }
    }

    public class DeleteRole
    {
        public Guid RoleId { get; set; }
    }
}