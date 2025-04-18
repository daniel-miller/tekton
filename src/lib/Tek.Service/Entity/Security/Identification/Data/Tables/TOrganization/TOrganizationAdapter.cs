namespace Tek.Service.Security;

public class TOrganizationAdapter : IEntityAdapter
{
    public void Copy(ModifyOrganization modify, TOrganizationEntity entity)
    {
        entity.OrganizationNumber = modify.OrganizationNumber;
        entity.OrganizationSlug = modify.OrganizationSlug;
        entity.OrganizationName = modify.OrganizationName;
        entity.PartitionNumber = modify.PartitionNumber;
        entity.ModifiedWhen = modify.ModifiedWhen;

    }

    public TOrganizationEntity ToEntity(CreateOrganization create)
    {
        var entity = new TOrganizationEntity
        {
            OrganizationId = create.OrganizationId,
            OrganizationNumber = create.OrganizationNumber,
            OrganizationSlug = create.OrganizationSlug,
            OrganizationName = create.OrganizationName,
            PartitionNumber = create.PartitionNumber,
            ModifiedWhen = create.ModifiedWhen
        };
        return entity;
    }

    public IEnumerable<OrganizationModel> ToModel(IEnumerable<TOrganizationEntity> entities)
    {
        return entities.Select(ToModel);
    }

    public OrganizationModel ToModel(TOrganizationEntity entity)
    {
        var model = new OrganizationModel
        {
            OrganizationId = entity.OrganizationId,
            OrganizationNumber = entity.OrganizationNumber,
            OrganizationSlug = entity.OrganizationSlug,
            OrganizationName = entity.OrganizationName,
            PartitionNumber = entity.PartitionNumber,
            ModifiedWhen = entity.ModifiedWhen
        };

        return model;
    }

    public IEnumerable<OrganizationMatch> ToMatch(IEnumerable<TOrganizationEntity> entities)
    {
        return entities.Select(ToMatch);
    }

    public OrganizationMatch ToMatch(TOrganizationEntity entity)
    {
        var match = new OrganizationMatch
        {
            OrganizationId = entity.OrganizationId

        };

        return match;
    }
}