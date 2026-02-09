using Microsoft.EntityFrameworkCore;
using CloPosProject.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection;

namespace CloPosProject.Persistence.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Order>? Orders { get; set; }
        public DbSet<OrderItem>? OrderItems { get; set; }
        public DbSet<MenuItem>? MenuItems { get; set; }
        public DbSet<Category>? Categories { get; set; }
        public DbSet<Table>? Tables { get; set; }
        public DbSet<Reservation>? Reservations { get; set; }
        public DbSet<Inventory>? Inventories { get; set; }
        public DbSet<Ingredient>? Ingredients { get; set; }
        public DbSet<Payment>? Payments { get; set; }
        public DbSet<InventoryTransaction>? InventoryTransactions { get; set; }
        public DbSet<Settings>? Settings { get; set; }
        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }


    }
}