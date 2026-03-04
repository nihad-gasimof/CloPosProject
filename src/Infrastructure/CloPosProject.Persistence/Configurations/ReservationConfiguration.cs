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

                builder.Property(r => r.TableId)
                    .IsRequired();

                builder.Property(r => r.CustomerName)
                    .IsRequired()
                    .HasMaxLength(200);

                builder.Property(r => r.CustomerPhone)
                    .IsRequired()
                    .HasMaxLength(20);

                builder.Property(r => r.CustomerEmail)
                    .HasMaxLength(200);

                builder.Property(r => r.GuestCount)
                    .IsRequired();

                builder.Property(r => r.ReservationDate)
                    .IsRequired();

                builder.Property(r => r.ReservationTime)
                    .IsRequired();

                builder.Property(r => r.DurationMinutes)
                    .IsRequired()
                    .HasDefaultValue(120); // Default 2 saat

                builder.Property(r => r.Status)
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                builder.Property(r => r.SpecialRequests)
                    .HasMaxLength(1000);

                builder.Property(r => r.CancellationReason)
                    .HasMaxLength(500);

                builder.Property(r => r.CreatedAt)
                    .IsRequired();

                builder.HasIndex(r => r.TableId);
                builder.HasIndex(r => r.ReservationDate);
                builder.HasIndex(r => r.Status);
                builder.HasIndex(r => r.CustomerPhone);

                // Composite index - tarix və vaxt üzrə axtarış
                builder.HasIndex(r => new { r.ReservationDate, r.ReservationTime });
            builder.Ignore(r => r.ReservationDateTime);
            builder.Ignore(r => r.EstimatedEndTime);
            builder.HasOne(r => r.Table)
                    .WithMany(t => t.Reservations)
                    .HasForeignKey(r => r.TableId)
                    .OnDelete(DeleteBehavior.Restrict);
            }
        }
    }
