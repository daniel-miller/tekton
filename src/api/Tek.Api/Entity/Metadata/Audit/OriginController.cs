using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Tek.Service.Metadata;

namespace Tek.Api;

[ApiController]
[ApiExplorerSettings(GroupName = Endpoints.MetadataApi.Audit.Name)]
[Authorize]
public class OriginController : ControllerBase
{
    private readonly OriginService _originService;

    public OriginController(OriginService originService)
    {
        _originService = originService;
    }

    [Authorize(Endpoints.MetadataApi.Audit.Origin.Assert)]
    [HttpHead(Endpoints.MetadataApi.Audit.Origin.Assert)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> AssertAsync([FromRoute] Guid origin, CancellationToken token)
    {
        var exists = await _originService.AssertAsync(origin, token);

        return Ok(exists);
    }

    [Authorize(Endpoints.MetadataApi.Audit.Origin.Fetch)]
    [HttpGet(Endpoints.MetadataApi.Audit.Origin.Fetch)]
    [ProducesResponseType(typeof(OriginModel), StatusCodes.Status200OK)]
    public async Task<ActionResult<OriginModel>> FetchAsync([FromRoute] Guid origin, CancellationToken token)
    {
        var model = await _originService.FetchAsync(origin, token);

        if (model == null)
            return NotFound();

        return Ok(model);
    }

    [Authorize(Endpoints.MetadataApi.Audit.Origin.Count)]
    [HttpGet(Endpoints.MetadataApi.Audit.Origin.Count)]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> CountAsync([FromQuery] CountOrigins query, CancellationToken token)
    {
        var count = await _originService.CountAsync(query, token);

        return Ok(count);
    }

    [Authorize(Endpoints.MetadataApi.Audit.Origin.Collect)]
    [HttpGet(Endpoints.MetadataApi.Audit.Origin.Collect)]
    [ProducesResponseType(typeof(IEnumerable<OriginModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<OriginModel>>> CollectAsync([FromQuery] CollectOrigins query, CancellationToken token)
    {
        var models = await _originService.CollectAsync(query, token);

        var count = await _originService.CountAsync(query, token);

        Response.AddPagination(query.Filter, models.Count(), count);

        return Ok(models);
    }

    [Authorize(Endpoints.MetadataApi.Audit.Origin.Search)]
    [HttpGet(Endpoints.MetadataApi.Audit.Origin.Search)]
    [ProducesResponseType(typeof(IEnumerable<OriginMatch>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<OriginMatch>>> SearchAsync([FromQuery] SearchOrigins query, CancellationToken token)
    {
        var matches = await _originService.SearchAsync(query, token);

        var count = await _originService.CountAsync(query, token);

        Response.AddPagination(query.Filter, matches.Count(), count);

        return Ok(matches);
    }

    [Authorize(Endpoints.MetadataApi.Audit.Origin.Create)]
    [HttpPost(Endpoints.MetadataApi.Audit.Origin.Create)]
    [ProducesResponseType(typeof(OriginModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OriginModel>> CreateAsync([FromBody] CreateOrigin create, CancellationToken token)
    {
        var created = await _originService.CreateAsync(create, token);

        if (!created)
            return BadRequest($"Duplicate not permitted: OriginId {create.OriginId}. You cannot insert a duplicate object with the same primary key.");

        var model = await _originService.FetchAsync(create.OriginId, token);

        return CreatedAtAction(nameof(CreateAsync), model);
    }

    [Authorize(Endpoints.MetadataApi.Audit.Origin.Modify)]
    [HttpPut(Endpoints.MetadataApi.Audit.Origin.Modify)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ModifyAsync([FromBody] ModifyOrigin modify, CancellationToken token)
    {
        var model = await _originService.FetchAsync(modify.OriginId, token);

        if (model is null)
            return NotFound($"Origin not found: OriginId {modify.OriginId}. You cannot modify an object that is not in the database.");

        var modified = await _originService.ModifyAsync(modify, token);

        if (!modified)
            return NotFound();
        
        return Ok();
    }

    [Authorize(Endpoints.MetadataApi.Audit.Origin.Delete)]
    [HttpDelete(Endpoints.MetadataApi.Audit.Origin.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid origin, CancellationToken token)
    {
        var deleted = await _originService.DeleteAsync(origin, token);
        
        if (!deleted)
            return NotFound();
        
        return Ok();
    }
}