using System;
using System.Collections.Generic;
using System.Linq;
using MarketplaceApi.Models;
using MarketplaceApi.Views;

namespace MarketplaceApi.Repositories
{
    public class ProductRepository : Repository
    {
        public ProductRepository(MarketplaceContext context) : base(context) {}

        private IQueryable<ProductView> SelectProductView() => Context.Product
            .Select(p => new ProductView()
            {
                Id = p.Id,
                UserId = p.UserId,
                Name = p.Name,
                ShopId = p.ShopId,
                Price = p.Price,
                Material = p.Material,
                Type = p.Type,
                UseCase = p.UseCase,
                WhereUsed = p.WhereUsed,
                Length = p.Length,
                Width = p.Width,
                Height = p.Height,
                InStockQuantity = p.InStockQuantity,
                IsPublic = p.IsPublic,
                PublicationDate = p.PublicationDate
            });

        public Product ExistingProduct(int productId) => Context.Product
            .FirstOrDefault(p => p.Id == productId);
        
        public dynamic ExistingProductView(int productId) => SelectProductView()
            .FirstOrDefault(p => p.Id == productId);
        
        public IQueryable<ProductView> ExistingProductsObj(int productId) => SelectProductView()
            .Where(p => p.Id == productId);
        
        public IQueryable<Product> ExistingProducts(int productId) => Context.Product.Where(p => p.Id == productId);
        
        public IQueryable<Product> ProductsByOrder(int orderId) => Context.Product
            .Where(p => p.Orders.Any(o => o.Id == orderId));

        public IEnumerable<ProductView> ProductsInShop(int shopId) => SelectProductView()
            .Where(p => p.ShopId == shopId);

        public IEnumerable<ProductView> SimilarProducts(ProductView product, int limit, 
            decimal priceFluctuation, decimal sizeFluctuation, decimal volumeFluctuation)
        {
            var similarProducts = SelectProductView()
                .Where(p => 
                            p.Id != product.Id &&
                            (
                                p.IsPublic && 
                                (
                                    p.Name == product.Name ||
                                    p.Material == product.Material ||
                                    p.Type == product.Type ||
                                    p.UseCase == product.UseCase ||
                                    p.WhereUsed == product.WhereUsed ||
                                    
                                    Convert.ToDecimal(p.Price) >= Convert.ToDecimal(product.Price) * (1 - priceFluctuation) ||
                                    Convert.ToDecimal(p.Price) <= Convert.ToDecimal(product.Price) * (1 + priceFluctuation) && 
                                    (
                                        Convert.ToDecimal(p.Length) >= Convert.ToDecimal(product.Length) * (1 - sizeFluctuation) ||
                                        Convert.ToDecimal(p.Length) <= Convert.ToDecimal(product.Length) * (1 + sizeFluctuation) ||
                                        Convert.ToDecimal(p.Width) >= Convert.ToDecimal(product.Width) * (1 - sizeFluctuation) ||
                                        Convert.ToDecimal(p.Width) <= Convert.ToDecimal(product.Width) * (1 + sizeFluctuation) ||
                                        Convert.ToDecimal(p.Height) >= Convert.ToDecimal(product.Height) * (1 - sizeFluctuation) ||
                                        Convert.ToDecimal(p.Height) <= Convert.ToDecimal(product.Height) * (1 + sizeFluctuation)
                                    ) && 
                                    (
                                        Convert.ToDecimal(p.Length * p.Width * p.Height) >= 
                                        Convert.ToDecimal(product.Length * product.Width * product.Height) * (1 - volumeFluctuation) ||
                                        Convert.ToDecimal(p.Length * p.Width * p.Height) >= 
                                        Convert.ToDecimal(product.Length * product.Width * product.Height) * (1 + volumeFluctuation)
                                    )
                                )    
                            )    
                )
                .Take(limit);

            return similarProducts;
        }

        public IQueryable<ProductView> NewInTimeInterval(int interval) => SelectProductView()
            .Where(p => p.IsPublic && p.PublicationDate >= DateTime.Now.AddDays(-interval))
            .OrderByDescending(p => p.PublicationDate);
        
        public List<int> GetAllTypes() => Context.Product
            .Select(p => p.Type).Distinct().ToList();
        
        public List<int> GetAllUseCases() => Context.Product
            .Select(p => p.UseCase).Distinct().ToList();
        
        public List<int> GetAllWhereUsed() => Context.Product
            .Select(p => p.WhereUsed).Distinct().ToList();
        
        public List<int> GetAllMaterials() => Context.Product
            .Select(p => p.Material).Distinct().ToList();

        public IEnumerable<ProductView> SearchByAttributes(string name, int? type, int? useCase, int? whereUsed,
            int? material, int? minLength, int? maxLength, int? minWidth, int? maxWidth,
            int? minHeight, int? maxHeight, int? minPrice, int? maxPrice)
        {
            
            var resultingProducts = SelectProductView()
                .AsEnumerable()
                .Where(p => 
                (name == null || p.Name.ToLower().StartsWith(name.ToLower()))
                && (type == null || p.Type == type)
                && (useCase == null || p.UseCase == useCase)
                && (whereUsed == null || p.WhereUsed == whereUsed)
                && (material == null || p.Material == material)
                
                && (minLength == null || p.Length >= minLength)
                && (maxLength == null || p.Length <= maxLength)
                && (minWidth == null || p.Width >= minWidth)
                && (maxWidth == null || p.Width <= maxWidth)
                && (minHeight == null || p.Height >= minHeight)
                && (maxHeight == null || p.Height <= maxHeight)
                && (minPrice == null || p.Price >= minPrice)
                && (maxPrice == null || p.Price <= maxPrice)
            );
            
            return resultingProducts;
        }

        public IEnumerable<Product> GetProductsInOrder(int orderId) => Context.Product
            .Where(p => p.Orders.Any(o => o.Id == orderId));
        
        public IEnumerable<T> GetProductsInOrderWithQuantity<T>(int orderId)
        {
            var products = Context.Product
                .Where(p => p.Orders.Any(o => o.Id == orderId))
                .Select(p => new ProductView()
                {
                    Id = p.Id, 
                    UserId = p.UserId,
                    Name = p.Name, 
                    ShopId = p.ShopId, 
                    Price = p.Price, 
                    Material = p.Material, 
                    Type = p.Type, 
                    UseCase = p.UseCase,
                    WhereUsed = p.WhereUsed,
                    Length = p.Length,
                    Width = p.Width,
                    Height = p.Height,
                    InStockQuantity = p.InStockQuantity,
                    IsPublic = p.IsPublic,
                    PublicationDate = p.PublicationDate
                });

            var orderedProducts = Context.OrderedProduct
                .Where(op => op.OrderId == orderId);
            
            return (IEnumerable<T>)products
                .Join(orderedProducts,
                    p => p.Id, 
                    op => op.ProductId,
                    (p, op) => new {p, op.Quantity});
        }

        public void Update(Product product) => Context.Product.Update(product);

        public void Add(Product product) => Context.Product.Add(product);
        
        public void Delete(Product product) => Context.Product.Remove(product);
        
        public void Attach(Product product) => Context.Product.Attach(product);

    }
}