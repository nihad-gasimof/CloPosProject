using CloPosProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloPosProject.Persistence.Configurations
{
    public class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
    {
        public void Configure(EntityTypeBuilder<Ingredient> builder)
        {
            builder.ToTable("Ingredients");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(i => i.Unit)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(i => i.Category)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(i => i.MinimumStock)
                .IsRequired()
                .HasPrecision(18, 3);

            builder.Property(i => i.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(i => i.CreatedAt)
                .IsRequired();

            builder.HasIndex(i => i.Name).IsUnique();
            builder.HasIndex(i => i.Category);

            builder.HasOne(i => i.Inventory)
                .WithOne(inv => inv.Ingredient)
                .HasForeignKey<Inventory>(inv => inv.IngredientId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
