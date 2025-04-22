using System;

namespace Tek.Contract
{
    public class CreateOrigin
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

    public class ModifyOrigin
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

    public class DeleteOrigin
    {
        public Guid OriginId { get; set; }
    }
}