using Microsoft.EntityFrameworkCore;
using MyDotNetEfApp.Entities;
using System;

namespace MyDotNetEfApp.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<Shipment> Shipments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Product configuration
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.SKU)
                .IsUnique();

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Supplier)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            // Order configuration
            modelBuilder.Entity<Order>()
                .HasIndex(o => o.OrderNumber)
                .IsUnique();

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Supplier)
                .WithMany(s => s.Orders)
                .HasForeignKey(o => o.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            // OrderItem configuration
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // InventoryItem configuration
            modelBuilder.Entity<InventoryItem>()
                .HasOne(ii => ii.Product)
                .WithMany(p => p.InventoryItems)
                .HasForeignKey(ii => ii.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InventoryItem>()
                .HasOne(ii => ii.Warehouse)
                .WithMany(w => w.InventoryItems)
                .HasForeignKey(ii => ii.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InventoryItem>()
                .HasIndex(ii => new { ii.ProductId, ii.WarehouseId })
                .IsUnique();

            // Shipment configuration
            modelBuilder.Entity<Shipment>()
                .HasIndex(s => s.TrackingNumber)
                .IsUnique();

            modelBuilder.Entity<Shipment>()
                .HasOne(s => s.Order)
                .WithOne(o => o.Shipment)
                .HasForeignKey<Shipment>(s => s.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Warehouse configuration
            modelBuilder.Entity<Warehouse>()
                .HasIndex(w => w.Code)
                .IsUnique();

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            var now = new DateTime(2026, 1, 18, 12, 0, 0, DateTimeKind.Utc);

            // Seed Warehouses
            modelBuilder.Entity<Warehouse>().HasData(
                new Warehouse { Id = 1, Name = "Magazzino Centrale Milano", Code = "WH-MI-01", Address = "Via Industriale 123", City = "Milano", Country = "Italia", Capacity = 10000, CreatedAt = now },
                new Warehouse { Id = 2, Name = "Magazzino Roma", Code = "WH-RM-01", Address = "Via Logistica 45", City = "Roma", Country = "Italia", Capacity = 8000, CreatedAt = now }
            );

            // Seed Suppliers
            modelBuilder.Entity<Supplier>().HasData(
                new Supplier { Id = 1, CompanyName = "Tech Components SRL", ContactName = "Mario Rossi", Email = "mario.rossi@techcomp.it", Phone = "+39 02 1234567", City = "Milano", Country = "Italia", LeadTimeDays = 7, CreatedAt = now },
                new Supplier { Id = 2, CompanyName = "Global Electronics", ContactName = "Laura Bianchi", Email = "laura.bianchi@globalelec.it", Phone = "+39 06 7654321", City = "Roma", Country = "Italia", LeadTimeDays = 10, CreatedAt = now }
            );

            // Seed Products
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Microcontrollore STM32", SKU = "MCU-STM32-001", Description = "Microcontrollore ARM Cortex-M4", UnitPrice = 12.50m, ReorderLevel = 100, SupplierId = 1, CreatedAt = now },
                new Product { Id = 2, Name = "Sensore Temperatura DHT22", SKU = "SNS-DHT22-001", Description = "Sensore temperatura e umidità", UnitPrice = 8.75m, ReorderLevel = 50, SupplierId = 1, CreatedAt = now },
                new Product { Id = 3, Name = "Display LCD 16x2", SKU = "DSP-LCD-001", Description = "Display LCD retroilluminato", UnitPrice = 15.00m, ReorderLevel = 30, SupplierId = 2, CreatedAt = now }
            );
        }
    }
}
