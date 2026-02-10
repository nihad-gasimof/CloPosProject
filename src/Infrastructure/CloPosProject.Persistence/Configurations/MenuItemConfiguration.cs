using CloPosProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloPosProject.Persistence.Configurations
{
    public class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
    {
        public void Configure(EntityTypeBuilder<MenuItem> builder)
        {
            builder.ToTable("MenuItems");
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(m => m.Description)
                .HasMaxLength(1000);

            builder.Property(m => m.Price)
                .HasColumnType("decimal(18,2)");

            builder.Property(m => m.ImageUrl)
                .HasMaxLength(1000);

            builder.Property(m => m.IsAvailable)
                .HasDefaultValue(true);

            builder.HasOne(m => m.Category)
                .WithMany(c => c.MenuItems)
                .HasForeignKey(m => m.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Many-to-many between MenuItem and Ingredient (if intended as List<Ingredient>)
            builder.HasMany(m => m.Ingredients)
                .WithMany(i => i.MenuItems);
               
        }
    }
}
