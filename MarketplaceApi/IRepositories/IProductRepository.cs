using System;
using System.Collections.Generic;
using System.Linq;
using MarketplaceApi.Models;
using MarketplaceApi.ViewModels;

namespace MarketplaceApi.IRepositories
{
    public interface IProductRepository
    {
        public Product ExistingProduct(int productId);

        public IQueryable<Product> ExistingProducts(int productId);
        
        public IQueryable<Product> ProductsByOrder(int orderId);

        public IQueryable ProductsInShop(int shopId);

        public IQueryable<Product> SimilarProducts(ProductView product, int limit,
            decimal priceFluctuation, decimal sizeFluctuation, decimal volumeFluctuation);

        public IQueryable<Product> NewInTimeInterval(int interval);
        
        public List<int> GetAllTypes();
        
        public List<int> GetAllUseCases();
        
        public List<int> GetAllWhereUsed();
        
        public List<int> GetAllMaterials();

        public IQueryable<Product> SearchByAttributes(string name, int? type, int? useCase, int? whereUsed,
            int? material, int? minLength, int? maxLength, int? minWidth, int? maxWidth,
            int? minHeight, int? maxHeight, int? minPrice, int? maxPrice);

        //public IEnumerable<Product> GetProductsInOrder(int orderId);

        public (IQueryable<Product>, IQueryable<OrderedProduct>) GetProductsInOrderWithQuantity(int orderId);

        public void Update(Product product);

        public void Add(Product product);
        
        public void Delete(Product product);
        
        public void Attach(Product product);

        public void Save();
    }
}