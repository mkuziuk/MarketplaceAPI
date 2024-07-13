using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MarketplaceApi.Enums;
using MarketplaceApi.Models;
using MarketplaceApi.Repositories;
using MarketplaceApi.ViewModels;

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
        private readonly IMapper _mapper;
        public ProductService(MarketplaceContext context, IMapper mapper)
        {
            _userRepository = new UserRepository(context);
            _productRepository = new ProductRepository(context);
            _shopRepository = new ShopRepository(context);
            _mapper = mapper;
        }

        public KeyValuePair<StatusCodeEnum, EnumerableAndString<ProductView>> GetProduct(int productId)
        {
            var product = _productRepository.ExistingProducts(productId);
            if (product.FirstOrDefault() == null)
                return new KeyValuePair<StatusCodeEnum, EnumerableAndString<ProductView>>
                    (StatusCodeEnum.NotFound, new EnumerableAndString<ProductView>
                        (null, $"Товар {productId} не существует"));
            
            var productView = _mapper.ProjectTo<ProductView>(product);
            
            return new KeyValuePair<StatusCodeEnum, EnumerableAndString<ProductView>>
                (StatusCodeEnum.Ok, new EnumerableAndString<ProductView>(productView, "Получилось"));
        }

        public KeyValuePair<StatusCodeEnum, EnumerableAndString<ProductView>> GetSimilar(int productId, int limit)
        {
            var product = _productRepository.ExistingProduct(productId);
            var productView = _mapper.Map<ProductView>(product);
            if (product == null)
                return new KeyValuePair<StatusCodeEnum, EnumerableAndString<ProductView>>
                    (StatusCodeEnum.NotFound, new EnumerableAndString<ProductView>
                        (null, $"Товар {productId} не существует"));

            var similarProducts = _productRepository
                .SimilarProducts(productView, limit, PriceFluctuation, SizeFluctuation, VolumeFluctuation);
            
            if (similarProducts.FirstOrDefault() == null)
                return new KeyValuePair<StatusCodeEnum, EnumerableAndString<ProductView>>
                (StatusCodeEnum.NotFound, new EnumerableAndString<ProductView>
                    (null, $"Товаров похожих на {productId} нет"));
            
            var similarProductsView = _mapper.ProjectTo<ProductView>(similarProducts);

            return new KeyValuePair<StatusCodeEnum, EnumerableAndString<ProductView>>(StatusCodeEnum.Ok,
                new EnumerableAndString<ProductView>(similarProductsView, "Получилось"));
        }

        public KeyValuePair<StatusCodeEnum, EnumerableAndString<ProductView>> GetNewOfTheWeek(int limit)
        {
            var newProducts = _productRepository.NewInTimeInterval(7).Take(limit);
            var newProductsView = _mapper.ProjectTo<ProductView>(newProducts);
            
            return new KeyValuePair<StatusCodeEnum, EnumerableAndString<ProductView>>(StatusCodeEnum.Ok,
                new EnumerableAndString<ProductView>(newProductsView, null));
        }

        public KeyValuePair<StatusCodeEnum, EnumerableAndString<List<int>>> GetAttributes()
        {
            IEnumerable<List<int>> attributes = new[]
            {
                _productRepository.GetAllTypes(),
                _productRepository.GetAllUseCases(),
                _productRepository.GetAllWhereUsed(),
                _productRepository.GetAllMaterials()
            };
            
            return new KeyValuePair<StatusCodeEnum, EnumerableAndString<List<int>>>(StatusCodeEnum.Ok,
                new EnumerableAndString<List<int>>(attributes, "Получилось")); 
        }

        public KeyValuePair<StatusCodeEnum, EnumerableAndString<ProductView>> Search(string name, int? type, int? useCase,
            int? whereUsed, int? material, int? minLength, int? maxLength, int? minWidth, int? maxWidth,
            int? minHeight, int? maxHeight, int? minPrice, int? maxPrice)
        {
            var products = _productRepository.SearchByAttributes(name, type, useCase, whereUsed, 
                material, minLength, maxLength, minWidth, maxWidth, minHeight, maxHeight, minPrice, maxPrice);
            var productsView = _mapper.ProjectTo<ProductView>(products);
            
            return new KeyValuePair<StatusCodeEnum, EnumerableAndString<ProductView>>(StatusCodeEnum.Ok,
                new EnumerableAndString<ProductView>(productsView, "Получилось"));
        }

        public KeyValuePair<StatusCodeEnum, string> AddProduct(ProductEntity productEntity)
        {
            var user = _userRepository.ExistingUser(productEntity.UserId);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.NotFound,
                    $"Пользователь {productEntity.UserId} не существует");

            var shop = _shopRepository.ExistingShop(productEntity.ShopId);
            if (shop == null)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.NotFound,
                    $"Магазин {productEntity.ShopId} не существует");

            var isModeratorInShop = _userRepository.IsModeratorInShop(productEntity.UserId, productEntity.ShopId);
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

            _productRepository.Add(product);
            _productRepository.Save();
            
            return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.Ok, "Получилось");        
        }
        
        public KeyValuePair<StatusCodeEnum, string> ChangeCharacteristics(int userId, int productId, int? price,
            int? quantity, bool isPublic = true)
        {
            var product = _productRepository.ExistingProduct(productId);
            if (product == null)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.NotFound,
                    $"Продукт {productId} не найден");

            var user = _userRepository.ExistingUser(userId);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.NotFound,
                    $"Пользователь {userId} не найден");
                
            var isModeratorInShop = _userRepository.IsModeratorInShop(userId, product.ShopId);
            if (!isModeratorInShop & !user.Admin)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.NotFound,
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
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.NotFound,
                    $"Продукт {productId} не найден");

            var user = _userRepository.ExistingUser(userId);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.NotFound,
                    $"Пользователь {userId} не найден");
                
            var isModeratorInShop = _userRepository.IsModeratorInShop(userId, product.ShopId);
            if (!isModeratorInShop & !user.Admin)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.NotFound,
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
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.NotFound,
                    $"Продукт {productId} не найден");

            var user = _userRepository.ExistingUser(userId);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.NotFound,
                    $"Пользователь {userId} не найден");
                
            var isModeratorInShop = _userRepository.IsModeratorInShop(userId, product.ShopId);
            if (!isModeratorInShop & !user.Admin)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.NotFound,
                    "У вас нет прав на удаление товаров в этом магазин");
            
            _productRepository.Delete(product);
            _productRepository.Save();
            
            return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.Ok, "Получилось");
        }
    }
}