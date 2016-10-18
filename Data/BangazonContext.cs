using Microsoft.EntityFrameworkCore;
using Bangazon.Models;

namespace Bangazon.Data
{
//this is the interface between what we write and the database
    public class BangazonContext : DbContext
    {
        public BangazonContext(DbContextOptions<BangazonContext> options)
            : base(options)
        { }
//what tables do we want to interact wtih? listed below. 
        public DbSet<Customer> Customer { get; set; }
        //DbSet - the database set
        public DbSet<Product> Product { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<PaymentType> PaymentType { get; set; }
        public DbSet<LineItem> LineItem { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //this is exactly what we want the database to do when something is generated - if we create a customer, set the value to this function. Specify specifically what to happen. 
            modelBuilder.Entity<Customer>()
                .Property(b => b.DateCreated)
                .HasDefaultValueSql("strftime('%Y-%m-%d %H:%M:%S')");

            modelBuilder.Entity<Order>()
                .Property(b => b.DateCreated)
                .HasDefaultValueSql("strftime('%Y-%m-%d %H:%M:%S')");

            modelBuilder.Entity<PaymentType>()
                .Property(b => b.DateCreated)
                .HasDefaultValueSql("strftime('%Y-%m-%d %H:%M:%S')");

            modelBuilder.Entity<Product>()
                .Property(b => b.DateCreated)
                .HasDefaultValueSql("strftime('%Y-%m-%d %H:%M:%S')");
        }
    }
}