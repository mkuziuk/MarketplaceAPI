using Microsoft.EntityFrameworkCore;


namespace MarketplaceApi.Models
{
    public class MarketplaceContext : DbContext
    {

        public MarketplaceContext(DbContextOptions<MarketplaceContext> dbContextOptions) : base(dbContextOptions)
        {
                
        }
        
        public DbSet<User> User { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Bill> Bill { get; set; }
        public DbSet<OrderedProduct> OrderedProduct { get; set; }
        public DbSet<Shop> Shop { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb
                .Entity<Order>()
                .HasMany(o => o.Products)
                .WithMany(p => p.Orders)
                .UsingEntity<OrderedProduct>(
                    j => j
                        .HasOne(pt => pt.Product)
                        .WithMany(p => p.OrderedProducts)
                        .HasForeignKey(pt => pt.ProductId),
                    j => j.HasOne(pt => pt.Order)
                        .WithMany(p => p.OrderedProducts)
                        .HasForeignKey(pt => pt.OrderId),
                    j =>
                    {
                        j.Property(pt => pt.Quantity);
                        j.HasKey(t => new { t.OrderId, t.ProductId });
                        j.ToTable("OrderedProduct");
                    });

            mb
                .Entity<User>()
                .HasMany(u => u.ShopsWhereModerator)
                .WithMany(p => p.ModeratorUsers);

            mb
                .Entity<User>()
                .HasMany(u => u.ShopsOwned)
                .WithOne(s => s.Owner);
        }
    }
}