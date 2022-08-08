using ProEvents.Domain.Model;

namespace ProEvents.Service.DTOs
{
    public class SpeakerUpdateDTO
    {
        public int Id { get; set; }
        public string Resume { get; set; }
        public int UserId { get; set; }
    }
}