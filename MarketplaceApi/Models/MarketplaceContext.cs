using System;
using Microsoft.EntityFrameworkCore;
using MarketplaceApi.Enums;


namespace MarketplaceApi.Models
{
    public class MarketplaceContext : DbContext
    {
        public MarketplaceContext(DbContextOptions<MarketplaceContext> dbContextOptions) : base(dbContextOptions) {}

        public DbSet<User> User { get; set; }
        public DbSet<Product> Product { get; set; }

        public DbSet<Order> Order { get; set; }

        //public DbSet<Bill> Bill { get; set; }
        public DbSet<OrderedProduct> OrderedProduct { get; set; }
        public DbSet<Shop> Shop { get; set; }
        public DbSet<ShopModerator> ShopModerator { get; set; }

        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies();
        }
        */
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
                    j => j
                        .HasOne(pt => pt.Order)
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
                .WithMany(p => p.Moderators)
                .UsingEntity<ShopModerator>(
                    j => j
                        .HasOne(pt => pt.Shop)
                        .WithMany(p => p.ShopModerators)
                        .HasForeignKey(pt => pt.ShopId),
                    j => j
                        .HasOne(pt => pt.Moderator)
                        .WithMany(p => p.ShopModerators)
                        .HasForeignKey(pt => pt.ModeratorId)
                );

            mb
                .Entity<User>()
                .HasMany(u => u.ShopsOwned)
                .WithOne(s => s.Owner);

            mb
                .Entity<User>().HasData(
                    new User()
                    {
                        Id = 1,
                        Phone = "+71111111111",
                        Email = "1111mail.com",
                        FirstName = "User1",
                        SecondName = "User1",
                        RegistrationDate = DateTime.Now,
                        DeliveryAddress = "Moscow",
                        Admin = true
                    },
                    new User()
                    {
                        Id = 2,
                        Phone = "+72222222222",
                        Email = "2222mail.com",
                        FirstName = "User2",
                        SecondName = "User2",
                        RegistrationDate = DateTime.Now,
                        DeliveryAddress = "Belgorod",
                        Seller = true,
                    },
                    new User()
                    {
                        Id = 3,
                        Phone = "+73333333333",
                        Email = "3333mail.com",
                        FirstName = "User3",
                        SecondName = "User3",
                        RegistrationDate = DateTime.Now,
                        DeliveryAddress = "Ufa",
                    });

            mb.Entity<Shop>().HasData(
                new Shop()
                {
                    Id = 1,
                    Name = "shop1",
                    CreationDate = DateTime.Now,
                    OwnerId = 2,
                });

            mb.Entity<ShopModerator>().HasData(
                new ShopModerator()
                {
                    ShopId = 1,
                    ModeratorId = 2,
                });

            mb.Entity<Product>().HasData(
                new Product()
                {
                    Id = 1,
                    UserId = 2,
                    Name = "ball",
                    Type = 1,
                    UseCase = 1,
                    WhereUsed = 1,
                    Material = 1,
                    Length = 6,
                    Width = 6,
                    Height = 6,
                    Price = 1500,
                    InStockQuantity = 20,
                    CreationDate = DateTime.Now,
                    IsPublic = true,
                    PublicationDate = DateTime.Now,
                    ShopId = 1,
                },
                new Product()
                {
                    Id = 2,
                    UserId = 2,
                    Name = "tower",
                    Type = 2,
                    UseCase = 2,
                    WhereUsed = 2,
                    Material = 2,
                    Length = 5,
                    Width = 5,
                    Height = 16,
                    Price = 2500,
                    InStockQuantity = 25,
                    CreationDate = DateTime.Now,
                    IsPublic = true,
                    PublicationDate = DateTime.Now,
                    ShopId = 1,
                });

            mb.Entity<Order>().HasData(
                new Order()
                {
                    Id = 1,
                    OrderDate = DateTime.Now,
                    OrderStatusId = (int)OrderStatusEnum.Basket,
                    UserId = 3,
                });
        }
    }
}