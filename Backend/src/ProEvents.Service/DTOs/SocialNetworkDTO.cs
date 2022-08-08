using ProEvents.Domain.Model;

namespace ProEvents.Service.DTOs
{
    public class SocialNetworkDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int? EventId { get; set; }
        public EventDTO Event { get; set; }
        public int? SpeakerId { get; set; }
        public SpeakerDTO Speaker { get; set; }
    }
}