using System;
using System.Collections.Generic;

namespace Tek.Contract
{
    public class AssertPermission : Query<bool>
    {
        public Guid PermissionId { get; set; }
    }

    public class FetchPermission : Query<PermissionModel>
    {
        public Guid PermissionId { get; set; }
    }

    public class CollectPermissions : Query<IEnumerable<PermissionModel>>, IPermissionCriteria
    {
        public Guid ResourceId { get; set; }
        public Guid RoleId { get; set; }

        public string AccessType { get; set; }

        public int AccessFlags { get; set; }
    }

    public class CountPermissions : Query<int>, IPermissionCriteria
    {
        public Guid ResourceId { get; set; }
        public Guid RoleId { get; set; }

        public string AccessType { get; set; }

        public int AccessFlags { get; set; }
    }

    public class SearchPermissions : Query<IEnumerable<PermissionMatch>>, IPermissionCriteria
    {
        public Guid ResourceId { get; set; }
        public Guid RoleId { get; set; }

        public string AccessType { get; set; }

        public int AccessFlags { get; set; }
    }

    public interface IPermissionCriteria
    {
        Filter Filter { get; set; }
        
        Guid ResourceId { get; set; }
        Guid RoleId { get; set; }

        string AccessType { get; set; }

        int AccessFlags { get; set; }
    }

    public partial class PermissionMatch
    {
        public Guid PermissionId { get; set; }
    }

    public partial class PermissionModel
    {
        public Guid PermissionId { get; set; }
        public Guid ResourceId { get; set; }
        public Guid RoleId { get; set; }

        public string AccessType { get; set; }

        public int AccessFlags { get; set; }
    }
}