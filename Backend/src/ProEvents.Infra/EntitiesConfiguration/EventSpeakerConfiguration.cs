using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProEvents.Domain.Model;

namespace ProEvents.Infra.EntitiesConfiguration
{
    public class EventSpeakerConfiguration : IEntityTypeConfiguration<EventSpeaker>
    {
        public void Configure(EntityTypeBuilder<EventSpeaker> builder)
        {
            // Classe do meio relação N:N
            builder.HasKey(SP => new { SP.EventId, SP.SpeakerId });
        }
    }
}