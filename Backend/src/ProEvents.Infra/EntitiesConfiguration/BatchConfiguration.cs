using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProEvents.Domain.Model;

namespace ProEvents.Infra.EntitiesConfiguration
{
    public class BatchConfiguration : IEntityTypeConfiguration<Batch>
    {
        public void Configure(EntityTypeBuilder<Batch> builder)
        {
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(180)
                .HasColumnType("VARCHAR(180)");

            builder.Property(x => x.Price)
                .IsRequired()
                .HasPrecision(10, 2);

            builder.Property(x => x.Amount)
                .IsRequired();

            builder.HasOne(b => b.Event)
                .WithMany(e => e.Batches);
        }
    }
}