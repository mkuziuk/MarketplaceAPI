using System;
using System.Linq;
using AutoMapper;
using MarketplaceApi.Controllers;
using MarketplaceApi.Models;
using MarketplaceApi.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;
using MarketplaceApi.Mapping;

namespace MarketplaceApiUnitTests
{
    public class MarketplaceApiUnitTests
    {
        private readonly MarketplaceContext _context;
        private readonly IMapper _mapper;

        private readonly OrderedProductController _orderedProductController;

        public MarketplaceApiUnitTests()
        {
            const string connectionString = "Host=localhost;Port=5432;Database=Marketplace;Username=postgres;Password=mypassword";

            var dbOption = new DbContextOptionsBuilder<MarketplaceContext>();
            dbOption.UseNpgsql(connectionString);

            _context = new MarketplaceContext(dbOption.Options);
            
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(typeof(AppMappingProfile)));

            _mapper = new Mapper(mapperConfig);

            _orderedProductController = new OrderedProductController(_context, _mapper);
        }
        
        [Fact]
        public void AddProductToOrderTest()
        {
            const int userId = 3;
            const int orderId = 2;
            const int productId = 1;
            const int quantity = 2;
            
            _orderedProductController.Post(userId, orderId, productId, quantity);
            
            var addedOrderedProduct =
                _context.OrderedProduct.FirstOrDefault(op => op.OrderId == orderId && op.ProductId == productId);
            
            Assert.NotNull(addedOrderedProduct);
            Assert.Equal(productId, addedOrderedProduct.ProductId);
            Assert.Equal(orderId, addedOrderedProduct.OrderId);
            Assert.Equal(quantity, addedOrderedProduct.Quantity);
        }
        
        [Fact]
        public void ChangeProductQuantityTest()
        {
            const int userId = 3;
            const int orderId = 2;
            const int productId = 1;
            const int newQuantity = 5;
            
            _orderedProductController.PatchQuantity(userId, orderId, productId, newQuantity);
            
            var addedOrderedProduct =
                _context.OrderedProduct.FirstOrDefault(op => op.OrderId == orderId && op.ProductId == productId);
            
            Assert.NotNull(addedOrderedProduct);
            Assert.Equal(productId, addedOrderedProduct.ProductId);
            Assert.Equal(orderId, addedOrderedProduct.OrderId);
            Assert.Equal(newQuantity, addedOrderedProduct.Quantity);
        }
        
        [Fact]
        public void DeleteProductFromOrderTest()
        {
            const int userId = 3;
            const int orderId = 2;
            const int productId = 1;
            
            _orderedProductController.Delete(userId, orderId, productId);
            
            var deletedOrderedProduct =
                _context.OrderedProduct.FirstOrDefault(op => op.OrderId == orderId && op.ProductId == productId);
            
            Assert.Null(deletedOrderedProduct);
        }
    }
}