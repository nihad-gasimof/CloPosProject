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
                .HasMaxLength(50);

            builder.Property(t => t.Capacity)
                .IsRequired();

            builder.Property(t => t.Status)
                .IsRequired();

            builder.Property(t => t.IsActive)
                .HasDefaultValue(true);

            builder.HasMany(t => t.Orders)
                .WithOne(o => o.Table)
                .HasForeignKey(o => o.TableId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.Reservations)
                .WithOne(r => r.Table)
                .HasForeignKey(r => r.TableId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
