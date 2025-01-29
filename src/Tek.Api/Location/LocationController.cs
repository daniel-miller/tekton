using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Tek.Service;

namespace Tek.Api;

[ApiController]
[ApiExplorerSettings(GroupName = "Location")]
public class LocationController : ControllerBase
{
    private readonly TestDbContext _context;

    public LocationController(TestDbContext context)
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