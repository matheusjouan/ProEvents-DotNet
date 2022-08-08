using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProEvents.Domain.Model;

namespace ProEvents.Infra.EntitiesConfiguration
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.Property(x => x.Local)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("VARCHAR(50)");

            builder.Property(x => x.Thema)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("VARCHAR(50)");

            builder.Property(x => x.ImageUrl)
                .IsRequired()
                .HasMaxLength(250)
                .HasColumnType("VARCHAR(250)");

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(180)
                .HasColumnType("VARCHAR(180)");

            builder.Property(x => x.PhoneNumber)
                .IsRequired()
                .HasMaxLength(18)
                .HasColumnType("VARCHAR(18)");

            builder.HasMany(e => e.SocialNetworks)
                .WithOne(e => e.Event)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Batches)
                .WithOne(e => e.Event)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}