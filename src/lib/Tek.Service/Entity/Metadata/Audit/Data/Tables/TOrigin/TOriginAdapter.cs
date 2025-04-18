namespace Tek.Service.Metadata;

public class TOriginAdapter : IEntityAdapter
{
    public void Copy(ModifyOrigin modify, TOriginEntity entity)
    {
        entity.OriginWhen = modify.OriginWhen;
        entity.OriginDescription = modify.OriginDescription;
        entity.OriginReason = modify.OriginReason;
        entity.OriginSource = modify.OriginSource;
        entity.UserId = modify.UserId;
        entity.OrganizationId = modify.OrganizationId;
        entity.ProxyAgent = modify.ProxyAgent;
        entity.ProxySubject = modify.ProxySubject;

    }

    public TOriginEntity ToEntity(CreateOrigin create)
    {
        var entity = new TOriginEntity
        {
            OriginId = create.OriginId,
            OriginWhen = create.OriginWhen,
            OriginDescription = create.OriginDescription,
            OriginReason = create.OriginReason,
            OriginSource = create.OriginSource,
            UserId = create.UserId,
            OrganizationId = create.OrganizationId,
            ProxyAgent = create.ProxyAgent,
            ProxySubject = create.ProxySubject
        };
        return entity;
    }

    public IEnumerable<OriginModel> ToModel(IEnumerable<TOriginEntity> entities)
    {
        return entities.Select(ToModel);
    }

    public OriginModel ToModel(TOriginEntity entity)
    {
        var model = new OriginModel
        {
            OriginId = entity.OriginId,
            OriginWhen = entity.OriginWhen,
            OriginDescription = entity.OriginDescription,
            OriginReason = entity.OriginReason,
            OriginSource = entity.OriginSource,
            UserId = entity.UserId,
            OrganizationId = entity.OrganizationId,
            ProxyAgent = entity.ProxyAgent,
            ProxySubject = entity.ProxySubject
        };

        return model;
    }

    public IEnumerable<OriginMatch> ToMatch(IEnumerable<TOriginEntity> entities)
    {
        return entities.Select(ToMatch);
    }

    public OriginMatch ToMatch(TOriginEntity entity)
    {
        var match = new OriginMatch
        {
            OriginId = entity.OriginId

        };

        return match;
    }
}