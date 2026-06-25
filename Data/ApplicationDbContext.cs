using LexiElectronics.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace LexiElectronics.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {        
        public DbSet<Models.ProductCategory> ProductCategories { get; set; }
        public DbSet<Models.Manufacturer> Manufacturers { get; set; }
        public DbSet<Models.Product> Products { get; set; }
        public DbSet<Models.Order> Orders { get; set; }
        public DbSet<Models.OrderItem> OrderItems { get; set; }
        public DbSet<Models.ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>(b =>
            {
                b.Property(u => u.Firstname).HasMaxLength(100);
                b.Property(u => u.Lastname).HasMaxLength(100);

                b.Property(u => u.InvoiceName).HasMaxLength(100);
                b.Property(u => u.InvoiceStreetAddress).HasMaxLength(100);                
                b.Property(u => u.InvoiceZipcode).HasMaxLength(100);
                b.Property(u => u.InvoiceCity).HasMaxLength(100);

                b.Property(u => u.DeliveryName).HasMaxLength(100);
                b.Property(u => u.DeliveryStreetAddress).HasMaxLength(100);
                b.Property(u => u.DeliveryZipcode).HasMaxLength(100);
                b.Property(u => u.DeliveryCity).HasMaxLength(100);
            });

            builder.Entity<Product>().Property(p => p.ShortDescription).HasMaxLength(125);

            builder.Entity<Product>().Property(p => p.VisibleInShop).HasDefaultValue(true);

            builder.Entity<ProductCategory>().Property(pc => pc.Name).HasMaxLength(100);
        }
    }
}
