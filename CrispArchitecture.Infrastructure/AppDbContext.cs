using CrispArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrispArchitecture.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<LineItem> LineItems { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Implementing cascading delete so LineItems will be deleted with the order.
            modelBuilder
                .Entity<Order>()
                .HasMany(o => o.LineItems)
                .WithOne(li => li.Order)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
