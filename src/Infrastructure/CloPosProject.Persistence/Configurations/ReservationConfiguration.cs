using CloPosProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloPosProject.Persistence.Configurations
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.ToTable("Reservations");
            builder.HasKey(r => r.Id);

            builder.Property(r => r.CustomerName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(r => r.CustomerPhone)
                .HasMaxLength(50);

            builder.Property(r => r.CustomerEmail)
                .HasMaxLength(200);

            builder.Property(r => r.GuestCount)
                .IsRequired();

            builder.Property(r => r.ReservationDate)
                .IsRequired();

            builder.Property(r => r.ReservationTime)
                .IsRequired();

            builder.Property(r => r.Status)
                .IsRequired();

            builder.Property(r => r.CreatedAt)
                .IsRequired();

            builder.HasOne(r => r.Table)
                .WithMany(t => t.Reservations)
                .HasForeignKey(r => r.TableId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
