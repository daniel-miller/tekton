using Microsoft.AspNetCore.Mvc;

using Tek.Service.Location;

namespace Tek.Api.Legacy;

[ApiController]
public class LocationsController : ControllerBase
{
    private readonly ObsoleteLocationService _service;

    public LocationsController(ObsoleteLocationService service)
    {
        _service = service;
    }

    [HttpGet("contact/locations/countries")]
    public async Task<ActionResult<List<Country>>> CountriesAsync()
    {
        return await _service.GetCountriesAsync();
    }

    [HttpGet("contact/locations/countries/{country}/provinces")]
    [HttpGet("contact/locations/countries/{country}/states")]
    public async Task<ActionResult<List<Province>>> ProvincesAsync(string country)
    {
        return await _service.GetProvincesAsync(country);
    }
}

