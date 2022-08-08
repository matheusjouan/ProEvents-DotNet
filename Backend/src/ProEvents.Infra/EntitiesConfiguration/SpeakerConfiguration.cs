using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProEvents.Domain.Model;

namespace ProEvents.Infra.EntitiesConfiguration
{
    public class SpeakerConfiguration : IEntityTypeConfiguration<Speaker>
    {
        public void Configure(EntityTypeBuilder<Speaker> builder)
        {
            builder.HasMany(speaker => speaker.SocialNetworks)
                .WithOne(sn => sn.Speaker);
        }
    }
}