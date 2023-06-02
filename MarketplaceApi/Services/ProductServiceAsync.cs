using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MarketplaceApi.IRepositories;
using MarketplaceApi.IServices;
using MarketplaceApi.Models;
using MarketplaceApi.Repositories;
using Xunit.Sdk;
using ArgumentNullException = System.ArgumentNullException;
using UnauthorizedAccessException = System.UnauthorizedAccessException;

namespace MarketplaceApi.Services
{
    public class ProductServiceAsync : IProductServiceAsync
    {
        private const decimal PriceFluctuation = 0.1M;
        private const decimal SizeFluctuation = 0.1M;
        private const decimal VolumeFluctuation = 0.1M;
        
        private readonly IUserRepository _userRepository;
        //private readonly ProductRepository _productRepository;
        private readonly IShopRepository _shopRepository;        
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        public ProductServiceAsync(IMapper mapper, IProductRepository productRepository, IUserRepository userRepository, IShopRepository shopRepository)
        {
            _userRepository = userRepository;
            _productRepository = productRepository;
            _shopRepository = shopRepository;
            _mapper = mapper;
        }

        public async Task AddProductAsync(ProductEntity productEntity)
        {
            var userTask = _userRepository.ExistingUserAsync(productEntity.UserId);

            var shopTask = _shopRepository.ExistingShopAsync(productEntity.ShopId);

            if (await userTask== null)
                throw new ArgumentNullException($"Пользователь {productEntity.UserId} не существует");
            if (await shopTask == null)
                throw new ArgumentNullException($"Магазин {productEntity.ShopId} не существует");
            
            var isModeratorInShopTask = await _userRepository.IsModeratorInShopAsync(productEntity.UserId, productEntity.ShopId);
            if (!isModeratorInShopTask & !userTask.Result.Admin)
                throw new UnauthorizedAccessException("У вас нет прав на добавление товаров в этом магазин");

            var ifTypeExists = ProductEntity.ListOfTypes.Any(lt => lt.Equals(productEntity.Type));
            if (!ifTypeExists)
                throw new InvalidDataException($"Тип {productEntity.Type} не существует");
            
            var ifUseCaseExists = ProductEntity.ListOfUseCases.Any(uc => uc.Equals(productEntity.UseCase));
            if (!ifUseCaseExists)
                throw new InvalidDataException($"Тип {productEntity.UseCase} не существует");
            
            var ifWhereCanBeUsedExists = ProductEntity.ListOfWhereUsed
                .Any(wu => wu.Equals(productEntity.WhereUsed));
            if (!ifWhereCanBeUsedExists)
                throw new InvalidDataException($"Место применения {productEntity.WhereUsed} не существует");
            
            var ifMaterialExists = ProductEntity.ListOfMaterials.Any(lm => lm.Equals(productEntity.Material));
            if (!ifMaterialExists)
                throw new InvalidDataException($"Материал {productEntity.Material} не существует");
            
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
        }
    }
}