using CloPosProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Persistence.Configurations
{
    public class InventoryLogConfiguration : IEntityTypeConfiguration<InventoryLog>
    {
        public void Configure(EntityTypeBuilder<InventoryLog> builder)
        {
            builder.ToTable("InventoryLogs");
            builder.HasKey(l => l.Id);

            builder.Property(l => l.LogType)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(l => l.QuantityBefore).HasPrecision(18, 3);
            builder.Property(l => l.QuantityChange).HasPrecision(18, 3);
            builder.Property(l => l.QuantityAfter).HasPrecision(18, 3);
            builder.Property(l => l.UnitPrice).HasPrecision(18, 2);

            builder.Property(l => l.Reason).HasMaxLength(500);
            builder.Property(l => l.CreatedAt).IsRequired();

            builder.HasIndex(l => l.IngredientId);
            builder.HasIndex(l => l.CreatedAt);
            builder.HasIndex(l => l.LogType);

            builder.HasOne(l => l.Ingredient)
                .WithMany()
                .HasForeignKey(l => l.IngredientId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
