namespace Tek.Service.Contact;

public partial class TCountryEntity
{
    public Guid CountryId { get; set; }

    public string? CapitalCityName { get; set; }
    public string? ContinentCode { get; set; }
    public string CountryCode { get; set; } = null!;
    public string CountryName { get; set; } = null!;
    public string? CurrencyCode { get; set; }
    public string? CurrencyName { get; set; }
    public string? Languages { get; set; }
    public string? TopLevelDomain { get; set; }
}