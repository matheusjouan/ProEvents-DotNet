using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProEvents.API.Extensions;
using ProEvents.Domain.Model;
using ProEvents.Infra.Pagination;
using ProEvents.Service.DTOs;
using ProEvents.Service.Interfaces;

namespace ProEvents.API.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;
    private readonly IUserService _userServive;

    public EventController(IEventService eventService,
        IUserService userServive)
    {
        _eventService = eventService;
        _userServive = userServive;
    }

    [HttpGet]
    public async Task<ActionResult<PageList<EventDTO>>> Get([FromQuery] PageParams pageParams)
    {
        try
        {
            var events = await _eventService.GetAllEventsAsync(User.GetUserId(), pageParams, true);
            if (events == null) return NotFound("Events not found");

            Response.AddPagination(events.CurrentPage, events.PageSize, events.TotalCount, events.TotalPages);

            return Ok(events);
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Error: {e.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EventDTO>> GetById(int id)
    {
        try
        {
            var evt = await _eventService.GetEventByIdAsync(User.GetUserId(), id, true);
            if (evt == null) return NotFound("Event not found");

            return Ok(evt);
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Error: {e.Message}");
        }
    }

    [HttpPost("upload-image/{eventId}")]
    public async Task<ActionResult> UploadImage(int eventId)
    {
        try
        {
            var evt = await _eventService.GetEventByIdAsync(User.GetUserId(), eventId);
            if (evt == null) return NotFound("EventId not exist");

            var file = Request.Form.Files[0];
            if (file.Length > 0)
            {
                // Delete Imagem antiga, caso tiver uma
                _eventService.DeleteImage(evt.ImageUrl, "Images");
                // Salvando a nova ou primeira imagem
                evt.ImageUrl = await _eventService.SaveImage(file, "Images");
            }

            var evtResult = await _eventService.UpdateEvent(User.GetUserId(), eventId, evt);
            return NoContent();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<EventDTO>> Post([FromBody] EventDTO modelDto)
    {
        try
        {
            var newEvent = await _eventService.AddEvent(User.GetUserId(), modelDto);
            if (newEvent == null) return BadRequest("Error trying to add event");

            return CreatedAtAction(nameof(GetById), new { id = newEvent.Id }, newEvent);
            // return StatusCode(StatusCodes.Status201Created, newEvent);
        }
        catch (Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Error: {e.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<EventDTO>> Put(int id, [FromBody] EventDTO modelDto)
    {
        try
        {
            var updatedEvent = await _eventService.UpdateEvent(User.GetUserId(), id, modelDto);
            if (updatedEvent == null) return BadRequest("Error trying to update event");

            return Ok(updatedEvent);
        }
        catch (Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Error: {e.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var evt = await _eventService.GetEventByIdAsync(User.GetUserId(), id);
            if (await _eventService.DeleteEvent(User.GetUserId(), id))
            {
                _eventService.DeleteImage(evt.ImageUrl, "Images");
                return NoContent();
            }

            return BadRequest("Error trying to delete event");
        }
        catch (Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Error: {e.Message}");
        }
    }
}
