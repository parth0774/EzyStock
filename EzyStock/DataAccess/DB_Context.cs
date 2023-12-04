using EzyStock.Models;
using Microsoft.EntityFrameworkCore;

namespace EzyStock.DataAccess
{
    public class DB_Context:DbContext
    {
        public DB_Context(DbContextOptions<DB_Context> options) : base(options) { }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<LoginInfo> Logins { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts {  get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Supplier)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.SupplierId);

            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Order)
                .WithMany(o => o.Products)
                .HasForeignKey(o => o.OrderId);
        }


    }
}
