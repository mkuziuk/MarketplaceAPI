using System;
using System.Collections.Generic;
using System.Linq;
using MarketplaceApi.Enums;
using MarketplaceApi.Models;
using MarketplaceApi.Repositories;
using MarketplaceApi.Views;

namespace MarketplaceApi.Services
{
    public class ProductService
    {
        private const decimal PriceFluctuation = 0.1M;
        private const decimal SizeFluctuation = 0.1M;
        private const decimal VolumeFluctuation = 0.1M;

        private readonly UserRepositoryBase _userRepositoryBase;
        private readonly ProductRepositoryBase _productRepositoryBase;
        private readonly ShopRepositoryBase _shopRepositoryBase;

        public ProductService(MarketplaceContext context)
        {
            _userRepositoryBase = new UserRepositoryBase(context);
            _productRepositoryBase = new ProductRepositoryBase(context);
            _shopRepositoryBase = new ShopRepositoryBase(context);
        }

        public KeyValuePair<StatusCodeEnum, QueryableAndString<object>> GetProduct(int productId)
        {
            var product = _productRepositoryBase.ExistingProductsObj(productId);
            if (product == null)
                return new KeyValuePair<StatusCodeEnum, QueryableAndString<object>>
                    (StatusCodeEnum.NotFound, new QueryableAndString<object>
                        (null, $"Товар {productId} не существует"));
            
            return new KeyValuePair<StatusCodeEnum, QueryableAndString<object>>
                (StatusCodeEnum.Ok, new QueryableAndString<object>(product, "Получилось"));
        }

        public KeyValuePair<StatusCodeEnum, QueryableAndString<object>> GetSimilar(int productId, int limit)
        {
            var product = _productRepositoryBase.ExistingProductsObj(productId).FirstOrDefault();
            if (product == null)
                return new KeyValuePair<StatusCodeEnum, QueryableAndString<object>>
                    (StatusCodeEnum.NotFound, new QueryableAndString<object>
                        (null, $"Товар {productId} не существует"));

            var similarProducts = _productRepositoryBase
                .SimilarProducts(product, limit, PriceFluctuation, SizeFluctuation, VolumeFluctuation);
            if (similarProducts == null)
                return new KeyValuePair<StatusCodeEnum, QueryableAndString<object>>
                (StatusCodeEnum.NotFound, new QueryableAndString<object>
                    (null, $"Товаров похожих на {productId} нет"));

            return new KeyValuePair<StatusCodeEnum, QueryableAndString<object>>(StatusCodeEnum.Ok,
                new QueryableAndString<object>(similarProducts, "Получилось"));
        }

        public KeyValuePair<StatusCodeEnum, QueryableAndString<ProductView>> GetNewOfTheWeek(int limit)
        {
            var newProducts = _productRepositoryBase.NewInTimeInterval(7).Take(limit);
            
            return new KeyValuePair<StatusCodeEnum, QueryableAndString<ProductView>>(StatusCodeEnum.Ok,
                new QueryableAndString<ProductView>(newProducts, null));
        }

        public KeyValuePair<StatusCodeEnum, QueryableAndString<List<int>>> GetAttributes()
        {
            IEnumerable<List<int>> attributes = new[]
            {
                _productRepositoryBase.GetAllTypes(),
                _productRepositoryBase.GetAllUseCases(),
                _productRepositoryBase.GetAllWhereUsed(),
                _productRepositoryBase.GetAllMaterials()
            };
            
            return new KeyValuePair<StatusCodeEnum, QueryableAndString<List<int>>>(StatusCodeEnum.Ok,
                new QueryableAndString<List<int>>(attributes, "Получилось")); 
        }

        public KeyValuePair<StatusCodeEnum, QueryableAndString<ProductView>> Search(string name, int? type, int? useCase,
            int? whereUsed, int? material, int? minLength, int? maxLength, int? minWidth, int? maxWidth,
            int? minHeight, int? maxHeight, int? minPrice, int? maxPrice)
        {
            var products = _productRepositoryBase.SearchByAttributes(name, type, useCase, whereUsed, 
                material, minLength, maxLength, minWidth, maxWidth, minHeight, maxHeight, minPrice, maxPrice);
            
            return new KeyValuePair<StatusCodeEnum, QueryableAndString<ProductView>>(StatusCodeEnum.Ok,
                new QueryableAndString<ProductView>(products, "Получилось"));
        }

        public KeyValuePair<StatusCodeEnum, string> AddProduct(ProductEntity productEntity)
        {
            var user = _userRepositoryBase.ExistingUser(productEntity.UserId);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.NotFound,
                    $"Пользователь {productEntity.UserId} не существует");

            var shop = _shopRepositoryBase.ExistingShop(productEntity.ShopId);
            if (shop == null)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.NotFound,
                    $"Магазин {productEntity.ShopId} не существует");

