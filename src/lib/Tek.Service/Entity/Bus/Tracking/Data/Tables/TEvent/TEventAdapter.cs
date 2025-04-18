namespace Tek.Service.Bus;

public class TEventAdapter : IEntityAdapter
{
    public void Copy(ModifyEvent modify, TEventEntity entity)
    {
        entity.EventType = modify.EventType;
        entity.EventData = modify.EventData;
        entity.AggregateId = modify.AggregateId;
        entity.AggregateVersion = modify.AggregateVersion;
        entity.OriginId = modify.OriginId;

    }

    public TEventEntity ToEntity(CreateEvent create)
    {
        var entity = new TEventEntity
        {
            EventId = create.EventId,
            EventType = create.EventType,
            EventData = create.EventData,
            AggregateId = create.AggregateId,
            AggregateVersion = create.AggregateVersion,
            OriginId = create.OriginId
        };
        return entity;
    }

    public IEnumerable<EventModel> ToModel(IEnumerable<TEventEntity> entities)
    {
        return entities.Select(ToModel);
    }

    public EventModel ToModel(TEventEntity entity)
    {
        var model = new EventModel
        {
            EventId = entity.EventId,
            EventType = entity.EventType,
            EventData = entity.EventData,
            AggregateId = entity.AggregateId,
            AggregateVersion = entity.AggregateVersion,
            OriginId = entity.OriginId
        };

        return model;
    }

    public IEnumerable<EventMatch> ToMatch(IEnumerable<TEventEntity> entities)
    {
        return entities.Select(ToMatch);
    }

    public EventMatch ToMatch(TEventEntity entity)
    {
        var match = new EventMatch
        {
            EventId = entity.EventId

        };

        return match;
    }
}