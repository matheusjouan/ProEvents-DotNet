using ProEvents.Domain.Identity;

namespace ProEvents.Domain.Model
{
    public class Speaker : BaseEntity
    {
        public string Resume { get; set; }

        // Identity
        public int UserId { get; set; }
        public User User { get; set; }
        //
        public IEnumerable<SocialNetwork> SocialNetworks { get; set; }
        public IEnumerable<EventSpeaker> EventsSpeakers { get; set; }
    }
}