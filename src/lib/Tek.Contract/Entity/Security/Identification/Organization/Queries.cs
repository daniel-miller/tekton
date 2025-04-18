using System;
using System.Collections.Generic;

namespace Tek.Contract
{
    public class AssertOrganization : Query<bool>
    {
        public Guid OrganizationId { get; set; }
    }

    public class FetchOrganization : Query<OrganizationModel>
    {
        public Guid OrganizationId { get; set; }
    }

    public class CollectOrganizations : Query<IEnumerable<OrganizationModel>>, IOrganizationCriteria
    {
        public string OrganizationName { get; set; }
        public string OrganizationSlug { get; set; }

        public int OrganizationNumber { get; set; }
        public int PartitionNumber { get; set; }

        public DateTime ModifiedWhen { get; set; }
    }

    public class CountOrganizations : Query<int>, IOrganizationCriteria
    {
        public string OrganizationName { get; set; }
        public string OrganizationSlug { get; set; }

        public int OrganizationNumber { get; set; }
        public int PartitionNumber { get; set; }

        public DateTime ModifiedWhen { get; set; }
    }

    public class SearchOrganizations : Query<IEnumerable<OrganizationMatch>>, IOrganizationCriteria
    {
        public string OrganizationName { get; set; }
        public string OrganizationSlug { get; set; }

        public int OrganizationNumber { get; set; }
        public int PartitionNumber { get; set; }

        public DateTime ModifiedWhen { get; set; }
    }

    public interface IOrganizationCriteria
    {
        Filter Filter { get; set; }
        
        string OrganizationName { get; set; }
        string OrganizationSlug { get; set; }

        int OrganizationNumber { get; set; }
        int PartitionNumber { get; set; }

        DateTime ModifiedWhen { get; set; }
    }

    public partial class OrganizationMatch
    {
        public Guid OrganizationId { get; set; }
    }

    public partial class OrganizationModel
    {
        public Guid OrganizationId { get; set; }

        public string OrganizationName { get; set; }
        public string OrganizationSlug { get; set; }

        public int OrganizationNumber { get; set; }
        public int PartitionNumber { get; set; }

        public DateTime ModifiedWhen { get; set; }
    }
}