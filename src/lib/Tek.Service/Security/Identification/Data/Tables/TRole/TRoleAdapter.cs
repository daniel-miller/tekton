namespace Tek.Service.Security;

public class TRoleAdapter : IEntityAdapter
{
    public void Copy(ModifyRole modify, TRoleEntity entity)
    {
        entity.RoleType = modify.RoleType;
        entity.RoleName = modify.RoleName;

    }

    public TRoleEntity ToEntity(CreateRole create)
    {
        var entity = new TRoleEntity
        {
            RoleId = create.RoleId,
            RoleType = create.RoleType,
            RoleName = create.RoleName
        };
        return entity;
    }

    public IEnumerable<RoleModel> ToModel(IEnumerable<TRoleEntity> entities)
    {
        return entities.Select(ToModel);
    }

    public RoleModel ToModel(TRoleEntity entity)
    {
        var model = new RoleModel
        {
            RoleId = entity.RoleId,
            RoleType = entity.RoleType,
            RoleName = entity.RoleName
        };

        return model;
    }

    public IEnumerable<RoleMatch> ToMatch(IEnumerable<TRoleEntity> entities)
    {
        return entities.Select(ToMatch);
    }

    public RoleMatch ToMatch(TRoleEntity entity)
    {
        var match = new RoleMatch
        {
            RoleId = entity.RoleId

        };

        return match;
    }
}