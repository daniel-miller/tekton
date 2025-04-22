namespace Tek.Service.Security;

public class TSecretAdapter : IEntityAdapter
{
    public void Copy(ModifySecret modify, TSecretEntity entity)
    {
        entity.PasswordId = modify.PasswordId;
        entity.SecretType = modify.SecretType;
        entity.SecretName = modify.SecretName;
        entity.SecretValue = modify.SecretValue;
        entity.SecretScope = modify.SecretScope;
        entity.SecretExpiry = modify.SecretExpiry;
        entity.SecretLimetimeLimit = modify.SecretLimetimeLimit;

    }

    public TSecretEntity ToEntity(CreateSecret create)
    {
        var entity = new TSecretEntity
        {
            PasswordId = create.PasswordId,
            SecretId = create.SecretId,
            SecretType = create.SecretType,
            SecretName = create.SecretName,
            SecretValue = create.SecretValue,
            SecretScope = create.SecretScope,
            SecretExpiry = create.SecretExpiry,
            SecretLimetimeLimit = create.SecretLimetimeLimit
        };
        return entity;
    }

    public IEnumerable<SecretModel> ToModel(IEnumerable<TSecretEntity> entities)
    {
        return entities.Select(ToModel);
    }

    public SecretModel ToModel(TSecretEntity entity)
    {
        var model = new SecretModel
        {
            PasswordId = entity.PasswordId,
            SecretId = entity.SecretId,
            SecretType = entity.SecretType,
            SecretName = entity.SecretName,
            SecretValue = entity.SecretValue,
            SecretScope = entity.SecretScope,
            SecretExpiry = entity.SecretExpiry,
            SecretLimetimeLimit = entity.SecretLimetimeLimit
        };

        return model;
    }

    public IEnumerable<SecretMatch> ToMatch(IEnumerable<TSecretEntity> entities)
    {
        return entities.Select(ToMatch);
    }

    public SecretMatch ToMatch(TSecretEntity entity)
    {
        var match = new SecretMatch
        {
            SecretId = entity.SecretId

        };

        return match;
    }
}