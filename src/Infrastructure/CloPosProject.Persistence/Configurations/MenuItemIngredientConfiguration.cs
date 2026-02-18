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
    public class MenuItemIngredientConfiguration : IEntityTypeConfiguration<MenuItemIngredient>
    {
        public void Configure(EntityTypeBuilder<MenuItemIngredient> builder)
        {
            builder.HasKey(mi => new { mi.MenuItemId, mi.IngredientId });

        
            builder
                .HasOne(mi => mi.MenuItem)
                .WithMany(m => m.MenuItemIngredients)
                .HasForeignKey(mi => mi.MenuItemId)
                .OnDelete(DeleteBehavior.Cascade); 

            builder
                .HasOne(mi => mi.Ingredient)
                .WithMany(i =>i.MenuItemIngredients)
                .HasForeignKey(mi => mi.IngredientId)
                .OnDelete(DeleteBehavior.Restrict); 

            
            builder.Property(mi => mi.Quantity)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)"); 

            builder.ToTable("MenuItemIngredients");
        }
    }
}
