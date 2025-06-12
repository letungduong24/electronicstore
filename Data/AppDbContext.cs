using ElectronicStore.Data;
using ElectronicStore.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectronicStore.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<ProductModel> Products { get; set; }
    public DbSet<AirConditioner> AirConditioners { get; set; }
    public DbSet<Television> Televisions { get; set; }
    public DbSet<WashingMachine> WashingMachines { get; set; }
    public DbSet<UserModel> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Table Per Type (TPT)
        modelBuilder.Entity<AirConditioner>().ToTable("AirConditioners");
        modelBuilder.Entity<Television>().ToTable("Televisions");
        modelBuilder.Entity<WashingMachine>().ToTable("WashingMachines");

        // Configure required fields for base class
        modelBuilder.Entity<ProductModel>()
            .Property(p => p.Name)
            .IsRequired();

        modelBuilder.Entity<ProductModel>()
            .Property(p => p.Brand)
            .IsRequired();

        // Configure decimal precision for Price
        modelBuilder.Entity<ProductModel>()
            .Property(p => p.Price)
            .HasPrecision(18, 2);

        // Configure specific constraints for each product type
        modelBuilder.Entity<AirConditioner>()
            .Property(p => p.Scope)
            .IsRequired();

        modelBuilder.Entity<Television>()
            .Property(p => p.ScreenSize)
            .IsRequired();

        modelBuilder.Entity<WashingMachine>()
            .Property(p => p.Capacity)
            .IsRequired();

        // Configure UserModel
        modelBuilder.Entity<UserModel>()
            .Property(u => u.Username)
            .IsRequired();

        modelBuilder.Entity<UserModel>()
            .Property(u => u.Password)
            .IsRequired();

        modelBuilder.Entity<UserModel>()
            .Property(u => u.Email)
            .IsRequired();

        modelBuilder.Entity<UserModel>()
            .Property(u => u.Role)
            .IsRequired();

        // Add unique constraints
        modelBuilder.Entity<UserModel>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<UserModel>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
} 