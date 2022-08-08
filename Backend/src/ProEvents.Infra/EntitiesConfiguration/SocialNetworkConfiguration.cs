using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProEvents.Domain.Model;

namespace ProEvents.Infra.EntitiesConfiguration
{
    public class SocialNetworkConfiguration : IEntityTypeConfiguration<SocialNetwork>
    {
        public void Configure(EntityTypeBuilder<SocialNetwork> builder)
        {
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(180)
                .HasColumnType("VARCHAR(180)");

            builder.Property(x => x.Url)
                .IsRequired()
                .HasMaxLength(250)
                .HasColumnType("VARCHAR(250)");

            builder.HasOne(s => s.Event)
                .WithMany(e => e.SocialNetworks);

            builder.HasOne(sn => sn.Speaker)
                .WithMany(s => s.SocialNetworks);
        }
    }
}