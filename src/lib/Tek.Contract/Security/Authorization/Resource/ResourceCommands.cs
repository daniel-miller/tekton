using System;

namespace Tek.Contract
{
    public class CreateResource
    {
        public Guid ResourceId { get; set; }

        public string ResourceName { get; set; }
        public string ResourceType { get; set; }
    }

    public class ModifyResource
    {
        public Guid ResourceId { get; set; }

        public string ResourceName { get; set; }
        public string ResourceType { get; set; }
    }

    public class DeleteResource
    {
        public Guid ResourceId { get; set; }
    }
}