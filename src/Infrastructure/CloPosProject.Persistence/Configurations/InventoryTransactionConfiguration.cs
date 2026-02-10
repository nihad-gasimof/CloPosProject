using CloPosProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloPosProject.Persistence.Configurations
{
    public class InventoryTransactionConfiguration : IEntityTypeConfiguration<InventoryTransaction>
    {
        public void Configure(EntityTypeBuilder<InventoryTransaction> builder)
        {
            builder.ToTable("InventoryTransactions");
            builder.HasKey(it => it.Id);

            builder.Property(it => it.Type)
                .IsRequired();

            builder.Property(it => it.Quantity)
                .HasColumnType("decimal(18,2)");

            builder.Property(it => it.UnitPrice)
                .HasColumnType("decimal(18,2)");

            builder.Property(it => it.SupplierName)
                .HasMaxLength(500);

            builder.Property(it => it.InvoiceNumber)
                .HasMaxLength(200);

            builder.Property(it => it.CreatedAt)
                .IsRequired();

            builder.HasOne(it => it.Ingredient)
                .WithMany()
                .HasForeignKey(it => it.IngredientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(it => it.User)
                .WithMany()
                .HasForeignKey(it => it.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
