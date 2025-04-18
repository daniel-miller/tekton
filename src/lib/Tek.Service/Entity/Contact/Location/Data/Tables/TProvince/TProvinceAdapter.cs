namespace Tek.Service.Contact;

public class TProvinceAdapter : IEntityAdapter
{
    public void Copy(ModifyProvince modify, TProvinceEntity entity)
    {
        entity.ProvinceCode = modify.ProvinceCode;
        entity.ProvinceName = modify.ProvinceName;
        entity.CountryCode = modify.CountryCode;
        entity.CountryId = modify.CountryId;

    }

    public TProvinceEntity ToEntity(CreateProvince create)
    {
        var entity = new TProvinceEntity
        {
            ProvinceId = create.ProvinceId,
            ProvinceCode = create.ProvinceCode,
            ProvinceName = create.ProvinceName,
            CountryCode = create.CountryCode,
            CountryId = create.CountryId
        };
        return entity;
    }

    public IEnumerable<ProvinceModel> ToModel(IEnumerable<TProvinceEntity> entities)
    {
        return entities.Select(ToModel);
    }

    public ProvinceModel ToModel(TProvinceEntity entity)
    {
        var model = new ProvinceModel
        {
            ProvinceId = entity.ProvinceId,
            ProvinceCode = entity.ProvinceCode,
            ProvinceName = entity.ProvinceName,
            CountryCode = entity.CountryCode,
            CountryId = entity.CountryId
        };

        return model;
    }

    public IEnumerable<ProvinceMatch> ToMatch(IEnumerable<TProvinceEntity> entities)
    {
        return entities.Select(ToMatch);
    }

    public ProvinceMatch ToMatch(TProvinceEntity entity)
    {
        var match = new ProvinceMatch
        {
            ProvinceId = entity.ProvinceId

        };

        return match;
    }
}