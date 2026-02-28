using CloPosProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace CloPosProject.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(o => o.Id);

            builder.Property(o => o.OrderNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(o => o.OrderDate)
                .IsRequired();

            builder.Property(o => o.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(o => o.OrderType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(o => o.TotalAmount)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(o => o.Tax)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(o => o.Discount)
                .IsRequired()
                .HasPrecision(18, 2)
                .HasDefaultValue(0);

            builder.Property(o => o.DeliveryFee)
                .IsRequired()
                .HasPrecision(18, 2)
                .HasDefaultValue(0);

            builder.Property(o => o.FinalAmount)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(o => o.CustomerName)
                .HasMaxLength(200);

            builder.Property(o => o.CustomerPhone)
                .HasMaxLength(20);

            builder.Property(o => o.Notes)
                .HasMaxLength(1000);

            builder.Property(o => o.TableNumber)
                .HasMaxLength(20);

            builder.Property(o => o.DeliveryProvider)
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(o => o.DeliveryAddress)
                .HasMaxLength(500);

            builder.Property(o => o.DeliveryInstructions)
                .HasMaxLength(500);

            builder.Property(o => o.IsPickedUp)
                .HasDefaultValue(false);

            builder.HasIndex(o => o.OrderNumber).IsUnique();
            builder.HasIndex(o => o.OrderDate);
            builder.HasIndex(o => o.Status);
            builder.HasIndex(o => o.OrderType);
            builder.HasIndex(o => o.TableId);
            builder.HasIndex(o => o.CustomerPhone);

            builder.HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
            builder
    .HasOne(o => o.Table)
    .WithMany(t => t.Orders)
    .HasForeignKey(o => o.TableId);
        }
    }
}
