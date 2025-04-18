using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Tek.Service.Security;

namespace Tek.Api;

[ApiController]
[ApiExplorerSettings(GroupName = Endpoints.SecurityApi.Authorization.Name)]
[Authorize]
public class ResourceController : ControllerBase
{
    private readonly ResourceService _resourceService;

    public ResourceController(ResourceService resourceService)
    {
        _resourceService = resourceService;
    }

    [Authorize(Endpoints.SecurityApi.Authorization.Resource.Assert)]
    [HttpHead(Endpoints.SecurityApi.Authorization.Resource.Assert)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> AssertAsync([FromRoute] Guid resource, CancellationToken token)
    {
        var exists = await _resourceService.AssertAsync(resource, token);

        return Ok(exists);
    }

    [Authorize(Endpoints.SecurityApi.Authorization.Resource.Fetch)]
    [HttpGet(Endpoints.SecurityApi.Authorization.Resource.Fetch)]
    [ProducesResponseType(typeof(ResourceModel), StatusCodes.Status200OK)]
    public async Task<ActionResult<ResourceModel>> FetchAsync([FromRoute] Guid resource, CancellationToken token)
    {
        var model = await _resourceService.FetchAsync(resource, token);

        if (model == null)
            return NotFound();

        return Ok(model);
    }

    [Authorize(Endpoints.SecurityApi.Authorization.Resource.Count)]
    [HttpGet(Endpoints.SecurityApi.Authorization.Resource.Count)]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> CountAsync([FromQuery] CountResources query, CancellationToken token)
    {
        var count = await _resourceService.CountAsync(query, token);

        return Ok(count);
    }

    [Authorize(Endpoints.SecurityApi.Authorization.Resource.Collect)]
    [HttpGet(Endpoints.SecurityApi.Authorization.Resource.Collect)]
    [ProducesResponseType(typeof(IEnumerable<ResourceModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ResourceModel>>> CollectAsync([FromQuery] CollectResources query, CancellationToken token)
    {
        var models = await _resourceService.CollectAsync(query, token);

        var count = await _resourceService.CountAsync(query, token);

        Response.AddPagination(query.Filter, models.Count(), count);

        return Ok(models);
    }

    [Authorize(Endpoints.SecurityApi.Authorization.Resource.Search)]
    [HttpGet(Endpoints.SecurityApi.Authorization.Resource.Search)]
    [ProducesResponseType(typeof(IEnumerable<ResourceMatch>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ResourceMatch>>> SearchAsync([FromQuery] SearchResources query, CancellationToken token)
    {
        var matches = await _resourceService.SearchAsync(query, token);

        var count = await _resourceService.CountAsync(query, token);

        Response.AddPagination(query.Filter, matches.Count(), count);

        return Ok(matches);
    }

    [Authorize(Endpoints.SecurityApi.Authorization.Resource.Create)]
    [HttpPost(Endpoints.SecurityApi.Authorization.Resource.Create)]
    [ProducesResponseType(typeof(ResourceModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ResourceModel>> CreateAsync([FromBody] CreateResource create, CancellationToken token)
    {
        var created = await _resourceService.CreateAsync(create, token);

        if (!created)
            return BadRequest($"Duplicate not permitted: ResourceId {create.ResourceId}. You cannot insert a duplicate object with the same primary key.");

        var model = await _resourceService.FetchAsync(create.ResourceId, token);

        return CreatedAtAction(nameof(CreateAsync), model);
    }

    [Authorize(Endpoints.SecurityApi.Authorization.Resource.Modify)]
    [HttpPut(Endpoints.SecurityApi.Authorization.Resource.Modify)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ModifyAsync([FromBody] ModifyResource modify, CancellationToken token)
    {
        var model = await _resourceService.FetchAsync(modify.ResourceId, token);

        if (model is null)
            return NotFound($"Resource not found: ResourceId {modify.ResourceId}. You cannot modify an object that is not in the database.");

        var modified = await _resourceService.ModifyAsync(modify, token);

        if (!modified)
            return NotFound();
        
        return Ok();
    }

    [Authorize(Endpoints.SecurityApi.Authorization.Resource.Delete)]
    [HttpDelete(Endpoints.SecurityApi.Authorization.Resource.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid resource, CancellationToken token)
    {
        var deleted = await _resourceService.DeleteAsync(resource, token);
        
        if (!deleted)
            return NotFound();
        
        return Ok();
    }
}