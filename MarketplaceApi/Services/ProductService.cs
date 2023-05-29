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

        private readonly UserRepository _userRepository;
        private readonly ProductRepository _productRepository;
        private readonly ShopRepository _shopRepository;

        public ProductService(MarketplaceContext context)
        {
            _userRepository = new UserRepository(context);
            _productRepository = new ProductRepository(context);
            _shopRepository = new ShopRepository(context);
        }

        public KeyValuePair<StatusCodeEnum, QueryableAndString<object>> GetProduct(int productId)
        {
            var product = _productRepository.ExistingProductsObj(productId);
            if (product == null)
                return new KeyValuePair<StatusCodeEnum, QueryableAndString<object>>
                    (StatusCodeEnum.BadRequest, new QueryableAndString<object>
                        (null, $"Товар {productId} не существует"));
            
            return new KeyValuePair<StatusCodeEnum, QueryableAndString<object>>
                (StatusCodeEnum.Ok, new QueryableAndString<object>(product, "Получилось"));
        }

        public KeyValuePair<StatusCodeEnum, QueryableAndString<object>> GetSimilar(int productId, int limit)
        {
            var product = _productRepository.ExistingProductsObj(productId).FirstOrDefault();
            if (product == null)
                return new KeyValuePair<StatusCodeEnum, QueryableAndString<object>>
                    (StatusCodeEnum.BadRequest, new QueryableAndString<object>
                        (null, $"Товар {productId} не существует"));

            var similarProducts = _productRepository
                .SimilarProducts(product, limit, PriceFluctuation, SizeFluctuation, VolumeFluctuation);

            return new KeyValuePair<StatusCodeEnum, QueryableAndString<object>>(StatusCodeEnum.Ok,
                new QueryableAndString<object>(similarProducts, "Получилось"));
        }

        public KeyValuePair<StatusCodeEnum, QueryableAndString<ProductView>> GetNewOfTheWeek(int limit)
        {
            var newProducts = _productRepository.NewInTimeInterval(7).Take(limit);
            
            return new KeyValuePair<StatusCodeEnum, QueryableAndString<ProductView>>(StatusCodeEnum.Ok,
                new QueryableAndString<ProductView>(newProducts, null));
        }

        public KeyValuePair<StatusCodeEnum, QueryableAndString<List<int>>> GetAttributes()
        {
            IEnumerable<List<int>> attributes = new[]
            {
                _productRepository.GetAllTypes(),
                _productRepository.GetAllUseCases(),
                _productRepository.GetAllWhereUsed(),
                _productRepository.GetAllMaterials()
            };
            
            return new KeyValuePair<StatusCodeEnum, QueryableAndString<List<int>>>(StatusCodeEnum.Ok,
                new QueryableAndString<List<int>>(attributes, "Получилось")); 
        }

        public KeyValuePair<StatusCodeEnum, QueryableAndString<ProductView>> Search(string name, int? type, int? useCase,
            int? whereUsed, int? material, int? minLength, int? maxLength, int? minWidth, int? maxWidth,
            int? minHeight, int? maxHeight, int? minPrice, int? maxPrice)
        {
            var products = _productRepository.SearchByAttributes(name, type, useCase, whereUsed, 
                material, minLength, maxLength, minWidth, maxWidth, minHeight, maxHeight, minPrice, maxPrice);
            
            return new KeyValuePair<StatusCodeEnum, QueryableAndString<ProductView>>(StatusCodeEnum.Ok,
                new QueryableAndString<ProductView>(products, "Получилось"));
        }

        public KeyValuePair<StatusCodeEnum, string> AddProduct(ProductEntity productEntity)
        {
            var user = _userRepository.ExistingUser(productEntity.UserId);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.BadRequest,
                    $"Пользователь {productEntity.UserId} не существует");

            var shop = _shopRepository.ExistingShop(productEntity.ShopId);
            if (shop == null)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.BadRequest,
                    $"Магазин {productEntity.ShopId} не существует");

            var isModeratorInShop = _userRepository.IsModeratorInShop(productEntity.UserId, productEntity.ShopId);
            if (!isModeratorInShop & !user.Admin)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.BadRequest,
                    "У вас нет прав на добавление товаров в этом магазин");
            
            var ifTypeExists = ProductEntity.ListOfTypes.Any(lt => lt.Equals(productEntity.Type));
            if (!ifTypeExists)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.BadRequest,
                    $"Тип {productEntity.Type} не существует");

            var ifUseCaseExists = ProductEntity.ListOfUseCases.Any(uc => uc.Equals(productEntity.UseCase));
            if(!ifUseCaseExists)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.BadRequest,
                    $"Способ применения {productEntity.UseCase} не существует");
            
            var ifWhereCanBeUsedExists = ProductEntity.ListOfWhereUsed
                .Any(wu => wu.Equals(productEntity.WhereUsed));
            if (!ifWhereCanBeUsedExists)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.BadRequest,
                    $"Место применения {productEntity.WhereUsed} не существует");
            
            var ifMaterialExists = ProductEntity.ListOfMaterials.Any(lm => lm.Equals(productEntity.Material));
            if (!ifMaterialExists)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.BadRequest,
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

            _productRepository.Add(product);
            _productRepository.Save();
            
            return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.Ok, "Получилось");        
        }
        
        public KeyValuePair<StatusCodeEnum, string> ChangeCharacteristics(int userId, int productId, int? price,
            int? quantity, bool isPublic = true)
        {
            var product = _productRepository.ExistingProduct(productId);
            if (product == null)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.BadRequest,
                    $"Продукт {productId} не найден");

            var user = _userRepository.ExistingUser(userId);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.BadRequest,
                    $"Пользователь {userId} не найден");
                
            var isModeratorInShop = _userRepository.IsModeratorInShop(userId, product.ShopId);
            if (!isModeratorInShop & !user.Admin)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.BadRequest,
                    "У вас нет прав на редактирование товаров в этом магазин");
            
            if (price != null) product.Price = price.Value;
            if (quantity != null) product.InStockQuantity = quantity.Value;
            if (product.IsPublic != isPublic) product.IsPublic = isPublic;
            
            _productRepository.Update(product);
            _productRepository.Save();
            
            return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.Ok, "Получилось");
        }

        public KeyValuePair<StatusCodeEnum, string> ChangePublicState(int userId, int productId)
        {
            var product = _productRepository.ExistingProduct(productId);
            if (product == null)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.BadRequest,
                    $"Продукт {productId} не найден");

            var user = _userRepository.ExistingUser(userId);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.BadRequest,
                    $"Пользователь {userId} не найден");
                
            var isModeratorInShop = _userRepository.IsModeratorInShop(userId, product.ShopId);
            if (!isModeratorInShop & !user.Admin)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.BadRequest,
                    "У вас нет прав на редактирование товаров в этом магазин");

            product.IsPublic = !product.IsPublic;
            
            _productRepository.Update(product);
            _productRepository.Save();
            
            return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.Ok, "Получилось");
        }

        public KeyValuePair<StatusCodeEnum, string> RemoveProduct(int userId, int productId)
        {
            var product = _productRepository.ExistingProduct(productId);
            if (product == null)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.BadRequest,
                    $"Продукт {productId} не найден");

            var user = _userRepository.ExistingUser(userId);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.BadRequest,
                    $"Пользователь {userId} не найден");
                
            var isModeratorInShop = _userRepository.IsModeratorInShop(userId, product.ShopId);
            if (!isModeratorInShop & !user.Admin)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.BadRequest,
                    "У вас нет прав на удаление товаров в этом магазин");
            
            _productRepository.Delete(product);
            _productRepository.Save();
            
            return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.Ok, "Получилось");
        }
    }
}