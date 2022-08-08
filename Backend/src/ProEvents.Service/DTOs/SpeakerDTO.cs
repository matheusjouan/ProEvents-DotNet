using ProEvents.Domain.Model;

namespace ProEvents.Service.DTOs
{
    public class SpeakerDTO
    {
        public int Id { get; set; }
        public string Resume { get; set; }
        public int UserId { get; set; }
        public UserUpdateDTO User { get; set; }
        public IEnumerable<SocialNetworkDTO> SocialNetworks { get; set; }
        public IEnumerable<EventDTO> Events { get; set; }
    }
}