namespace Tek.Service.Contact;

public class TCountryAdapter : IEntityAdapter
{
    public void Copy(ModifyCountry modify, TCountryEntity entity)
    {
        entity.CountryCode = modify.CountryCode;
        entity.CountryName = modify.CountryName;
        entity.Languages = modify.Languages;
        entity.CurrencyCode = modify.CurrencyCode;
        entity.CurrencyName = modify.CurrencyName;
        entity.TopLevelDomain = modify.TopLevelDomain;
        entity.ContinentCode = modify.ContinentCode;
        entity.CapitalCityName = modify.CapitalCityName;

    }

    public TCountryEntity ToEntity(CreateCountry create)
    {
        var entity = new TCountryEntity
        {
            CountryId = create.CountryId,
            CountryCode = create.CountryCode,
            CountryName = create.CountryName,
            Languages = create.Languages,
            CurrencyCode = create.CurrencyCode,
            CurrencyName = create.CurrencyName,
            TopLevelDomain = create.TopLevelDomain,
            ContinentCode = create.ContinentCode,
            CapitalCityName = create.CapitalCityName
        };
        return entity;
    }

    public IEnumerable<CountryModel> ToModel(IEnumerable<TCountryEntity> entities)
    {
        return entities.Select(ToModel);
    }

    public CountryModel ToModel(TCountryEntity entity)
    {
        var model = new CountryModel
        {
            CountryId = entity.CountryId,
            CountryCode = entity.CountryCode,
            CountryName = entity.CountryName,
            Languages = entity.Languages,
            CurrencyCode = entity.CurrencyCode,
            CurrencyName = entity.CurrencyName,
            TopLevelDomain = entity.TopLevelDomain,
            ContinentCode = entity.ContinentCode,
            CapitalCityName = entity.CapitalCityName
        };

        return model;
    }

    public IEnumerable<CountryMatch> ToMatch(IEnumerable<TCountryEntity> entities)
    {
        return entities.Select(ToMatch);
    }

    public CountryMatch ToMatch(TCountryEntity entity)
    {
        var match = new CountryMatch
        {
            CountryId = entity.CountryId

        };

        return match;
    }
}