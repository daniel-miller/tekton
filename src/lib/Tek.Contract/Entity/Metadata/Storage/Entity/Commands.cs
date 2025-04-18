using System;

namespace Tek.Contract
{
    public class CreateEntity
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

    public class ModifyEntity
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

    public class DeleteEntity
    {
        public Guid EntityId { get; set; }
    }
}