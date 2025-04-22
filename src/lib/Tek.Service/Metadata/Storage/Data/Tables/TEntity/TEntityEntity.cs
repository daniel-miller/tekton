namespace Tek.Service.Metadata;

public partial class TEntityEntity
{
    public Guid EntityId { get; set; }

    public string CollectionKey { get; set; } = null!;
    public string CollectionSlug { get; set; } = null!;
    public string ComponentFeature { get; set; } = null!;
    public string ComponentName { get; set; } = null!;
    public string ComponentType { get; set; } = null!;
    public string EntityName { get; set; } = null!;
    public string StorageKey { get; set; } = null!;
    public string StorageSchema { get; set; } = null!;
    public string StorageStructure { get; set; } = null!;
    public string StorageTable { get; set; } = null!;
    public string? StorageTableFuture { get; set; }
}