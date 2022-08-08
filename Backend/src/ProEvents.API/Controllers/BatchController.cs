using Microsoft.AspNetCore.Mvc;
using ProEvents.Domain.Model;
using ProEvents.Service.DTOs;
using ProEvents.Service.Interfaces;

namespace ProEvents.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BatchController : ControllerBase
{
    private readonly IBatchService _batchService;

    public BatchController(IBatchService batchService)
    {
        _batchService = batchService;
    }

    [HttpGet("{eventId}")]
    public async Task<ActionResult<IEnumerable<BatchDTO>>> Get(int eventId)
    {
        try
        {
            var batches = await _batchService.GetBatchesByEventIdAsync(eventId);
            if (batches == null) return BadRequest("Batches not found associated with EventId");

            return Ok(batches);
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Error: {e.Message}");
        }
    }

    [HttpPut("{eventId}")]
    public async Task<ActionResult<IEnumerable<BatchDTO>>> SaveBatches(int eventId, [FromBody] IEnumerable<BatchDTO> modelsDtos)
    {
        try
        {
            var batches = await _batchService.SaveBatches(eventId, modelsDtos);
            if (batches == null) return BadRequest("Error trying to save event");

            return Ok(batches);
        }
        catch (Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Error: {e.Message}");
        }
    }

    [HttpDelete("{eventId}/{batchId}")]
    public async Task<ActionResult> Delete(int eventId, int batchId)
    {
        try
        {
            if (await _batchService.DeleteBatch(eventId, batchId))
                return NoContent();

            return BadRequest("Error trying to delete event");
        }
        catch (Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Error: {e.Message}");
        }
    }
}
