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

            builder.Property(i => i.NameAz)
                .HasMaxLength(200);

            builder.Property(i => i.Unit)
                .IsRequired();

            builder.Property(i => i.CurrentStock)
                .HasColumnType("decimal(18,2)");

            builder.Property(i => i.MinimumStock)
                .HasColumnType("decimal(18,2)");

            builder.Property(i => i.UnitPrice)
                .HasColumnType("decimal(18,2)");

            builder.Property(i => i.IsActive)
                .HasDefaultValue(true);

            builder.HasMany(i => i.MenuItems)
                .WithMany(m => m.Ingredients);
               
        }
    }
}
