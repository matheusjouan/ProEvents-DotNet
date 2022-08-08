using ProEvents.Domain.Model;

namespace ProEvents.Infra.Interface
{
    public interface ISocialNetworkRepository : IBaseRepository<SocialNetwork>
    {
        Task<SocialNetwork> GetSocialNetworkEventByIdAsync(int eventId, int id);
        Task<SocialNetwork> GetSocialNetworkSpeakerByIdAsync(int speakerId, int id);
        Task<IEnumerable<SocialNetwork>> GetAllByEventIdAsync(int eventId);
        Task<IEnumerable<SocialNetwork>> GetAllBySpeakerIdAsync(int speakerId);
    }
}