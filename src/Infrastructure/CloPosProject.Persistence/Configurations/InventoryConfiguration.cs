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

            builder.Property(i => i.IngredientId)
                .IsRequired();

            builder.Property(i => i.Quantity)
                .IsRequired()
                .HasPrecision(18, 3);

            builder.Property(i => i.AverageUnitPrice)
                .IsRequired()
                .HasPrecision(18, 2);

           

            builder.HasIndex(i => i.IngredientId).IsUnique();
        }
    }
}
