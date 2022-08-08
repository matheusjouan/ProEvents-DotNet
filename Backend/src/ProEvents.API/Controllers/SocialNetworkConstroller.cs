using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProEvents.Service.DTOs;
using ProEvents.Service.Interfaces;
using ProEvents.API.Extensions;


namespace ProEvents.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/[controller]")]
    public class SocialNetworkController : ControllerBase
    {
        private readonly ISocialNetworkService _socialNetworkService;
        private readonly IEventService _eventService;
        private readonly ISpeakerService _speakerService;

        public SocialNetworkController(ISocialNetworkService socialNetworkService,
        IEventService eventService, ISpeakerService speakerService)
        {
            _socialNetworkService = socialNetworkService;
            _eventService = eventService;
            _speakerService = speakerService;
        }

        [HttpGet("event/{eventId}")]
        public async Task<ActionResult<IEnumerable<SocialNetworkDTO>>> GetByEvent(int eventId)
        {
            try
            {
                if (!(await AuthorEvent(eventId)))
                    return Unauthorized();

                var socialNetworks = await _socialNetworkService.GetAllByEventIdAsync(eventId);
                if (socialNetworks == null) return NoContent();

                return Ok(socialNetworks);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        [HttpGet("speaker")]
        public async Task<ActionResult<IEnumerable<SocialNetworkDTO>>> GetBySpeaker()
        {
            try
            {
                var speaker = await _speakerService.GetSpeakerByUserIdAsync(User.GetUserId(), false);
                if (speaker == null) return Unauthorized();

                var socialNetworks = await _socialNetworkService.GetAllBySpeakerIdAsync(speaker.Id);
                if (socialNetworks == null) return NoContent();

                return Ok(socialNetworks);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        [HttpPut("speaker")]
        public async Task<IActionResult> SaveSocialNetworkByEvent([FromBody] SocialNetworkDTO[] models)
        {
            try
            {

                var speaker = await _speakerService.GetSpeakerByUserIdAsync(User.GetUserId(), false);
                if (speaker == null) return Unauthorized();

                var socialNetworks = await _socialNetworkService.SaveBySpeaker(speaker.Id, models);
                if (socialNetworks == null) return NoContent();

                return Ok(socialNetworks);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        [HttpPut("event/{eventId}")]
        public async Task<IActionResult> SaveSocialNetworkByEvent(int eventId, [FromBody] SocialNetworkDTO[] models)
        {
            try
            {
                if (!(await AuthorEvent(eventId)))
                    return Unauthorized();

                var socialNetworks = await _socialNetworkService.SaveByEvent(eventId, models);
                if (socialNetworks == null) return NoContent();

                return Ok(socialNetworks);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        [HttpDelete("event/{eventId}/{socialNetworkId}")]
        public async Task<IActionResult> DeleteByEvent(int eventId, int socialNetworkId)
        {
            try
            {
                if (!(await AuthorEvent(eventId)))
                    return Unauthorized();

                var socialNetwork = await _socialNetworkService.GetSocialNetworkEventByIdsAsync(eventId, socialNetworkId);
                if (socialNetwork == null) return NoContent();

                await _socialNetworkService.DeleteByEvent(eventId, socialNetwork.Id);
                return Ok(new { message = "SocialNetwork was deleted" });
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        [HttpDelete("speaker/{socialNetworkId}")]
        public async Task<IActionResult> DeleteBySpeaker(int socialNetworkId)
        {
            try
            {
                var speaker = await _speakerService.GetSpeakerByUserIdAsync(User.GetUserId(), false);
                if (speaker == null) return Unauthorized();

                var socialNetwork = await _socialNetworkService.GetSocialNetworkSpeakerByIdsAsync(speaker.Id, socialNetworkId);
                if (socialNetwork == null) return NoContent();

                await _socialNetworkService.DeleteBySpeaker(speaker.Id, socialNetwork.Id);
                return Ok(new { message = "SocialNetwork was deleted" });

            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        [NonAction]
        private async Task<bool> AuthorEvent(int eventId)
        {
            var evento = await _eventService.GetEventByIdAsync(User.GetUserId(), eventId, false);
            if (evento == null) return false;

            return true;
        }
    }
}