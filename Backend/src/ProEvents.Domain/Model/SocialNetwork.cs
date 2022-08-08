namespace ProEvents.Domain.Model;

public class SocialNetwork : BaseEntity
{
    public string Name { get; set; }
    public string Url { get; set; }
    public int? EventId { get; set; }
    public Event Event { get; set; }
    public int? SpeakerId { get; set; }
    public Speaker Speaker { get; set; }

}
