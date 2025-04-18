namespace Tek.Service.Security;

public class TPasswordAdapter : IEntityAdapter
{
    public void Copy(ModifyPassword modify, TPasswordEntity entity)
    {
        entity.EmailId = modify.EmailId;
        entity.EmailAddress = modify.EmailAddress;
        entity.PasswordHash = modify.PasswordHash;
        entity.PasswordExpiry = modify.PasswordExpiry;
        entity.DefaultPlaintext = modify.DefaultPlaintext;
        entity.DefaultExpiry = modify.DefaultExpiry;
        entity.CreatedWhen = modify.CreatedWhen;
        entity.LastForgottenWhen = modify.LastForgottenWhen;
        entity.LastModifiedWhen = modify.LastModifiedWhen;

    }

    public TPasswordEntity ToEntity(CreatePassword create)
    {
        var entity = new TPasswordEntity
        {
            PasswordId = create.PasswordId,
            EmailId = create.EmailId,
            EmailAddress = create.EmailAddress,
            PasswordHash = create.PasswordHash,
            PasswordExpiry = create.PasswordExpiry,
            DefaultPlaintext = create.DefaultPlaintext,
            DefaultExpiry = create.DefaultExpiry,
            CreatedWhen = create.CreatedWhen,
            LastForgottenWhen = create.LastForgottenWhen,
            LastModifiedWhen = create.LastModifiedWhen
        };
        return entity;
    }

    public IEnumerable<PasswordModel> ToModel(IEnumerable<TPasswordEntity> entities)
    {
        return entities.Select(ToModel);
    }

    public PasswordModel ToModel(TPasswordEntity entity)
    {
        var model = new PasswordModel
        {
            PasswordId = entity.PasswordId,
            EmailId = entity.EmailId,
            EmailAddress = entity.EmailAddress,
            PasswordHash = entity.PasswordHash,
            PasswordExpiry = entity.PasswordExpiry,
            DefaultPlaintext = entity.DefaultPlaintext,
            DefaultExpiry = entity.DefaultExpiry,
            CreatedWhen = entity.CreatedWhen,
            LastForgottenWhen = entity.LastForgottenWhen,
            LastModifiedWhen = entity.LastModifiedWhen
        };

        return model;
    }

    public IEnumerable<PasswordMatch> ToMatch(IEnumerable<TPasswordEntity> entities)
    {
        return entities.Select(ToMatch);
    }

    public PasswordMatch ToMatch(TPasswordEntity entity)
    {
        var match = new PasswordMatch
        {
            PasswordId = entity.PasswordId

        };

        return match;
    }
}