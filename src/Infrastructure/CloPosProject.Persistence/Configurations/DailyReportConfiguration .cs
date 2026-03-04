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
    public class DailyReportConfiguration : IEntityTypeConfiguration<DailyReport>
    {
        public void Configure(EntityTypeBuilder<DailyReport> builder)
        {
            builder.ToTable("DailyReports");
            builder.HasKey(r => r.Id);

            builder.Property(r => r.ReportDate).IsRequired();
            builder.Property(r => r.GeneratedAt).IsRequired();

            builder.Property(r => r.TotalRevenue).HasPrecision(18, 2);
            builder.Property(r => r.TotalTax).HasPrecision(18, 2);
            builder.Property(r => r.TotalDiscount).HasPrecision(18, 2);
            builder.Property(r => r.AverageOrderValue).HasPrecision(18, 2);
            builder.Property(r => r.CashPayments).HasPrecision(18, 2);
            builder.Property(r => r.CardPayments).HasPrecision(18, 2);
            builder.Property(r => r.InventoryValue).HasPrecision(18, 2);

            builder.Property(r => r.TopSellingItems).HasColumnType("nvarchar(max)");
            builder.Property(r => r.CategorySales).HasColumnType("nvarchar(max)");
            builder.Property(r => r.WaiterPerformance).HasColumnType("nvarchar(max)");
            builder.Property(r => r.LowStockItems).HasColumnType("nvarchar(max)");

            builder.HasIndex(r => r.ReportDate).IsUnique();
        }
    }
}
