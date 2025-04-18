namespace Tek.Service.Content;

public class TTranslationAdapter : IEntityAdapter
{
    public void Copy(ModifyTranslation modify, TTranslationEntity entity)
    {
        entity.TranslationText = modify.TranslationText;
        entity.ModifiedWhen = modify.ModifiedWhen;

    }

    public TTranslationEntity ToEntity(CreateTranslation create)
    {
        var entity = new TTranslationEntity
        {
            TranslationId = create.TranslationId,
            TranslationText = create.TranslationText,
            ModifiedWhen = create.ModifiedWhen
        };
        return entity;
    }

    public IEnumerable<TranslationModel> ToModel(IEnumerable<TTranslationEntity> entities)
    {
        return entities.Select(ToModel);
    }

    public TranslationModel ToModel(TTranslationEntity entity)
    {
        var model = new TranslationModel
        {
            TranslationId = entity.TranslationId,
            TranslationText = entity.TranslationText,
            ModifiedWhen = entity.ModifiedWhen
        };

        return model;
    }

    public IEnumerable<TranslationMatch> ToMatch(IEnumerable<TTranslationEntity> entities)
    {
        return entities.Select(ToMatch);
    }

    public TranslationMatch ToMatch(TTranslationEntity entity)
    {
        var match = new TranslationMatch
        {
            TranslationId = entity.TranslationId

        };

        return match;
    }
}