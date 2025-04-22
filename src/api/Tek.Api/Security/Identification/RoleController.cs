using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Tek.Service.Security;

namespace Tek.Api;

[ApiController]
[ApiExplorerSettings(GroupName = Endpoints.SecurityApi.Identification.Name)]
[Authorize]
public class RoleController : ControllerBase
{
    private readonly RoleService _roleService;

    public RoleController(RoleService roleService)
    {
        _roleService = roleService;
    }

    [Authorize(Endpoints.SecurityApi.Identification.Role.Assert)]
    [HttpHead(Endpoints.SecurityApi.Identification.Role.Assert)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> AssertAsync([FromRoute] Guid role, CancellationToken token)
    {
        var exists = await _roleService.AssertAsync(role, token);

        return Ok(exists);
    }

    [Authorize(Endpoints.SecurityApi.Identification.Role.Fetch)]
    [HttpGet(Endpoints.SecurityApi.Identification.Role.Fetch)]
    [ProducesResponseType(typeof(RoleModel), StatusCodes.Status200OK)]
    public async Task<ActionResult<RoleModel>> FetchAsync([FromRoute] Guid role, CancellationToken token)
    {
        var model = await _roleService.FetchAsync(role, token);

        if (model == null)
            return NotFound();

        return Ok(model);
    }

    [Authorize(Endpoints.SecurityApi.Identification.Role.Count)]
    [HttpGet(Endpoints.SecurityApi.Identification.Role.Count)]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> CountAsync([FromQuery] CountRoles query, CancellationToken token)
    {
        var count = await _roleService.CountAsync(query, token);

        return Ok(count);
    }

    [Authorize(Endpoints.SecurityApi.Identification.Role.Collect)]
    [HttpGet(Endpoints.SecurityApi.Identification.Role.Collect)]
    [ProducesResponseType(typeof(IEnumerable<RoleModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RoleModel>>> CollectAsync([FromQuery] CollectRoles query, CancellationToken token)
    {
        var models = await _roleService.CollectAsync(query, token);

        var count = await _roleService.CountAsync(query, token);

        Response.AddPagination(query.Filter, models.Count(), count);

        return Ok(models);
    }

    [Authorize(Endpoints.SecurityApi.Identification.Role.Search)]
    [HttpGet(Endpoints.SecurityApi.Identification.Role.Search)]
    [ProducesResponseType(typeof(IEnumerable<RoleMatch>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RoleMatch>>> SearchAsync([FromQuery] SearchRoles query, CancellationToken token)
    {
        var matches = await _roleService.SearchAsync(query, token);

        var count = await _roleService.CountAsync(query, token);

        Response.AddPagination(query.Filter, matches.Count(), count);

        return Ok(matches);
    }

    [Authorize(Endpoints.SecurityApi.Identification.Role.Create)]
    [HttpPost(Endpoints.SecurityApi.Identification.Role.Create)]
    [ProducesResponseType(typeof(RoleModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RoleModel>> CreateAsync([FromBody] CreateRole create, CancellationToken token)
    {
        var created = await _roleService.CreateAsync(create, token);

        if (!created)
            return BadRequest($"Duplicate not permitted: RoleId {create.RoleId}. You cannot insert a duplicate object with the same primary key.");

        var model = await _roleService.FetchAsync(create.RoleId, token);

        return CreatedAtAction(nameof(CreateAsync), model);
    }

    [Authorize(Endpoints.SecurityApi.Identification.Role.Modify)]
    [HttpPut(Endpoints.SecurityApi.Identification.Role.Modify)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ModifyAsync([FromBody] ModifyRole modify, CancellationToken token)
    {
        var model = await _roleService.FetchAsync(modify.RoleId, token);

        if (model is null)
            return NotFound($"Role not found: RoleId {modify.RoleId}. You cannot modify an object that is not in the database.");

        var modified = await _roleService.ModifyAsync(modify, token);

        if (!modified)
            return NotFound();
        
        return Ok();
    }

    [Authorize(Endpoints.SecurityApi.Identification.Role.Delete)]
    [HttpDelete(Endpoints.SecurityApi.Identification.Role.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid role, CancellationToken token)
    {
        var deleted = await _roleService.DeleteAsync(role, token);
        
        if (!deleted)
            return NotFound();
        
        return Ok();
    }
}