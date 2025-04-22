namespace Tek.Service.Location;

public class ObsoleteLocationService
{
    private readonly TCountryReader _countries;
    private readonly TProvinceReader _provinces;

    public ObsoleteLocationService(TCountryReader countries, TProvinceReader provinces)
    {
        _countries = countries;
        _provinces = provinces;
    }

    public async Task<List<Country>> GetCountriesAsync()
    {
        var criteria = new CollectCountries();
        
        var list = await _countries.CollectAsync(criteria, new CancellationToken());

        return list.Select(i => new Country 
        {
            Code = i.CountryCode,
            Name = i.CountryName,
            Identifier = i.CountryId
        })
        .OrderBy(x => x.Name)
        .ToList();
    }

    public async Task<List<Province>> GetProvincesAsync(string country)
    {
        var criteria = new CollectProvinces();

        criteria.CountryCode = country;

        var list = await _provinces.CollectAsync(criteria, new CancellationToken());

        return list.Select(i => new Province
        {
            Code = i.ProvinceCode,
            Name = i.ProvinceName,
            Country = i.CountryCode,
            Translations = i.ProvinceNameTranslation
        })
        .OrderBy(x => x.Name)
        .ToList();
    }
}

public class Country
{
    public Country()
    {
        Code = "--";
        Name = "None";
        Identifier = Guid.Empty;
    }

    public Country(string code, string name, Guid identifier)
    {
        Code = code;
        Name = name;
        Identifier = identifier;
    }

    public string Code { get; set; }
    public string Name { get; set; }
    public Guid Identifier { get; set; }
}

public class Province
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Country { get; set; }
    public string? Translations { get; set; }
}