using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Tek.Service.Location;

namespace Tek.Api;

[ApiController]
[ApiExplorerSettings(GroupName = Endpoints.LocationApi.Region.Name)]
[Authorize]
public class ProvinceController : ControllerBase
{
    private readonly ProvinceService _provinceService;

    public ProvinceController(ProvinceService provinceService)
    {
        _provinceService = provinceService;
    }

    [Authorize(Endpoints.LocationApi.Region.Province.Assert)]
    [HttpHead(Endpoints.LocationApi.Region.Province.Assert)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> AssertAsync([FromRoute] Guid province, CancellationToken token)
    {
        var exists = await _provinceService.AssertAsync(province, token);

        return Ok(exists);
    }

    [Authorize(Endpoints.LocationApi.Region.Province.Fetch)]
    [HttpGet(Endpoints.LocationApi.Region.Province.Fetch)]
    [ProducesResponseType(typeof(ProvinceModel), StatusCodes.Status200OK)]
    public async Task<ActionResult<ProvinceModel>> FetchAsync([FromRoute] Guid province, CancellationToken token)
    {
        var model = await _provinceService.FetchAsync(province, token);

        if (model == null)
            return NotFound();

        return Ok(model);
    }

    [Authorize(Endpoints.LocationApi.Region.Province.Count)]
    [HttpGet(Endpoints.LocationApi.Region.Province.Count)]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> CountAsync([FromQuery] CountProvinces query, CancellationToken token)
    {
        var count = await _provinceService.CountAsync(query, token);

        return Ok(count);
    }

    [Authorize(Endpoints.LocationApi.Region.Province.Collect)]
    [HttpGet(Endpoints.LocationApi.Region.Province.Collect)]
    [ProducesResponseType(typeof(IEnumerable<ProvinceModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProvinceModel>>> CollectAsync([FromQuery] CollectProvinces query, CancellationToken token)
    {
        var models = await _provinceService.CollectAsync(query, token);

        var count = await _provinceService.CountAsync(query, token);

        Response.AddPagination(query.Filter, models.Count(), count);

        return Ok(models);
    }

    [Authorize(Endpoints.LocationApi.Region.Province.Search)]
    [HttpGet(Endpoints.LocationApi.Region.Province.Search)]
    [ProducesResponseType(typeof(IEnumerable<ProvinceMatch>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProvinceMatch>>> SearchAsync([FromQuery] SearchProvinces query, CancellationToken token)
    {
        var matches = await _provinceService.SearchAsync(query, token);

        var count = await _provinceService.CountAsync(query, token);

        Response.AddPagination(query.Filter, matches.Count(), count);

        return Ok(matches);
    }

    [Authorize(Endpoints.LocationApi.Region.Province.Create)]
    [HttpPost(Endpoints.LocationApi.Region.Province.Create)]
    [ProducesResponseType(typeof(ProvinceModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProvinceModel>> CreateAsync([FromBody] CreateProvince create, CancellationToken token)
    {
        var created = await _provinceService.CreateAsync(create, token);

        if (!created)
            return BadRequest($"Duplicate not permitted: ProvinceId {create.ProvinceId}. You cannot insert a duplicate object with the same primary key.");

        var model = await _provinceService.FetchAsync(create.ProvinceId, token);

        return CreatedAtAction(nameof(CreateAsync), model);
    }

    [Authorize(Endpoints.LocationApi.Region.Province.Modify)]
    [HttpPut(Endpoints.LocationApi.Region.Province.Modify)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ModifyAsync([FromBody] ModifyProvince modify, CancellationToken token)
    {
        var model = await _provinceService.FetchAsync(modify.ProvinceId, token);

        if (model is null)
            return NotFound($"Province not found: ProvinceId {modify.ProvinceId}. You cannot modify an object that is not in the database.");

        var modified = await _provinceService.ModifyAsync(modify, token);

        if (!modified)
            return NotFound();
        
        return Ok();
    }

    [Authorize(Endpoints.LocationApi.Region.Province.Delete)]
    [HttpDelete(Endpoints.LocationApi.Region.Province.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid province, CancellationToken token)
    {
        var deleted = await _provinceService.DeleteAsync(province, token);
        
        if (!deleted)
            return NotFound();
        
        return Ok();
    }
}