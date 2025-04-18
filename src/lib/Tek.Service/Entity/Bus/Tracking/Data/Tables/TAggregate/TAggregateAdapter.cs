namespace Tek.Service.Bus;

public class TAggregateAdapter : IEntityAdapter
{
    public void Copy(ModifyAggregate modify, TAggregateEntity entity)
    {
        entity.AggregateType = modify.AggregateType;
        entity.AggregateRoot = modify.AggregateRoot;

    }

    public TAggregateEntity ToEntity(CreateAggregate create)
    {
        var entity = new TAggregateEntity
        {
            AggregateId = create.AggregateId,
            AggregateType = create.AggregateType,
            AggregateRoot = create.AggregateRoot
        };
        return entity;
    }

    public IEnumerable<AggregateModel> ToModel(IEnumerable<TAggregateEntity> entities)
    {
        return entities.Select(ToModel);
    }

    public AggregateModel ToModel(TAggregateEntity entity)
    {
        var model = new AggregateModel
        {
            AggregateId = entity.AggregateId,
            AggregateType = entity.AggregateType,
            AggregateRoot = entity.AggregateRoot
        };

        return model;
    }

    public IEnumerable<AggregateMatch> ToMatch(IEnumerable<TAggregateEntity> entities)
    {
        return entities.Select(ToMatch);
    }

    public AggregateMatch ToMatch(TAggregateEntity entity)
    {
        var match = new AggregateMatch
        {
            AggregateId = entity.AggregateId

        };

        return match;
    }
}