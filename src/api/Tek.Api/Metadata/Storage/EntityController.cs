using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Tek.Service.Metadata;

namespace Tek.Api;

[ApiController]
[ApiExplorerSettings(GroupName = Endpoints.MetadataApi.Storage.Name)]
[Authorize]
public class EntityController : ControllerBase
{
    private readonly EntityService _entityService;

    public EntityController(EntityService entityService)
    {
        _entityService = entityService;
    }

    [Authorize(Endpoints.MetadataApi.Storage.Entity.Assert)]
    [HttpHead(Endpoints.MetadataApi.Storage.Entity.Assert)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> AssertAsync([FromRoute] Guid entity, CancellationToken token)
    {
        var exists = await _entityService.AssertAsync(entity, token);

        return Ok(exists);
    }

    [Authorize(Endpoints.MetadataApi.Storage.Entity.Fetch)]
    [HttpGet(Endpoints.MetadataApi.Storage.Entity.Fetch)]
    [ProducesResponseType(typeof(EntityModel), StatusCodes.Status200OK)]
    public async Task<ActionResult<EntityModel>> FetchAsync([FromRoute] Guid entity, CancellationToken token)
    {
        var model = await _entityService.FetchAsync(entity, token);

        if (model == null)
            return NotFound();

        return Ok(model);
    }

    [Authorize(Endpoints.MetadataApi.Storage.Entity.Count)]
    [HttpGet(Endpoints.MetadataApi.Storage.Entity.Count)]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> CountAsync([FromQuery] CountEntities query, CancellationToken token)
    {
        var count = await _entityService.CountAsync(query, token);

        return Ok(count);
    }

    [Authorize(Endpoints.MetadataApi.Storage.Entity.Collect)]
    [HttpGet(Endpoints.MetadataApi.Storage.Entity.Collect)]
    [ProducesResponseType(typeof(IEnumerable<EntityModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<EntityModel>>> CollectAsync([FromQuery] CollectEntities query, CancellationToken token)
    {
        var models = await _entityService.CollectAsync(query, token);

        var count = await _entityService.CountAsync(query, token);

        Response.AddPagination(query.Filter, models.Count(), count);

        return Ok(models);
    }

    [Authorize(Endpoints.MetadataApi.Storage.Entity.Search)]
    [HttpGet(Endpoints.MetadataApi.Storage.Entity.Search)]
    [ProducesResponseType(typeof(IEnumerable<EntityMatch>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<EntityMatch>>> SearchAsync([FromQuery] SearchEntities query, CancellationToken token)
    {
        var matches = await _entityService.SearchAsync(query, token);

        var count = await _entityService.CountAsync(query, token);

        Response.AddPagination(query.Filter, matches.Count(), count);

        return Ok(matches);
    }

    [Authorize(Endpoints.MetadataApi.Storage.Entity.Create)]
    [HttpPost(Endpoints.MetadataApi.Storage.Entity.Create)]
    [ProducesResponseType(typeof(EntityModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EntityModel>> CreateAsync([FromBody] CreateEntity create, CancellationToken token)
    {
        var created = await _entityService.CreateAsync(create, token);

        if (!created)
            return BadRequest($"Duplicate not permitted: EntityId {create.EntityId}. You cannot insert a duplicate object with the same primary key.");

        var model = await _entityService.FetchAsync(create.EntityId, token);

        return CreatedAtAction(nameof(CreateAsync), model);
    }

    [Authorize(Endpoints.MetadataApi.Storage.Entity.Modify)]
    [HttpPut(Endpoints.MetadataApi.Storage.Entity.Modify)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ModifyAsync([FromBody] ModifyEntity modify, CancellationToken token)
    {
        var model = await _entityService.FetchAsync(modify.EntityId, token);

        if (model is null)
            return NotFound($"Entity not found: EntityId {modify.EntityId}. You cannot modify an object that is not in the database.");

        var modified = await _entityService.ModifyAsync(modify, token);

        if (!modified)
            return NotFound();
        
        return Ok();
    }

    [Authorize(Endpoints.MetadataApi.Storage.Entity.Delete)]
    [HttpDelete(Endpoints.MetadataApi.Storage.Entity.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid entity, CancellationToken token)
    {
        var deleted = await _entityService.DeleteAsync(entity, token);
        
        if (!deleted)
            return NotFound();
        
        return Ok();
    }
}