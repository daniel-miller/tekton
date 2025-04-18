using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Tek.Service.Content;

namespace Tek.Api;

[ApiController]
[ApiExplorerSettings(GroupName = Endpoints.ContentApi.Text.Name)]
[Authorize]
public class TranslationController : ControllerBase
{
    private readonly TranslationService _translationService;

    public TranslationController(TranslationService translationService)
    {
        _translationService = translationService;
    }

    [Authorize(Endpoints.ContentApi.Text.Translation.Assert)]
    [HttpHead(Endpoints.ContentApi.Text.Translation.Assert)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> AssertAsync([FromRoute] Guid translation, CancellationToken token)
    {
        var exists = await _translationService.AssertAsync(translation, token);

        return Ok(exists);
    }

    [Authorize(Endpoints.ContentApi.Text.Translation.Fetch)]
    [HttpGet(Endpoints.ContentApi.Text.Translation.Fetch)]
    [ProducesResponseType(typeof(TranslationModel), StatusCodes.Status200OK)]
    public async Task<ActionResult<TranslationModel>> FetchAsync([FromRoute] Guid translation, CancellationToken token)
    {
        var model = await _translationService.FetchAsync(translation, token);

        if (model == null)
            return NotFound();

        return Ok(model);
    }

    [Authorize(Endpoints.ContentApi.Text.Translation.Count)]
    [HttpGet(Endpoints.ContentApi.Text.Translation.Count)]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> CountAsync([FromQuery] CountTranslations query, CancellationToken token)
    {
        var count = await _translationService.CountAsync(query, token);

        return Ok(count);
    }

    [Authorize(Endpoints.ContentApi.Text.Translation.Collect)]
    [HttpGet(Endpoints.ContentApi.Text.Translation.Collect)]
    [ProducesResponseType(typeof(IEnumerable<TranslationModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TranslationModel>>> CollectAsync([FromQuery] CollectTranslations query, CancellationToken token)
    {
        var models = await _translationService.CollectAsync(query, token);

        var count = await _translationService.CountAsync(query, token);

        Response.AddPagination(query.Filter, models.Count(), count);

        return Ok(models);
    }

    [Authorize(Endpoints.ContentApi.Text.Translation.Search)]
    [HttpGet(Endpoints.ContentApi.Text.Translation.Search)]
    [ProducesResponseType(typeof(IEnumerable<TranslationMatch>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TranslationMatch>>> SearchAsync([FromQuery] SearchTranslations query, CancellationToken token)
    {
        var matches = await _translationService.SearchAsync(query, token);

        var count = await _translationService.CountAsync(query, token);

        Response.AddPagination(query.Filter, matches.Count(), count);

        return Ok(matches);
    }

    [Authorize(Endpoints.ContentApi.Text.Translation.Create)]
    [HttpPost(Endpoints.ContentApi.Text.Translation.Create)]
    [ProducesResponseType(typeof(TranslationModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TranslationModel>> CreateAsync([FromBody] CreateTranslation create, CancellationToken token)
    {
        var created = await _translationService.CreateAsync(create, token);

        if (!created)
            return BadRequest($"Duplicate not permitted: TranslationId {create.TranslationId}. You cannot insert a duplicate object with the same primary key.");

        var model = await _translationService.FetchAsync(create.TranslationId, token);

        return CreatedAtAction(nameof(CreateAsync), model);
    }

    [Authorize(Endpoints.ContentApi.Text.Translation.Modify)]
    [HttpPut(Endpoints.ContentApi.Text.Translation.Modify)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ModifyAsync([FromBody] ModifyTranslation modify, CancellationToken token)
    {
        var model = await _translationService.FetchAsync(modify.TranslationId, token);

        if (model is null)
            return NotFound($"Translation not found: TranslationId {modify.TranslationId}. You cannot modify an object that is not in the database.");

        var modified = await _translationService.ModifyAsync(modify, token);

        if (!modified)
            return NotFound();
        
        return Ok();
    }

    [Authorize(Endpoints.ContentApi.Text.Translation.Delete)]
    [HttpDelete(Endpoints.ContentApi.Text.Translation.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid translation, CancellationToken token)
    {
        var deleted = await _translationService.DeleteAsync(translation, token);
        
        if (!deleted)
            return NotFound();
        
        return Ok();
    }
}