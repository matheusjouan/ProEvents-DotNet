using ProEvents.Service.DTOs;

namespace ProEvents.Service.Interfaces
{
    public interface ISocialNetworkService
    {
        Task<IEnumerable<SocialNetworkDTO>> SaveByEvent(int eventId, SocialNetworkDTO[] models);
        Task<bool> DeleteByEvent(int eventId, int socialNetworkId);
        Task<IEnumerable<SocialNetworkDTO>> SaveBySpeaker(int speakerId, SocialNetworkDTO[] models);
        Task<bool> DeleteBySpeaker(int speakerId, int socialNetworkId);
        Task<IEnumerable<SocialNetworkDTO>> GetAllByEventIdAsync(int eventId);
        Task<IEnumerable<SocialNetworkDTO>> GetAllBySpeakerIdAsync(int speakerId);
        Task<SocialNetworkDTO> GetSocialNetworkEventByIdsAsync(int eventId, int socialNetworkId);
        Task<SocialNetworkDTO> GetSocialNetworkSpeakerByIdsAsync(int speakerId, int socialNetworkId);
    }
}