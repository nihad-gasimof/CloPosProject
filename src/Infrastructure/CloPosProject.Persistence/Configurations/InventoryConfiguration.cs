using CloPosProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloPosProject.Persistence.Configurations
{
    public class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
    {
        public void Configure(EntityTypeBuilder<Inventory> builder)
        {
            builder.ToTable("Inventories");
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Quantity)
                .HasColumnType("decimal(18,2)");

            builder.Property(i => i.AverageUnitPrice)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(i => i.Ingredient)
                .WithMany()
                .HasForeignKey(i => i.IngredientId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
