namespace Tek.Service.Location;

public partial class TProvinceEntity
{
    public Guid? CountryId { get; set; }
    public Guid ProvinceId { get; set; }

    public string CountryCode { get; set; } = null!;
    public string? ProvinceCode { get; set; }
    public string ProvinceName { get; set; } = null!;
    public string? ProvinceNameTranslation { get; set; }
}