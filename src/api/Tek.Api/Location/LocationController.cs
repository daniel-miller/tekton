using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Tek.Service;

namespace Tek.Api;

[ApiController]
[ApiExplorerSettings(GroupName = "Location")]
public class LocationController : ControllerBase
{
    private readonly LocationDbContext _context;

    public LocationController(LocationDbContext context)
    {
        _context = context;
    }

    [HttpGet(Endpoints.Location.Countries)]
    [Authorize(Endpoints.Location.Countries)]
    public IActionResult GetCountries()
    {
        var result = _context.GetCountries();

        return Ok(result);
    }
}