using System;
using System.Collections.Generic;

namespace Tek.Contract
{
    public class AssertEntity : Query<bool>
    {
        public Guid EntityId { get; set; }
    }

    public class FetchEntity : Query<EntityModel>
    {
        public Guid EntityId { get; set; }
    }

    public class CollectEntities : Query<IEnumerable<EntityModel>>, IEntityCriteria
    {
        public string CollectionKey { get; set; }
        public string CollectionSlug { get; set; }
        public string ComponentFeature { get; set; }
        public string ComponentName { get; set; }
        public string ComponentType { get; set; }
        public string EntityName { get; set; }
        public string StorageKey { get; set; }
        public string StorageSchema { get; set; }
        public string StorageStructure { get; set; }
        public string StorageTable { get; set; }
        public string StorageTableFuture { get; set; }
    }

    public class CountEntities : Query<int>, IEntityCriteria
    {
        public string CollectionKey { get; set; }
        public string CollectionSlug { get; set; }
        public string ComponentFeature { get; set; }
        public string ComponentName { get; set; }
        public string ComponentType { get; set; }
        public string EntityName { get; set; }
        public string StorageKey { get; set; }
        public string StorageSchema { get; set; }
        public string StorageStructure { get; set; }
        public string StorageTable { get; set; }
        public string StorageTableFuture { get; set; }
    }

    public class SearchEntities : Query<IEnumerable<EntityMatch>>, IEntityCriteria
    {
        public string CollectionKey { get; set; }
        public string CollectionSlug { get; set; }
        public string ComponentFeature { get; set; }
        public string ComponentName { get; set; }
        public string ComponentType { get; set; }
        public string EntityName { get; set; }
        public string StorageKey { get; set; }
        public string StorageSchema { get; set; }
        public string StorageStructure { get; set; }
        public string StorageTable { get; set; }
        public string StorageTableFuture { get; set; }
    }

    public interface IEntityCriteria
    {
        Filter Filter { get; set; }
        
        string CollectionKey { get; set; }
        string CollectionSlug { get; set; }
        string ComponentFeature { get; set; }
        string ComponentName { get; set; }
        string ComponentType { get; set; }
        string EntityName { get; set; }
        string StorageKey { get; set; }
        string StorageSchema { get; set; }
        string StorageStructure { get; set; }
        string StorageTable { get; set; }
        string StorageTableFuture { get; set; }
    }

    public partial class EntityMatch
    {
        public Guid EntityId { get; set; }
    }

    public partial class EntityModel
    {
        public Guid EntityId { get; set; }

        public string CollectionKey { get; set; }
        public string CollectionSlug { get; set; }
        public string ComponentFeature { get; set; }
        public string ComponentName { get; set; }
        public string ComponentType { get; set; }
        public string EntityName { get; set; }
        public string StorageKey { get; set; }
        public string StorageSchema { get; set; }
        public string StorageStructure { get; set; }
        public string StorageTable { get; set; }
        public string StorageTableFuture { get; set; }
    }
}