using CodeTech_Task_1.Migrations;
using CodeTech_Task_1.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeTech_Task_1.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Order> OrderItem { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItem { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2); // Or HasColumnType("decimal(18,2)")

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
                .HasPrecision(18, 2); // Or HasColumnType("decimal(18,2)")

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2); // Or HasColumnType("decimal(18,2)")

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2); // or .HasColumnType("decimal(18,2)")

            //    // Customer
            //    modelBuilder.Entity<Customer>(entity =>
            //    {
            //        entity.HasKey(c => c.ID);
            //        entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
            //        entity.Property(c => c.Password).IsRequired().HasMaxLength(100);
            //        entity.Property(c => c.Email).IsRequired().HasMaxLength(100);
            //    });

            //    // Order
            //    modelBuilder.Entity<Order>(entity =>
            //    {
            //        entity.HasKey(o => o.OID);
            //        entity.Property(o => o.Amount).IsRequired();
            //        entity.Property(o => o.Date).IsRequired();

            //        entity.HasOne(o => o.Customer)
            //              .WithMany(c => c.Orders)
            //              .HasForeignKey(o => o.CID)
            //              .OnDelete(DeleteBehavior.Cascade);
            //    });

            //    // Product
            //    modelBuilder.Entity<Product>(entity =>
            //    {
            //        entity.HasKey(p => p.OID);
            //        entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
            //        entity.Property(p => p.Description).HasMaxLength(500);
            //        entity.Property(p => p.Price).IsRequired();

            //        entity.HasOne(p => p.Category)
            //              .WithMany(c => c.Products)
            //              .HasForeignKey(p => p.CID)
            //              .OnDelete(DeleteBehavior.Cascade);
            //    });

            //    // Payment
            //    modelBuilder.Entity<Payment>(entity =>
            //    {
            //        entity.HasKey(p => p.PID);
            //        entity.Property(p => p.Type).IsRequired().HasMaxLength(50);
            //        entity.Property(p => p.Amount).IsRequired();

            //        entity.HasOne(p => p.Order)
            //              .WithMany(o => o.Payments)
            //              .HasForeignKey(p => p.OID)
            //              .OnDelete(DeleteBehavior.Cascade);
            //    });

            //    // Category
            //    modelBuilder.Entity<Category>(entity =>
            //    {
            //        entity.HasKey(c => c.CID);
            //        entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
            //        entity.Property(c => c.Description).HasMaxLength(300);
            //        entity.Property(c => c.Picture).HasMaxLength(200);
            //    });

            //    // Cart
            //    modelBuilder.Entity<Cart>(entity =>
            //    {
            //        entity.HasKey(c => c.ID);

            //        entity.HasOne(c => c.Customer)
            //              .WithMany(cu => cu.Carts)
            //              .HasForeignKey(c => c.CID)
            //              .OnDelete(DeleteBehavior.Cascade);
            //    });

            //    // OrderProduct (Join Table)
            //    modelBuilder.Entity<OrderProduct>()
            //        .HasKey(op => new { op.OID, op.ProductID });

            //    modelBuilder.Entity<OrderProduct>()
            //        .HasOne(op => op.Order)
            //        .WithMany(o => o.OrderProducts)
            //        .HasForeignKey(op => op.OID);

            //    modelBuilder.Entity<OrderProduct>()
            //        .HasOne(op => op.Product)
            //        .WithMany(p => p.OrderProducts)
            //        .HasForeignKey(op => op.ProductID);

            //    // CartProduct (Join Table)
            //    modelBuilder.Entity<CartProduct>()
            //        .HasKey(cp => new { cp.CartID, cp.ProductID });

            //    modelBuilder.Entity<CartProduct>()
            //        .HasOne(cp => cp.Cart)
            //        .WithMany(c => c.CartProducts)
            //        .HasForeignKey(cp => cp.CartID);

            //    modelBuilder.Entity<CartProduct>()
            //        .HasOne(cp => cp.Product)
            //        .WithMany(p => p.CartProducts)
            //        .HasForeignKey(cp => cp.ProductID);
        }
    }
}
