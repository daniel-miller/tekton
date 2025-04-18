using System;
using System.Collections.Generic;

namespace Tek.Contract
{
    public class AssertRole : Query<bool>
    {
        public Guid RoleId { get; set; }
    }

    public class FetchRole : Query<RoleModel>
    {
        public Guid RoleId { get; set; }
    }

    public class CollectRoles : Query<IEnumerable<RoleModel>>, IRoleCriteria
    {
        public string RoleName { get; set; }
        public string RoleType { get; set; }
    }

    public class CountRoles : Query<int>, IRoleCriteria
    {
        public string RoleName { get; set; }
        public string RoleType { get; set; }
    }

    public class SearchRoles : Query<IEnumerable<RoleMatch>>, IRoleCriteria
    {
        public string RoleName { get; set; }
        public string RoleType { get; set; }
    }

    public interface IRoleCriteria
    {
        Filter Filter { get; set; }
        
        string RoleName { get; set; }
        string RoleType { get; set; }
    }

    public partial class RoleMatch
    {
        public Guid RoleId { get; set; }
    }

    public partial class RoleModel
    {
        public Guid RoleId { get; set; }

        public string RoleName { get; set; }
        public string RoleType { get; set; }
    }
}