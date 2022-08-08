using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProEvents.API.Extensions;
using ProEvents.Infra.Pagination;
using ProEvents.Service.DTOs;
using ProEvents.Service.Interfaces;

namespace ProEvents.API.Controllers
{

    [ApiController]
    [Authorize]
    [Route("api/v1/[controller]")]
    public class SpeakerController : ControllerBase
    {
        private readonly ISpeakerService _speakerService;
        private readonly IUserService _userServive;

        public SpeakerController(ISpeakerService speakerService, IUserService userServive)
        {
            _speakerService = speakerService;
            _userServive = userServive;
        }

        [HttpGet("all")]
        public async Task<ActionResult<PageList<SpeakerDTO>>> GetAll([FromQuery] PageParams pageParams)
        {
            try
            {
                var speakers = await _speakerService.GetAllSpeakersAsync(pageParams, true);
                if (speakers == null) return NoContent();

                Response.AddPagination(
                    speakers.CurrentPage, speakers.PageSize, speakers.TotalCount, speakers.TotalPages
                );

                return Ok(speakers);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Error to get speakers. Error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult<SpeakerDTO>> GetSpeaker()
        {
            try
            {
                var speaker = await _speakerService.GetSpeakerByUserIdAsync(User.GetUserId(), true);
                if (speaker == null) return NoContent();

                return Ok(speaker);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Error to get speakers. Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<SpeakerDTO>> CreateSpeaker(SpeakerAddDTO model)
        {
            try
            {
                var speaker = await _speakerService.GetSpeakerByUserIdAsync(User.GetUserId(), false);
                if (speaker == null)
                    speaker = await _speakerService.AddSpeaker(User.GetUserId(), model);

                return Ok(speaker);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Error to get speakers. Error: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<ActionResult<SpeakerDTO>> UpdateSpeaker(SpeakerUpdateDTO model)
        {
            try
            {
                var speaker = await _speakerService.UpdateSpeaker(User.GetUserId(), model);
                if (speaker == null) return NoContent();

                return Ok(speaker);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Error to get speakers. Error: {ex.Message}");
            }
        }

    }
}