            var isModeratorInShop = _userRepositoryBase.IsModeratorInShop(productEntity.UserId, productEntity.ShopId);
            if (!isModeratorInShop & !user.Admin)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.NotFound,
                    "У вас нет прав на добавление товаров в этом магазин");
            
            var ifTypeExists = ProductEntity.ListOfTypes.Any(lt => lt.Equals(productEntity.Type));
            if (!ifTypeExists)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.NotFound,
                    $"Тип {productEntity.Type} не существует");

            var ifUseCaseExists = ProductEntity.ListOfUseCases.Any(uc => uc.Equals(productEntity.UseCase));
            if(!ifUseCaseExists)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.NotFound,
                    $"Способ применения {productEntity.UseCase} не существует");
            
            var ifWhereCanBeUsedExists = ProductEntity.ListOfWhereUsed
                .Any(wu => wu.Equals(productEntity.WhereUsed));
            if (!ifWhereCanBeUsedExists)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.NotFound,
                    $"Место применения {productEntity.WhereUsed} не существует");
            
            var ifMaterialExists = ProductEntity.ListOfMaterials.Any(lm => lm.Equals(productEntity.Material));
            if (!ifMaterialExists)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.NotFound,
                    $"Материал {productEntity.Material} не существует");
            
            var product = new Product()
            {
                UserId = productEntity.UserId,
                Name = productEntity.Name,
                Type = productEntity.Type,
                UseCase = productEntity.UseCase,
                WhereUsed = productEntity.WhereUsed,
                Material = productEntity.Material,
                Length = productEntity.Length,
                Width = productEntity.Width,
                Height = productEntity.Height,
                Price = productEntity.Price,
                InStockQuantity = productEntity.Quantity,
                CreationDate = DateTime.Now,
                ShopId = productEntity.ShopId,
                IsPublic = productEntity.IsPublic
            };

            if (productEntity.IsPublic) product.PublicationDate = DateTime.Now;

            _productRepositoryBase.Add(product);
            _productRepositoryBase.Save();
            
            return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.Ok, "Получилось");        
        }
        
        public KeyValuePair<StatusCodeEnum, string> ChangeCharacteristics(int userId, int productId, int? price,
            int? quantity, bool isPublic = true)
        {
            var product = _productRepositoryBase.ExistingProduct(productId);
            if (product == null)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.NotFound,
                    $"Продукт {productId} не найден");

            var user = _userRepositoryBase.ExistingUser(userId);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.NotFound,
                    $"Пользователь {userId} не найден");
                
            var isModeratorInShop = _userRepositoryBase.IsModeratorInShop(userId, product.ShopId);
            if (!isModeratorInShop & !user.Admin)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.NotFound,
                    "У вас нет прав на редактирование товаров в этом магазин");
            
            if (price != null) product.Price = price.Value;
            if (quantity != null) product.InStockQuantity = quantity.Value;
            if (product.IsPublic != isPublic) product.IsPublic = isPublic;
            
            _productRepositoryBase.Update(product);
            _productRepositoryBase.Save();
            
            return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.Ok, "Получилось");
        }

        public KeyValuePair<StatusCodeEnum, string> ChangePublicState(int userId, int productId)
        {
            var product = _productRepositoryBase.ExistingProduct(productId);
            if (product == null)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.NotFound,
                    $"Продукт {productId} не найден");

            var user = _userRepositoryBase.ExistingUser(userId);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.NotFound,
                    $"Пользователь {userId} не найден");
                
            var isModeratorInShop = _userRepositoryBase.IsModeratorInShop(userId, product.ShopId);
            if (!isModeratorInShop & !user.Admin)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.NotFound,
                    "У вас нет прав на редактирование товаров в этом магазин");

            product.IsPublic = !product.IsPublic;
            
            _productRepositoryBase.Update(product);
            _productRepositoryBase.Save();
            
            return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.Ok, "Получилось");
        }

        public KeyValuePair<StatusCodeEnum, string> RemoveProduct(int userId, int productId)
        {
            var product = _productRepositoryBase.ExistingProduct(productId);
            if (product == null)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.NotFound,
                    $"Продукт {productId} не найден");

            var user = _userRepositoryBase.ExistingUser(userId);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.NotFound,
                    $"Пользователь {userId} не найден");
                
            var isModeratorInShop = _userRepositoryBase.IsModeratorInShop(userId, product.ShopId);
            if (!isModeratorInShop & !user.Admin)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.NotFound,
                    "У вас нет прав на удаление товаров в этом магазин");
            
            _productRepositoryBase.Delete(product);
            _productRepositoryBase.Save();
            
            return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.Ok, "Получилось");
        }
    }
}