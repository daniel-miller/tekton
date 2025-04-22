namespace Tek.Service.Security;

public class TResourceAdapter : IEntityAdapter
{
    public void Copy(ModifyResource modify, TResourceEntity entity)
    {
        entity.ResourceType = modify.ResourceType;
        entity.ResourceName = modify.ResourceName;

    }

    public TResourceEntity ToEntity(CreateResource create)
    {
        var entity = new TResourceEntity
        {
            ResourceId = create.ResourceId,
            ResourceType = create.ResourceType,
            ResourceName = create.ResourceName
        };
        return entity;
    }

    public IEnumerable<ResourceModel> ToModel(IEnumerable<TResourceEntity> entities)
    {
        return entities.Select(ToModel);
    }

    public ResourceModel ToModel(TResourceEntity entity)
    {
        var model = new ResourceModel
        {
            ResourceId = entity.ResourceId,
            ResourceType = entity.ResourceType,
            ResourceName = entity.ResourceName
        };

        return model;
    }

    public IEnumerable<ResourceMatch> ToMatch(IEnumerable<TResourceEntity> entities)
    {
        return entities.Select(ToMatch);
    }

    public ResourceMatch ToMatch(TResourceEntity entity)
    {
        var match = new ResourceMatch
        {
            ResourceId = entity.ResourceId

        };

        return match;
    }
}