namespace Tek.Service.Security;

public class TPartitionAdapter : IEntityAdapter
{
    public void Copy(ModifyPartition modify, TPartitionEntity entity)
    {
        entity.PartitionSlug = modify.PartitionSlug;
        entity.PartitionName = modify.PartitionName;
        entity.PartitionHost = modify.PartitionHost;
        entity.PartitionEmail = modify.PartitionEmail;
        entity.PartitionSettings = modify.PartitionSettings;
        entity.PartitionTesters = modify.PartitionTesters;
        entity.ModifiedWhen = modify.ModifiedWhen;

    }

    public TPartitionEntity ToEntity(CreatePartition create)
    {
        var entity = new TPartitionEntity
        {
            PartitionNumber = create.PartitionNumber,
            PartitionSlug = create.PartitionSlug,
            PartitionName = create.PartitionName,
            PartitionHost = create.PartitionHost,
            PartitionEmail = create.PartitionEmail,
            PartitionSettings = create.PartitionSettings,
            PartitionTesters = create.PartitionTesters,
            ModifiedWhen = create.ModifiedWhen
        };
        return entity;
    }

    public IEnumerable<PartitionModel> ToModel(IEnumerable<TPartitionEntity> entities)
    {
        return entities.Select(ToModel);
    }

    public PartitionModel ToModel(TPartitionEntity entity)
    {
        var model = new PartitionModel
        {
            PartitionNumber = entity.PartitionNumber,
            PartitionSlug = entity.PartitionSlug,
            PartitionName = entity.PartitionName,
            PartitionHost = entity.PartitionHost,
            PartitionEmail = entity.PartitionEmail,
            PartitionSettings = entity.PartitionSettings,
            PartitionTesters = entity.PartitionTesters,
            ModifiedWhen = entity.ModifiedWhen
        };

        return model;
    }

    public IEnumerable<PartitionMatch> ToMatch(IEnumerable<TPartitionEntity> entities)
    {
        return entities.Select(ToMatch);
    }

    public PartitionMatch ToMatch(TPartitionEntity entity)
    {
        var match = new PartitionMatch
        {
            PartitionNumber = entity.PartitionNumber

        };

        return match;
    }
}