namespace Tek.Service.Metadata;

public class TVersionAdapter : IEntityAdapter
{
    public void Copy(ModifyVersion modify, TVersionEntity entity)
    {
        entity.VersionType = modify.VersionType;
        entity.VersionName = modify.VersionName;
        entity.ScriptPath = modify.ScriptPath;
        entity.ScriptContent = modify.ScriptContent;
        entity.ScriptExecuted = modify.ScriptExecuted;

    }

    public TVersionEntity ToEntity(CreateVersion create)
    {
        var entity = new TVersionEntity
        {
            VersionNumber = create.VersionNumber,
            VersionType = create.VersionType,
            VersionName = create.VersionName,
            ScriptPath = create.ScriptPath,
            ScriptContent = create.ScriptContent,
            ScriptExecuted = create.ScriptExecuted
        };
        return entity;
    }

    public IEnumerable<VersionModel> ToModel(IEnumerable<TVersionEntity> entities)
    {
        return entities.Select(ToModel);
    }

    public VersionModel ToModel(TVersionEntity entity)
    {
        var model = new VersionModel
        {
            VersionNumber = entity.VersionNumber,
            VersionType = entity.VersionType,
            VersionName = entity.VersionName,
            ScriptPath = entity.ScriptPath,
            ScriptContent = entity.ScriptContent,
            ScriptExecuted = entity.ScriptExecuted
        };

        return model;
    }

    public IEnumerable<VersionMatch> ToMatch(IEnumerable<TVersionEntity> entities)
    {
        return entities.Select(ToMatch);
    }

    public VersionMatch ToMatch(TVersionEntity entity)
    {
        var match = new VersionMatch
        {
            VersionNumber = entity.VersionNumber

        };

        return match;
    }
}