using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Tek.Service.Security;

namespace Tek.Api;

[ApiController]
[ApiExplorerSettings(GroupName = Endpoints.SecurityApi.Identification.Name)]
[Authorize]
public class SecretController : ControllerBase
{
    private readonly SecretService _secretService;

    public SecretController(SecretService secretService)
    {
        _secretService = secretService;
    }

    [Authorize(Endpoints.SecurityApi.Identification.Secret.Assert)]
    [HttpHead(Endpoints.SecurityApi.Identification.Secret.Assert)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> AssertAsync([FromRoute] Guid secret, CancellationToken token)
    {
        var exists = await _secretService.AssertAsync(secret, token);

        return Ok(exists);
    }

    [Authorize(Endpoints.SecurityApi.Identification.Secret.Fetch)]
    [HttpGet(Endpoints.SecurityApi.Identification.Secret.Fetch)]
    [ProducesResponseType(typeof(SecretModel), StatusCodes.Status200OK)]
    public async Task<ActionResult<SecretModel>> FetchAsync([FromRoute] Guid secret, CancellationToken token)
    {
        var model = await _secretService.FetchAsync(secret, token);

        if (model == null)
            return NotFound();

        return Ok(model);
    }

    [Authorize(Endpoints.SecurityApi.Identification.Secret.Count)]
    [HttpGet(Endpoints.SecurityApi.Identification.Secret.Count)]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> CountAsync([FromQuery] CountSecrets query, CancellationToken token)
    {
        var count = await _secretService.CountAsync(query, token);

        return Ok(count);
    }

    [Authorize(Endpoints.SecurityApi.Identification.Secret.Collect)]
    [HttpGet(Endpoints.SecurityApi.Identification.Secret.Collect)]
    [ProducesResponseType(typeof(IEnumerable<SecretModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SecretModel>>> CollectAsync([FromQuery] CollectSecrets query, CancellationToken token)
    {
        var models = await _secretService.CollectAsync(query, token);

        var count = await _secretService.CountAsync(query, token);

        Response.AddPagination(query.Filter, models.Count(), count);

        return Ok(models);
    }

    [Authorize(Endpoints.SecurityApi.Identification.Secret.Search)]
    [HttpGet(Endpoints.SecurityApi.Identification.Secret.Search)]
    [ProducesResponseType(typeof(IEnumerable<SecretMatch>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SecretMatch>>> SearchAsync([FromQuery] SearchSecrets query, CancellationToken token)
    {
        var matches = await _secretService.SearchAsync(query, token);

        var count = await _secretService.CountAsync(query, token);

        Response.AddPagination(query.Filter, matches.Count(), count);

        return Ok(matches);
    }

    [Authorize(Endpoints.SecurityApi.Identification.Secret.Create)]
    [HttpPost(Endpoints.SecurityApi.Identification.Secret.Create)]
    [ProducesResponseType(typeof(SecretModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SecretModel>> CreateAsync([FromBody] CreateSecret create, CancellationToken token)
    {
        var created = await _secretService.CreateAsync(create, token);

        if (!created)
            return BadRequest($"Duplicate not permitted: SecretId {create.SecretId}. You cannot insert a duplicate object with the same primary key.");

        var model = await _secretService.FetchAsync(create.SecretId, token);

        return CreatedAtAction(nameof(CreateAsync), model);
    }

    [Authorize(Endpoints.SecurityApi.Identification.Secret.Modify)]
    [HttpPut(Endpoints.SecurityApi.Identification.Secret.Modify)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ModifyAsync([FromBody] ModifySecret modify, CancellationToken token)
    {
        var model = await _secretService.FetchAsync(modify.SecretId, token);

        if (model is null)
            return NotFound($"Secret not found: SecretId {modify.SecretId}. You cannot modify an object that is not in the database.");

        var modified = await _secretService.ModifyAsync(modify, token);

        if (!modified)
            return NotFound();
        
        return Ok();
    }

    [Authorize(Endpoints.SecurityApi.Identification.Secret.Delete)]
    [HttpDelete(Endpoints.SecurityApi.Identification.Secret.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid secret, CancellationToken token)
    {
        var deleted = await _secretService.DeleteAsync(secret, token);
        
        if (!deleted)
            return NotFound();
        
        return Ok();
    }
}