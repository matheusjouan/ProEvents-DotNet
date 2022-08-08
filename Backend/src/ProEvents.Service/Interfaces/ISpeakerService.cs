using ProEvents.Infra.Pagination;
using ProEvents.Service.DTOs;

namespace ProEvents.Service.Interfaces
{
    public interface ISpeakerService
    {
        Task<SpeakerDTO> AddSpeaker(int userId, SpeakerAddDTO model);
        Task<SpeakerDTO> UpdateSpeaker(int userId, SpeakerUpdateDTO model);
        Task<PageList<SpeakerDTO>> GetAllSpeakersAsync(PageParams pageParams, bool includeEvent = false);
        Task<SpeakerDTO> GetSpeakerByUserIdAsync(int userId, bool includeEvent = false);
    }
}