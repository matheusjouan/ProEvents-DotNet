namespace ProEvents.Domain.Model;

public class EventSpeaker : BaseEntity
{
    public int SpeakerId { get; set; }
    public Speaker Speaker { get; set; }
    public int EventId { get; set; }
    public Event Event { get; set; }
}
