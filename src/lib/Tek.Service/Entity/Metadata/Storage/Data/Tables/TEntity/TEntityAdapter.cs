namespace Tek.Service.Metadata;

public class TEntityAdapter : IEntityAdapter
{
    public void Copy(ModifyEntity modify, TEntityEntity entity)
    {
        entity.StorageStructure = modify.StorageStructure;
        entity.StorageSchema = modify.StorageSchema;
        entity.StorageTable = modify.StorageTable;
        entity.StorageTableFuture = modify.StorageTableFuture;
        entity.StorageKey = modify.StorageKey;
        entity.ComponentType = modify.ComponentType;
        entity.ComponentName = modify.ComponentName;
        entity.ComponentFeature = modify.ComponentFeature;
        entity.EntityName = modify.EntityName;
        entity.CollectionSlug = modify.CollectionSlug;
        entity.CollectionKey = modify.CollectionKey;

    }

    public TEntityEntity ToEntity(CreateEntity create)
    {
        var entity = new TEntityEntity
        {
            StorageStructure = create.StorageStructure,
            StorageSchema = create.StorageSchema,
            StorageTable = create.StorageTable,
            StorageTableFuture = create.StorageTableFuture,
            StorageKey = create.StorageKey,
            ComponentType = create.ComponentType,
            ComponentName = create.ComponentName,
            ComponentFeature = create.ComponentFeature,
            EntityName = create.EntityName,
            EntityId = create.EntityId,
            CollectionSlug = create.CollectionSlug,
            CollectionKey = create.CollectionKey
        };
        return entity;
    }

    public IEnumerable<EntityModel> ToModel(IEnumerable<TEntityEntity> entities)
    {
        return entities.Select(ToModel);
    }

    public EntityModel ToModel(TEntityEntity entity)
    {
        var model = new EntityModel
        {
            StorageStructure = entity.StorageStructure,
            StorageSchema = entity.StorageSchema,
            StorageTable = entity.StorageTable,
            StorageTableFuture = entity.StorageTableFuture,
            StorageKey = entity.StorageKey,
            ComponentType = entity.ComponentType,
            ComponentName = entity.ComponentName,
            ComponentFeature = entity.ComponentFeature,
            EntityName = entity.EntityName,
            EntityId = entity.EntityId,
            CollectionSlug = entity.CollectionSlug,
            CollectionKey = entity.CollectionKey
        };

        return model;
    }

    public IEnumerable<EntityMatch> ToMatch(IEnumerable<TEntityEntity> entities)
    {
        return entities.Select(ToMatch);
    }

    public EntityMatch ToMatch(TEntityEntity entity)
    {
        var match = new EntityMatch
        {
            EntityId = entity.EntityId

        };

        return match;
    }
}