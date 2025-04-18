using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Tek.Service.Contact;

namespace Tek.Api;

[ApiController]
[ApiExplorerSettings(GroupName = Endpoints.ContactApi.Location.Name)]
[Authorize]
public class CountryController : ControllerBase
{
    private readonly CountryService _countryService;

    public CountryController(CountryService countryService)
    {
        _countryService = countryService;
    }

    [Authorize(Endpoints.ContactApi.Location.Country.Assert)]
    [HttpHead(Endpoints.ContactApi.Location.Country.Assert)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> AssertAsync([FromRoute] Guid country, CancellationToken token)
    {
        var exists = await _countryService.AssertAsync(country, token);

        return Ok(exists);
    }

    [Authorize(Endpoints.ContactApi.Location.Country.Fetch)]
    [HttpGet(Endpoints.ContactApi.Location.Country.Fetch)]
    [ProducesResponseType(typeof(CountryModel), StatusCodes.Status200OK)]
    public async Task<ActionResult<CountryModel>> FetchAsync([FromRoute] Guid country, CancellationToken token)
    {
        var model = await _countryService.FetchAsync(country, token);

        if (model == null)
            return NotFound();

        return Ok(model);
    }

    [Authorize(Endpoints.ContactApi.Location.Country.Count)]
    [HttpGet(Endpoints.ContactApi.Location.Country.Count)]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> CountAsync([FromQuery] CountCountries query, CancellationToken token)
    {
        var count = await _countryService.CountAsync(query, token);

        return Ok(count);
    }

    [Authorize(Endpoints.ContactApi.Location.Country.Collect)]
    [HttpGet(Endpoints.ContactApi.Location.Country.Collect)]
    [ProducesResponseType(typeof(IEnumerable<CountryModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CountryModel>>> CollectAsync([FromQuery] CollectCountries query, CancellationToken token)
    {
        var models = await _countryService.CollectAsync(query, token);

        var count = await _countryService.CountAsync(query, token);

        Response.AddPagination(query.Filter, models.Count(), count);

        return Ok(models);
    }

    [Authorize(Endpoints.ContactApi.Location.Country.Search)]
    [HttpGet(Endpoints.ContactApi.Location.Country.Search)]
    [ProducesResponseType(typeof(IEnumerable<CountryMatch>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CountryMatch>>> SearchAsync([FromQuery] SearchCountries query, CancellationToken token)
    {
        var matches = await _countryService.SearchAsync(query, token);

        var count = await _countryService.CountAsync(query, token);

        Response.AddPagination(query.Filter, matches.Count(), count);

        return Ok(matches);
    }

    [Authorize(Endpoints.ContactApi.Location.Country.Create)]
    [HttpPost(Endpoints.ContactApi.Location.Country.Create)]
    [ProducesResponseType(typeof(CountryModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CountryModel>> CreateAsync([FromBody] CreateCountry create, CancellationToken token)
    {
        var created = await _countryService.CreateAsync(create, token);

        if (!created)
            return BadRequest($"Duplicate not permitted: CountryId {create.CountryId}. You cannot insert a duplicate object with the same primary key.");

        var model = await _countryService.FetchAsync(create.CountryId, token);

        return CreatedAtAction(nameof(CreateAsync), model);
    }

    [Authorize(Endpoints.ContactApi.Location.Country.Modify)]
    [HttpPut(Endpoints.ContactApi.Location.Country.Modify)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ModifyAsync([FromBody] ModifyCountry modify, CancellationToken token)
    {
        var model = await _countryService.FetchAsync(modify.CountryId, token);

        if (model is null)
            return NotFound($"Country not found: CountryId {modify.CountryId}. You cannot modify an object that is not in the database.");

        var modified = await _countryService.ModifyAsync(modify, token);

        if (!modified)
            return NotFound();
        
        return Ok();
    }

    [Authorize(Endpoints.ContactApi.Location.Country.Delete)]
    [HttpDelete(Endpoints.ContactApi.Location.Country.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid country, CancellationToken token)
    {
        var deleted = await _countryService.DeleteAsync(country, token);
        
        if (!deleted)
            return NotFound();
        
        return Ok();
    }
}