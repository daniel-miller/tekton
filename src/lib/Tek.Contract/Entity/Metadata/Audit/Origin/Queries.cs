using System;
using System.Collections.Generic;

namespace Tek.Contract
{
    public class AssertOrigin : Query<bool>
    {
        public Guid OriginId { get; set; }
    }

    public class FetchOrigin : Query<OriginModel>
    {
        public Guid OriginId { get; set; }
    }

    public class CollectOrigins : Query<IEnumerable<OriginModel>>, IOriginCriteria
    {
        public Guid OrganizationId { get; set; }
        public Guid? ProxyAgent { get; set; }
        public Guid? ProxySubject { get; set; }
        public Guid UserId { get; set; }

        public string OriginDescription { get; set; }
        public string OriginReason { get; set; }
        public string OriginSource { get; set; }

        public DateTime OriginWhen { get; set; }
    }

    public class CountOrigins : Query<int>, IOriginCriteria
    {
        public Guid OrganizationId { get; set; }
        public Guid? ProxyAgent { get; set; }
        public Guid? ProxySubject { get; set; }
        public Guid UserId { get; set; }

        public string OriginDescription { get; set; }
        public string OriginReason { get; set; }
        public string OriginSource { get; set; }

        public DateTime OriginWhen { get; set; }
    }

    public class SearchOrigins : Query<IEnumerable<OriginMatch>>, IOriginCriteria
    {
        public Guid OrganizationId { get; set; }
        public Guid? ProxyAgent { get; set; }
        public Guid? ProxySubject { get; set; }
        public Guid UserId { get; set; }

        public string OriginDescription { get; set; }
        public string OriginReason { get; set; }
        public string OriginSource { get; set; }

        public DateTime OriginWhen { get; set; }
    }

    public interface IOriginCriteria
    {
        Filter Filter { get; set; }
        
        Guid OrganizationId { get; set; }
        Guid? ProxyAgent { get; set; }
        Guid? ProxySubject { get; set; }
        Guid UserId { get; set; }

        string OriginDescription { get; set; }
        string OriginReason { get; set; }
        string OriginSource { get; set; }

        DateTime OriginWhen { get; set; }
    }

    public partial class OriginMatch
    {
        public Guid OriginId { get; set; }
    }

    public partial class OriginModel
    {
        public Guid OrganizationId { get; set; }
        public Guid OriginId { get; set; }
        public Guid? ProxyAgent { get; set; }
        public Guid? ProxySubject { get; set; }
        public Guid UserId { get; set; }

        public string OriginDescription { get; set; }
        public string OriginReason { get; set; }
        public string OriginSource { get; set; }

        public DateTime OriginWhen { get; set; }
    }
}