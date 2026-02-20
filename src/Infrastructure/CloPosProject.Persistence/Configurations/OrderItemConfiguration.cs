using CloPosProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloPosProject.Persistence.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems");
            builder.HasKey(oi => oi.Id);

            builder.Property(oi => oi.MenuItemName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(oi => oi.UnitPrice)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(oi => oi.Quantity)
                .IsRequired();

            builder.Property(oi => oi.Subtotal)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(oi => oi.SpecialInstructions)
                .HasMaxLength(500);

            builder.HasIndex(oi => oi.OrderId);
            builder.HasIndex(oi => oi.MenuItemId);

            builder.HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            builder.HasOne(oi => oi.MenuItem)
                .WithMany()
                .HasForeignKey(oi => oi.MenuItemId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
