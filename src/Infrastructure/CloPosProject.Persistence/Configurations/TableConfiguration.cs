using CloPosProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloPosProject.Persistence.Configurations
{
        public class TableConfiguration : IEntityTypeConfiguration<Table>
        {
            public void Configure(EntityTypeBuilder<Table> builder)
            {
                builder.ToTable("Tables");
                builder.HasKey(t => t.Id);

                builder.Property(t => t.TableNumber)
                    .IsRequired()
                    .HasMaxLength(20);

                builder.Property(t => t.Capacity)
                    .IsRequired();

                builder.Property(t => t.Status)
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                builder.Property(t => t.IsActive)
                    .IsRequired()
                    .HasDefaultValue(true);

                builder.Property(t => t.Location)
                    .HasMaxLength(100);

                builder.HasIndex(t => t.TableNumber).IsUnique();
                builder.HasIndex(t => t.Status);
                builder.HasIndex(t => t.IsActive);

                builder.HasMany(t => t.Orders)
                    .WithOne()
                    .HasForeignKey(o => o.TableId)
                    .OnDelete(DeleteBehavior.Restrict);

                builder.HasMany(t => t.Reservations)
                    .WithOne(r => r.Table)
                    .HasForeignKey(r => r.TableId)
                    .OnDelete(DeleteBehavior.Restrict);
            }
        }
        }
