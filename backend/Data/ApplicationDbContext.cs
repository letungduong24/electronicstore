using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Models;

namespace UserManagementAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ProductModel> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure ProductModel inheritance
            modelBuilder.Entity<ProductModel>()
                .HasDiscriminator<string>("ProductType")
                .HasValue<Television>("Television")
                .HasValue<AirConditioner>("AirConditioner")
                .HasValue<WashingMachine>("WashingMachine");

            // Configure ProductModel properties
            modelBuilder.Entity<ProductModel>()
                .Property(p => p.Name)
                .IsRequired();

            modelBuilder.Entity<ProductModel>()
                .Property(p => p.Description)
                .IsRequired();

            modelBuilder.Entity<ProductModel>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            // Ignore Properties navigation for all product types
            modelBuilder.Entity<ProductModel>()
                .Ignore(p => p.Properties);

            modelBuilder.Entity<Television>()
                .Ignore(t => t.Properties);

            modelBuilder.Entity<AirConditioner>()
                .Ignore(a => a.Properties);

            modelBuilder.Entity<WashingMachine>()
                .Ignore(w => w.Properties);

            // Configure Television properties
            modelBuilder.Entity<Television>()
                .Property(t => t.ScreenSize)
                .IsRequired();

            // Configure AirConditioner properties
            modelBuilder.Entity<AirConditioner>()
                .Property(a => a.Scope)
                .IsRequired();

            // Configure WashingMachine properties
            modelBuilder.Entity<WashingMachine>()
                .Property(w => w.Capacity)
                .IsRequired();

            // Configure Cart relationships
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithOne(u => u.Cart)
                .HasForeignKey<Cart>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany()
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Order relationships
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure decimal precision for Order
            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.TotalPrice)
                .HasPrecision(18, 2);
        }
    }
} 