using Microsoft.EntityFrameworkCore;


namespace MarketplaceApi.Models
{
    public class MarketplaceContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Bill> Bill { get; set; }
        public DbSet<OrderedProduct> OrderedProduct { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Marketplace;Username=postgres;" +
                                     "Password=mypassword");
        }
    }
}