using CloPosProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloPosProject.Persistence.Configurations
{
    public class SettingsConfiguration : IEntityTypeConfiguration<Settings>
    {
        public void Configure(EntityTypeBuilder<Settings> builder)
        {
            builder.ToTable("Settings");
            builder.HasKey(s => s.Id);

            builder.Property(s => s.RestaurantName)
                .HasMaxLength(500);

            builder.Property(s => s.Address)
                .HasMaxLength(1000);

            builder.Property(s => s.Phone)
                .HasMaxLength(100);

            builder.Property(s => s.TaxRate)
                .HasColumnType("decimal(5,2)");

            builder.Property(s => s.Currency)
                .HasMaxLength(50);

            builder.Property(s => s.OpeningTime)
                .IsRequired();

            builder.Property(s => s.ClosingTime)
                .IsRequired();

            builder.Property(s => s.DefaultReservationDuration)
                .IsRequired();

            builder.Property(s => s.EnableReservations)
                .IsRequired();
        }
    }
}
