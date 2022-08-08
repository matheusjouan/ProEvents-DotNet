using ProEvents.Domain.Identity;

namespace ProEvents.Domain.Model;

public class Event : BaseEntity
{
    public string Local { get; set; }
    public DateTime? EventDate { get; set; }
    public string Thema { get; set; }
    public int AmountPeople { get; set; }
    public string ImageUrl { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }

    // Identity
    public int UserId { get; set; }
    public User User { get; set; }
    //
    public IEnumerable<Batch> Batches { get; set; }
    public IEnumerable<SocialNetwork> SocialNetworks { get; set; }
    public IEnumerable<EventSpeaker> EventsSpeakers { get; set; }
}
