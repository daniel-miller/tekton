using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Tek.Service.Metadata;

namespace Tek.Api;

[ApiController]
[ApiExplorerSettings(GroupName = Endpoints.MetadataApi.Audit.Name)]
[Authorize]
public class VersionController : ControllerBase
{
    private readonly VersionService _versionService;

    public VersionController(VersionService versionService)
    {
        _versionService = versionService;
    }

    [Authorize(Endpoints.MetadataApi.Audit.Version.Assert)]
    [HttpHead(Endpoints.MetadataApi.Audit.Version.Assert)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> AssertAsync([FromRoute] int versionNumber, CancellationToken token)
    {
        var exists = await _versionService.AssertAsync(versionNumber, token);

        return Ok(exists);
    }

    [Authorize(Endpoints.MetadataApi.Audit.Version.Fetch)]
    [HttpGet(Endpoints.MetadataApi.Audit.Version.Fetch)]
    [ProducesResponseType(typeof(VersionModel), StatusCodes.Status200OK)]
    public async Task<ActionResult<VersionModel>> FetchAsync([FromRoute] int versionNumber, CancellationToken token)
    {
        var model = await _versionService.FetchAsync(versionNumber, token);

        if (model == null)
            return NotFound();

        return Ok(model);
    }

    [Authorize(Endpoints.MetadataApi.Audit.Version.Count)]
    [HttpGet(Endpoints.MetadataApi.Audit.Version.Count)]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> CountAsync([FromQuery] CountVersions query, CancellationToken token)
    {
        var count = await _versionService.CountAsync(query, token);

        return Ok(count);
    }

    [Authorize(Endpoints.MetadataApi.Audit.Version.Collect)]
    [HttpGet(Endpoints.MetadataApi.Audit.Version.Collect)]
    [ProducesResponseType(typeof(IEnumerable<VersionModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<VersionModel>>> CollectAsync([FromQuery] CollectVersions query, CancellationToken token)
    {
        var models = await _versionService.CollectAsync(query, token);

        var count = await _versionService.CountAsync(query, token);

        Response.AddPagination(query.Filter, models.Count(), count);

        return Ok(models);
    }

    [Authorize(Endpoints.MetadataApi.Audit.Version.Search)]
    [HttpGet(Endpoints.MetadataApi.Audit.Version.Search)]
    [ProducesResponseType(typeof(IEnumerable<VersionMatch>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<VersionMatch>>> SearchAsync([FromQuery] SearchVersions query, CancellationToken token)
    {
        var matches = await _versionService.SearchAsync(query, token);

        var count = await _versionService.CountAsync(query, token);

        Response.AddPagination(query.Filter, matches.Count(), count);

        return Ok(matches);
    }

    [Authorize(Endpoints.MetadataApi.Audit.Version.Create)]
    [HttpPost(Endpoints.MetadataApi.Audit.Version.Create)]
    [ProducesResponseType(typeof(VersionModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<VersionModel>> CreateAsync([FromBody] CreateVersion create, CancellationToken token)
    {
        var created = await _versionService.CreateAsync(create, token);

        if (!created)
            return BadRequest($"Duplicate not permitted: VersionNumber {create.VersionNumber}. You cannot insert a duplicate object with the same primary key.");

        var model = await _versionService.FetchAsync(create.VersionNumber, token);

        return CreatedAtAction(nameof(CreateAsync), model);
    }

    [Authorize(Endpoints.MetadataApi.Audit.Version.Modify)]
    [HttpPut(Endpoints.MetadataApi.Audit.Version.Modify)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ModifyAsync([FromBody] ModifyVersion modify, CancellationToken token)
    {
        var model = await _versionService.FetchAsync(modify.VersionNumber, token);

        if (model is null)
            return NotFound($"Version not found: VersionNumber {modify.VersionNumber}. You cannot modify an object that is not in the database.");

        var modified = await _versionService.ModifyAsync(modify, token);

        if (!modified)
            return NotFound();
        
        return Ok();
    }

    [Authorize(Endpoints.MetadataApi.Audit.Version.Delete)]
    [HttpDelete(Endpoints.MetadataApi.Audit.Version.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int versionNumber, CancellationToken token)
    {
        var deleted = await _versionService.DeleteAsync(versionNumber, token);
        
        if (!deleted)
            return NotFound();
        
        return Ok();
    }
}