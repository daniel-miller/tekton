namespace Tek.Service.Security;

public class TPermissionAdapter : IEntityAdapter
{
    public void Copy(ModifyPermission modify, TPermissionEntity entity)
    {
        entity.AccessType = modify.AccessType;
        entity.AccessFlags = modify.AccessFlags;
        entity.ResourceId = modify.ResourceId;
        entity.RoleId = modify.RoleId;

    }

    public TPermissionEntity ToEntity(CreatePermission create)
    {
        var entity = new TPermissionEntity
        {
            PermissionId = create.PermissionId,
            AccessType = create.AccessType,
            AccessFlags = create.AccessFlags,
            ResourceId = create.ResourceId,
            RoleId = create.RoleId
        };
        return entity;
    }

    public IEnumerable<PermissionModel> ToModel(IEnumerable<TPermissionEntity> entities)
    {
        return entities.Select(ToModel);
    }

    public PermissionModel ToModel(TPermissionEntity entity)
    {
        var model = new PermissionModel
        {
            PermissionId = entity.PermissionId,
            AccessType = entity.AccessType,
            AccessFlags = entity.AccessFlags,
            ResourceId = entity.ResourceId,
            RoleId = entity.RoleId
        };

        return model;
    }

    public IEnumerable<PermissionMatch> ToMatch(IEnumerable<TPermissionEntity> entities)
    {
        return entities.Select(ToMatch);
    }

    public PermissionMatch ToMatch(TPermissionEntity entity)
    {
        var match = new PermissionMatch
        {
            PermissionId = entity.PermissionId

        };

        return match;
    }
}