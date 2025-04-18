using System;
using System.Collections.Generic;

namespace Tek.Contract
{
    public class AssertResource : Query<bool>
    {
        public Guid ResourceId { get; set; }
    }

    public class FetchResource : Query<ResourceModel>
    {
        public Guid ResourceId { get; set; }
    }

    public class CollectResources : Query<IEnumerable<ResourceModel>>, IResourceCriteria
    {
        public string ResourceName { get; set; }
        public string ResourceType { get; set; }
    }

    public class CountResources : Query<int>, IResourceCriteria
    {
        public string ResourceName { get; set; }
        public string ResourceType { get; set; }
    }

    public class SearchResources : Query<IEnumerable<ResourceMatch>>, IResourceCriteria
    {
        public string ResourceName { get; set; }
        public string ResourceType { get; set; }
    }

    public interface IResourceCriteria
    {
        Filter Filter { get; set; }
        
        string ResourceName { get; set; }
        string ResourceType { get; set; }
    }

    public partial class ResourceMatch
    {
        public Guid ResourceId { get; set; }
    }

    public partial class ResourceModel
    {
        public Guid ResourceId { get; set; }

        public string ResourceName { get; set; }
        public string ResourceType { get; set; }
    }
}