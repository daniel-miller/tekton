using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Tek.Service.Security;

namespace Tek.Api;

[ApiController]
[ApiExplorerSettings(GroupName = Endpoints.SecurityApi.Identification.Name)]
[Authorize]
public class PartitionController : ControllerBase
{
    private readonly PartitionService _partitionService;

    public PartitionController(PartitionService partitionService)
    {
        _partitionService = partitionService;
    }

    [Authorize(Endpoints.SecurityApi.Identification.Partition.Assert)]
    [HttpHead(Endpoints.SecurityApi.Identification.Partition.Assert)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> AssertAsync([FromRoute] int partitionNumber, CancellationToken token)
    {
        var exists = await _partitionService.AssertAsync(partitionNumber, token);

        return Ok(exists);
    }

    [Authorize(Endpoints.SecurityApi.Identification.Partition.Fetch)]
    [HttpGet(Endpoints.SecurityApi.Identification.Partition.Fetch)]
    [ProducesResponseType(typeof(PartitionModel), StatusCodes.Status200OK)]
    public async Task<ActionResult<PartitionModel>> FetchAsync([FromRoute] int partitionNumber, CancellationToken token)
    {
        var model = await _partitionService.FetchAsync(partitionNumber, token);

        if (model == null)
            return NotFound();

        return Ok(model);
    }

    [Authorize(Endpoints.SecurityApi.Identification.Partition.Count)]
    [HttpGet(Endpoints.SecurityApi.Identification.Partition.Count)]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> CountAsync([FromQuery] CountPartitions query, CancellationToken token)
    {
        var count = await _partitionService.CountAsync(query, token);

        return Ok(count);
    }

    [Authorize(Endpoints.SecurityApi.Identification.Partition.Collect)]
    [HttpGet(Endpoints.SecurityApi.Identification.Partition.Collect)]
    [ProducesResponseType(typeof(IEnumerable<PartitionModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PartitionModel>>> CollectAsync([FromQuery] CollectPartitions query, CancellationToken token)
    {
        var models = await _partitionService.CollectAsync(query, token);

        var count = await _partitionService.CountAsync(query, token);

        Response.AddPagination(query.Filter, models.Count(), count);

        return Ok(models);
    }

    [Authorize(Endpoints.SecurityApi.Identification.Partition.Search)]
    [HttpGet(Endpoints.SecurityApi.Identification.Partition.Search)]
    [ProducesResponseType(typeof(IEnumerable<PartitionMatch>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PartitionMatch>>> SearchAsync([FromQuery] SearchPartitions query, CancellationToken token)
    {
        var matches = await _partitionService.SearchAsync(query, token);

        var count = await _partitionService.CountAsync(query, token);

        Response.AddPagination(query.Filter, matches.Count(), count);

        return Ok(matches);
    }

    [Authorize(Endpoints.SecurityApi.Identification.Partition.Create)]
    [HttpPost(Endpoints.SecurityApi.Identification.Partition.Create)]
    [ProducesResponseType(typeof(PartitionModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PartitionModel>> CreateAsync([FromBody] CreatePartition create, CancellationToken token)
    {
        var created = await _partitionService.CreateAsync(create, token);

        if (!created)
            return BadRequest($"Duplicate not permitted: PartitionNumber {create.PartitionNumber}. You cannot insert a duplicate object with the same primary key.");

        var model = await _partitionService.FetchAsync(create.PartitionNumber, token);

        return CreatedAtAction(nameof(CreateAsync), model);
    }

    [Authorize(Endpoints.SecurityApi.Identification.Partition.Modify)]
    [HttpPut(Endpoints.SecurityApi.Identification.Partition.Modify)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ModifyAsync([FromBody] ModifyPartition modify, CancellationToken token)
    {
        var model = await _partitionService.FetchAsync(modify.PartitionNumber, token);

        if (model is null)
            return NotFound($"Partition not found: PartitionNumber {modify.PartitionNumber}. You cannot modify an object that is not in the database.");

        var modified = await _partitionService.ModifyAsync(modify, token);

        if (!modified)
            return NotFound();
        
        return Ok();
    }

    [Authorize(Endpoints.SecurityApi.Identification.Partition.Delete)]
    [HttpDelete(Endpoints.SecurityApi.Identification.Partition.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int partitionNumber, CancellationToken token)
    {
        var deleted = await _partitionService.DeleteAsync(partitionNumber, token);
        
        if (!deleted)
            return NotFound();
        
        return Ok();
    }
}